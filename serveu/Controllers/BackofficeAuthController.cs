using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using serveu.Context;
using serveu.Dtos;
using serveu.Interceptor;
using serveu.Models;
using ServeU.Utils;


namespace serveu.Controllers;

[Route("/api/back-office/auth")]
[ApiController()]
[ServiceFilter(typeof(ApiResponseFormatFilter))]
public class BackOfficeAuthController : ControllerBase
{


    private readonly AppDbContext appDbContext;
    private readonly UserManager<ApplicationUser> userManager;
    private RoleManager<IdentityRole> roleManager;
    private readonly IConfiguration configuration;

    private readonly IMapper mapper;
    private readonly AuthUtils authUtils;
    public BackOfficeAuthController(AppDbContext _appDbContext, UserManager<ApplicationUser> _userManager, IConfiguration _configuration, RoleManager<IdentityRole> _roleManager, IMapper _mapper, AuthUtils _authUtils)
    {
        appDbContext = _appDbContext;
        userManager = _userManager;
        configuration = _configuration;
        roleManager = _roleManager;
        mapper = _mapper;
        authUtils = _authUtils;
    }


    [HttpGet("info")]
    [Authorize]
    public async Task<ActionResult<ApplicationUserDto>> getInfo()
    {
        var foundUser = await authUtils.getAuthenticatedUserAsync(HttpContext.User);
        return mapper.Map<ApplicationUserDto>(foundUser);
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
            PhoneNumber = signUpDto.PhoneNumber,
            Role = UserRole.ADMIN
        }, signUpDto.Password);

        if (useIdentity.Succeeded)
        {
            if (!await roleManager.RoleExistsAsync(UserRole.ADMIN))
            {
                await roleManager.CreateAsync(new IdentityRole(UserRole.ADMIN));
            }

            var createdUser = await userManager.FindByEmailAsync(signUpDto.Email);
            await userManager.AddToRoleAsync(createdUser, UserRole.ADMIN);
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
                User = new Dtos.ApplicationUserDto()
                {
                    Email = createdUser.Email,
                    Name = createdUser.Name,
                    Role = UserRole.ADMIN,
                    IsVerified = false,
                },
                AccessToken = token
            });
        }

        return Unauthorized(useIdentity.Errors.First().Description);
    }


    [HttpPost("sign-in")]
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
            User = new Dtos.ApplicationUserDto()
            {
                Email = foundUser.Email,
                IsVerified = true,
                Name = foundUser.Name,
            },
            AccessToken = token
        });
    }


    private string GenerateToken(IEnumerable<Claim> claims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTKey:Secret"]));
        var _TokenExpiryTimeInHour = Convert.ToInt64(configuration["JWTKey:TokenExpiryTimeInHour"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = configuration["JWTKey:ValidIssuer"],
            Audience = configuration["JWTKey:ValidAudience"],
            Expires = DateTime.UtcNow.AddHours(_TokenExpiryTimeInHour),
            // Expires = DateTime.UtcNow.AddMinutes(60),
            SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
            Subject = new ClaimsIdentity(claims)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

}