using Microsoft.AspNetCore.Identity;

namespace Auth.Models;

public class User : IdentityUser
{
      public override string? UserName { get; set; }
      public override string? Email { get;  set; }
}