using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using serveu.Context;
using serveu.Interceptor;
using serveu.Models;


namespace serveu.Controllers;

[Route("[controller]")]
[ApiController()]
[ServiceFilter(typeof(ApiResponseFormatFilter))]
public class AuthController : ControllerBase
{


    private readonly AppDbContext appDbContext;
    private readonly UserManager<ApplicationUser> userManager;
    private RoleManager<IdentityRole> roleManager;
    private readonly IConfiguration configuration;



    public AuthController(AppDbContext _appDbContext, UserManager<ApplicationUser> _userManager, IConfiguration _configuration, RoleManager<IdentityRole> _roleManager)
    {
        appDbContext = _appDbContext;
        userManager = _userManager;
        configuration = _configuration;
        roleManager = _roleManager;
    }


    [HttpPost("sign-up")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<LoginResponseDto>> SignUp(SignUpDto signUpDto)
    {
        var foundUserByEmail = await userManager.FindByEmailAsync(signUpDto.Email);

        if (foundUserByEmail != null)
        {
            return Unauthorized("User already exists!");
        }



        var useIdentity = await userManager.CreateAsync(new ApplicationUser()
        {
            Email = signUpDto.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = signUpDto.Email,
            Name = signUpDto.Name,

        }, signUpDto.Password);



        if (useIdentity.Succeeded)
        {
            if (!await roleManager.RoleExistsAsync(signUpDto.role))
            {
                await roleManager.CreateAsync(new IdentityRole(signUpDto.role));
            }

            var createdUser = await userManager.FindByEmailAsync(signUpDto.Email);
            await userManager.AddToRoleAsync(createdUser, signUpDto.role);
            var userRoles = await userManager.GetRolesAsync(createdUser);
            var authClaims = new List<Claim>
            {
               new Claim(ClaimTypes.Name, createdUser.UserName),
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }
            string token = GenerateToken(authClaims);


            return Ok(new LoginResponseDto()
            {
                user = new Dtos.ApplicationUserDto()
                {
                    email = createdUser.Email,
                    name = createdUser.Name,
                    isVerified = true,
                    role = userRoles.ToArray().Length == 0 ? null : userRoles.ToArray()[0]
                },
                authToken = token
            });
        }

        return Unauthorized(useIdentity.Errors.First().Description);
    }


    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<LoginResponseDto>> Login(LoginDto loginDto)
    {

        var foundUser = await userManager.FindByEmailAsync(loginDto.Email);

        if (foundUser == null || !await userManager.CheckPasswordAsync(foundUser, loginDto.Password))
        {
            return Unauthorized("Invalid password or email");

        }
        var userRoles = await userManager.GetRolesAsync(foundUser);
        var authClaims = new List<Claim>
            {
               new Claim(ClaimTypes.Name, foundUser.UserName),
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

        foreach (var userRole in userRoles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, userRole));
        }
        string token = GenerateToken(authClaims);

        return Ok(new LoginResponseDto()
        {
            user = new Dtos.ApplicationUserDto()
            {
                email = foundUser.Email,
                isVerified = true,
                name = foundUser.Name,
                role = userRoles.ToArray().Length == 0 ? null : userRoles.ToArray()[0]
            },
            authToken = token
        });
    }


    private string GenerateToken(IEnumerable<Claim> claims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTKey:Secret"]));
        // var _TokenExpiryTimeInHour = Convert.ToInt64(configuration["JWTKey:TokenExpiryTimeInHour"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = configuration["JWTKey:ValidIssuer"],
            Audience = configuration["JWTKey:ValidAudience"],
            //Expires = DateTime.UtcNow.AddHours(_TokenExpiryTimeInHour),
            Expires = DateTime.UtcNow.AddMinutes(1),
            SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
            Subject = new ClaimsIdentity(claims)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

}