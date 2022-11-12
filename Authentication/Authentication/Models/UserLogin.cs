using System.ComponentModel.DataAnnotations;

namespace Auth.Models;

public class UserLogin
{
    [Required]
    public string? UserName { get; set; }
    [Required]
    [StringLength(30, MinimumLength = 8, ErrorMessage = "Password must be a string with a minimum length of 8 and a maximum length of 30")]
    public string? Password { get; set; }
}