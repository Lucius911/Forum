using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Forum.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Forum.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class AccountController(
    UserManager<IdentityUser> userManager,
    SignInManager<IdentityUser> signInManager,
    IConfiguration configuration,
    IOptions<JwtSettings> jwtSettings)
    : ControllerBase
  {
    private readonly UserManager<IdentityUser> _userManager = userManager;
    private readonly SignInManager<IdentityUser> _signInManager = signInManager;
    private readonly IConfiguration _configuration = configuration;
    private readonly IOptions<JwtSettings> _jwtSettings = jwtSettings;

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }
      var user = new IdentityUser
      {
        UserName = model.DisplayName,
        Email = model.Email,
      };
      var result = await _userManager.CreateAsync(user, model.Password);
      if (result.Succeeded)
      {
        return Ok(new { Message = "User registered successfully." });
      }
      return BadRequest(result.Errors);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
      if (!result.Succeeded)
        return Unauthorized();


      var user = await _userManager.FindByEmailAsync(model.Email);
      var token = GenerateJwtToken(user);
      return Ok(new
      {
        Token = token, 
        Expiration = token.ValidTo
      });

    }

    private JwtSecurityToken GenerateJwtToken(IdentityUser user)
    {
      //Create our Jwt for auth 
      var authClaims = new List<Claim>
      {
        new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
      };

      var authSignKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_jwtSettings.Value.Key));

      return new JwtSecurityToken(
        issuer: _jwtSettings.Value.Issuer,
        audience: _jwtSettings.Value.Audience,
        expires: DateTime.Now.AddMinutes(60),
        claims: authClaims,
        signingCredentials: new SigningCredentials(authSignKey, SecurityAlgorithms.HmacSha256)
      );
    }
  }
}
