using AutoMapper;
using EliteSoftTask.Data.Database;
using EliteSoftTask.Data.Database.Entities;
using EliteSoftTask.Data.DTOs;
using EliteSoftTask.Http.Requests;
using EliteSoftTask.Http.Responses;
using EliteSoftTask.Services.Utils;
using Microsoft.EntityFrameworkCore;
using HttpResponse = EliteSoftTask.Services.Utils.HttpResponse;
using FreeIPA.DotNet;
using FreeIPA.DotNet.Models.Login;
using FreeIPA.DotNet.Models.RPC;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace EliteSoftTask.Services;

public class AuthService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly TokenService _tokenService;
    private readonly IMapper _mapper;
    private readonly DbSet<User> _userRepo;
    private readonly IpaClient _ipaClient;

    public AuthService(ApplicationDbContext dbContext,TokenService tokenService,IMapper mapper,IConfiguration configuration)
    {
        _dbContext = dbContext;
        _tokenService = tokenService;
        _userRepo = dbContext.Users;
        _mapper = mapper;
        _ipaClient = new IpaClient(configuration["IPA_Server"]);
    }

    public async Task<ServiceResponse<AuthResponse, HttpResponse>> Register(RegisterRequest request)
    {
        var isUsernameExisted = await _userRepo.Where(u => u.Username == request.Username).AnyAsync();
        if (isUsernameExisted)
        {
            return ServiceResponse<AuthResponse, HttpResponse>.ActionFail(new HttpResponse()
                { StatusCode = 401, Message = "Email Already Existed" });
        }

        var user = (await _userRepo.AddAsync(new User
        {
            Username = request.Username,
            Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
            AuthSource = User.AuthenticationSource.Db,
        })).Entity;
        await _dbContext.SaveChangesAsync();
        var token = _tokenService.CreateToken(user).Success;
        return ServiceResponse<AuthResponse, HttpResponse>.ActionSuccess(RespondWithToken(user,token));
    }

    public async Task<ServiceResponse<AuthResponse, HttpResponse>> Login(LoginRequest request)
    {
        var user = await _userRepo.Where(u => u.Username == request.Username).SingleOrDefaultAsync();
        var userDbAttempt = user?.AuthSource == User.AuthenticationSource.Db && !request.IsFreeIPA &&
                            BCrypt.Net.BCrypt.Verify(request.Password, user?.Password);
        string token;
        if (userDbAttempt)
        {
            token = _tokenService.CreateToken(user).Success;
            return ServiceResponse<AuthResponse, HttpResponse>.ActionSuccess(RespondWithToken(user,token));
        }

        try
        {
             await _ipaClient.LoginWithPassword(new IpaLoginRequestModel {Password = request.Password,Username = request.Username});
             user = (await _userRepo.AddAsync(new User
             {
                 Username = request.Username,
                 Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                 AuthSource = User.AuthenticationSource.FreeIpa
             })).Entity;
             await _dbContext.SaveChangesAsync();
        }
        catch (Exception)
        {
            return ServiceResponse<AuthResponse, HttpResponse>.ActionFail(new HttpResponse()
            {
                Message = "User not found in FreeIPA or in System database",
                StatusCode = 401
            });
        }
        
        
        var rpcUserResponse = await _ipaClient.SendRpcRequest(new IpaRpcRequestModel
        {
            Id = 0,
            Method = "user_show/1",
            Parameters = [ new[]{request.Username},new {all=true,version="2.253"}],
            Version = ""
        });
        var rpcUserData = JsonConvert.DeserializeObject<FreeIpaUserRpcResponse>(JsonConvert.SerializeObject(rpcUserResponse.Data.Result));
        token = _tokenService.CreateToken(user,rpcUserData?.Result.UidNumber[0]).Success;
        return ServiceResponse<AuthResponse, HttpResponse>.ActionSuccess(RespondWithToken(user, token));
    }

    private AuthResponse RespondWithToken(User user, string token)
    {
        return new AuthResponse
        {
            User = _mapper.Map<UserDTO>(user),
            Token = token
        };
    }
}