using EliteSoftTask.Data.Database.Entities;

namespace EliteSoftTask.Data.DTOs;

public record UserDTO
{
    public long Id { get; set; }
    public string Username { get; set; }
    public User.AuthenticationSource AuthSource { get; set; }
}