using EliteSoftTask.Data.DTOs;

namespace EliteSoftTask.Http.Responses;

public record AuthResponse
{
    public UserDTO User { get; init; }
    public string Token { get; init; }
}