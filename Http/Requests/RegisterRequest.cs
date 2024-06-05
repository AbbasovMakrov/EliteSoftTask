using System.ComponentModel.DataAnnotations;

namespace EliteSoftTask.Http.Requests;

public record RegisterRequest
{
    [Required]
    public string Username { get; init; }
    [Required]
    public string Password { get; init; }
    
}