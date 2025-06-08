using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Forum.DTOs;
using Forum.DTOs.Account;
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
    RoleManager<IdentityRole> roleManager,
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
        if (model.IsModerator)
        {
          await CheckAndAddUserToRole(user);
        }

        return Ok(new { Message = "User registered successfully." });
      }
      return BadRequest(result.Errors);
    }

    private async Task CheckAndAddUserToRole(IdentityUser user)
    {
      var roleExists = await roleManager.RoleExistsAsync("Moderator");

      if (!roleExists)
      {
        await roleManager.CreateAsync(new IdentityRole("Moderator"));
      }

      await userManager.AddToRoleAsync(user, "Moderator");
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }
      var user = await userManager.FindByEmailAsync(model.Email);

      if (user == null)
      {
        return this.Unauthorized("User does not exist, Please register");
      }

      var result = await signInManager.PasswordSignInAsync(user.UserName!, model.Password, false, false);
      if (!result.Succeeded)
        return Unauthorized();

      var token = await GenerateJwtToken(user);

      // return for ease of use 
      return this.Ok(new JwtSecurityTokenHandler().WriteToken(token));
      //return Ok(new
      //{
      //  Token = token, 
      //  Expiration = token.ValidTo
      //});
    }

    private async Task<JwtSecurityToken> GenerateJwtToken(IdentityUser user)
    {
      //Create our Jwt for auth 
      var authClaims = new List<Claim>
      {
        new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
      };

      var isModerator = await userManager.IsInRoleAsync(user, "Moderator");

      if (isModerator)
      {
        // hardcoding due to time contraint
        authClaims.Add(new Claim(ClaimTypes.Role, "Moderator"));
      }

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
