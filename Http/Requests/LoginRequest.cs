using System.ComponentModel.DataAnnotations;

namespace EliteSoftTask.Http.Requests;

public record LoginRequest
{
    [Required]
    public string Username { get; init; }
    [Required]
    public string Password { get; init; }

    public bool IsFreeIPA { get; init; } = false;
}