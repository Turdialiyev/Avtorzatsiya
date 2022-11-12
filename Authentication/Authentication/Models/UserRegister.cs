using System.ComponentModel.DataAnnotations;

namespace Auth.Models;

public class UserRegiter
{
    [Required]
    public string? UserName { get; set; }
    [Required]
    [EmailAddress]
    [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
    public string? Email { get; set; }
    [Required]
    [StringLength(30, MinimumLength = 8, ErrorMessage = "Password must be a string with a minimum length of 8 and a maximum length of 30")]
    public string? Password { get; set; }
}