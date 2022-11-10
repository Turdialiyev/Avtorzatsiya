

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
    public async Task<IActionResult> Register([FromBody] User user)
    {
        var result = new Result { Data = null, ErrorMessage = "Success", StatusCode = 200 };
        var userModel = ToModel(user);

        var createdUser = await _userManager.CreateAsync(userModel, user.Password);

        if (createdUser.Succeeded)
            return Ok(result);

        result.ErrorMessage = "UnSuccess";
        result.StatusCode = 401;
        return Unauthorized(result);
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
        var result = new Result { Data = null, ErrorMessage = "UnSuccess", StatusCode = 401 };


        if (user is null)
            return Unauthorized(result);

        await _signInManager.SignOutAsync();

        // var signInResult = await _signInManager.PasswordSignInAsync(ToModel(user), userModel.PasswordHash, false, false);

        // if (!signInResult.Succeeded)
        //     _logger.LogInformation("=========> ");
        // return Unauthorized();

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
        result.Data = token;
        result.ErrorMessage = "Success";
        result.StatusCode = 201;
        
        return Ok(result);
    }

    [HttpPost("logOut")]
    public async Task<IActionResult> LogOut()
    {
        await _signInManager.SignOutAsync();
        return Ok();
    }
}