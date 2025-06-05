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
    IOptions<JwtSettings> jwtSettings)
    : ControllerBase
  {
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;

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
      var result = await userManager.CreateAsync(user, model.Password);
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

      var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
      if (!result.Succeeded)
        return Unauthorized();


      var user = await userManager.FindByEmailAsync(model.Email);
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

      var authSignKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_jwtSettings.Key));

      return new JwtSecurityToken(
        issuer: jwtSettings.Value.Issuer,
        audience: jwtSettings.Value.Audience,
        expires: DateTime.Now.AddMinutes(60),
        claims: authClaims,
        signingCredentials: new SigningCredentials(authSignKey, SecurityAlgorithms.HmacSha256)
      );
    }
  }
}
