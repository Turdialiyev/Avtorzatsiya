

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Auth.Dtos;
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

    [HttpPost("SiginUp")]
    public async Task<IActionResult> Register([FromBody]User user)
    {
        var userModel = ToModel(user);

        var result = await _userManager.CreateAsync(userModel, user.Password);

        if (result.Succeeded)
            return Ok();

        return Unauthorized();
    }

    private Auth.Models.User ToModel(User user)
    => new()
    {
        UserName = user.UserName,
        Email = user.Email,
        PasswordHash = user.Password,
    };

    [HttpPost("SiginIn")]
    public async Task<IActionResult> Login(User user)
    {
        var userModel = await _userManager.FindByNameAsync(user.UserName);

        if (user is null)
            return Unauthorized();

        await _signInManager.SignOutAsync();

        var signInResult = await _signInManager.PasswordSignInAsync(ToModel(user), userModel.PasswordHash, false, false);

        // if (!signInResult.Succeeded)
        //     return Unauthorized();

        List<Claim> claims = new List<Claim>{

            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.Email, user.Email!),
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

        return Ok(token);
    }

    [HttpPost("logOut")]
    public async Task<IActionResult> LogOut()
    {
        await _signInManager.SignOutAsync();
        return Ok();
    }
}