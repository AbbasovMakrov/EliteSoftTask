using AutoMapper;
using EliteSoftTask.Data.Database.Entities;

namespace EliteSoftTask.Data.DTOs;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDTO>();
    }
}