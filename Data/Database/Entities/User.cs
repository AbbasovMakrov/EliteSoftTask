using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace EliteSoftTask.Data.Database.Entities;

public class User
{
    public enum AuthenticationSource
    {
        Db=1,
        FreeIpa
    }
    [Key]
    public long Id { get; set; }
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public AuthenticationSource AuthSource { get; set; }
}