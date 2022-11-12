

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Auth.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private ILogger<AuthController> _logger;
    private UserManager<IdentityUser> _userManager;
    private SignInManager<IdentityUser> _signInManager;
    private RoleManager<IdentityRole> _roleManager;

    public AuthController(
            ILogger<AuthController> logger,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager)
    {
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }

    [HttpPost("SignUp")]
    public async Task<IActionResult> Register([FromBody] UserRegiter user)
    {

        var result = new Result<string>();

        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var existUser = _userManager.FindByNameAsync(user.UserName);

        if (existUser.Result is not null)
        {
            result.Error = new Error { Code = 1, Message = "UserName Already exist" };
            return Ok(result);
        }

        var userModel = new User
        {
            UserName = user.UserName,
            PasswordHash = user.Password,
            Email = user.Email
        };

        var createdUser = await _userManager.CreateAsync(userModel, user.Password);

        if (createdUser.Succeeded)
        {
            result.Error = new Error { Code = 0, Message = "Success" };

            return Ok(result);
        }

        result.Error = new Error { Code = 4, Message = "Unexpected error" };

        return Ok(result);
    }

    [HttpPost("SignIn")]
    public async Task<IActionResult> Login([FromBody] UserLogin user)
    {
        var result = new Result<string>();

        if (!ModelState.IsValid)
            return BadRequest();

        var existUser = await _userManager.FindByNameAsync(user.UserName);

        if (existUser is null)
        {
            result.Error = new Error { Code = 2, Message = "UserName is not valid" };

            return Ok(result);
        }

        var signInResult = await _signInManager.CheckPasswordSignInAsync(existUser, user.Password, false);

        if (!signInResult.Succeeded)
        {
            result.Error = new Error { Code = 3, Message = "Password is invalid" };

            return Ok(result);
        }

        List<Claim> claims = new List<Claim>{

            new Claim(ClaimTypes.Name, user.UserName!),
        };

        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("salom#2_saSad_@sasa:Asasasldlnl"));

        var signInCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        var tokenOptions = new JwtSecurityToken(
            issuer: "https://localhost:44342",
            audience: "https://localhost:44342",
            claims: claims,
            signingCredentials: signInCredentials
        );

        var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        result.Data = token;
        result.Error = new Error { Code = 0, Message = "Success" };

        return Ok(result);
    }

    [HttpPost("logOut")]
    public async Task<IActionResult> LogOut()
    {
        await _signInManager.SignOutAsync();
        return Ok();
    }
}