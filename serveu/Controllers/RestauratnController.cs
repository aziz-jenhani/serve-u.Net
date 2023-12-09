using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using serveu.Context;
using serveu.Dtos;
using serveu.Interceptor;
using serveu.Models;


namespace serveu.Controllers;

[Route("/api/back-office/restaurants")]
[ApiController()]
[ServiceFilter(typeof(ApiResponseFormatFilter))]
[Authorize(Roles = UserRole.ADMIN)]
public class RestaurantController : ControllerBase
{

    private readonly AppDbContext appDbContext;
    private readonly IMapper mapper;
    private readonly UserManager<ApplicationUser> userManager;

    public RestaurantController(AppDbContext _appDbContext, UserManager<ApplicationUser> _userManager, IMapper _mapper, RoleManager<IdentityRole> _roleManager)
    {
        appDbContext = _appDbContext;
        userManager = _userManager;
        mapper = _mapper;
    }


    [HttpGet]
    public async Task<ActionResult<PaginatedItemsDto<ApplicationUserDto>>> GetMany([FromQuery] int page = 1, [FromQuery] int limit = 10)
    {

        var users = await userManager.Users
                        .Where(u => u.Role == UserRole.RESTAURANT)
                        .Skip((page - 1) * limit)
                        .Take(limit)
                        .ToListAsync();


        var totalItems = await userManager.Users.Where(u => u.Role == UserRole.RESTAURANT).CountAsync();

        return Ok(new PaginatedItemsDto<ApplicationUserDto>()
        {
            Items = mapper.Map<List<ApplicationUserDto>>(users),
            Meta = new PaginatedItemsMetaDto()
            {
                TotalItems = totalItems,
                ItemCount = users.Count,
                ItemsPerPage = limit,
                TotalPages = (int)Math.Ceiling((double)totalItems / limit),
                CurrentPage = page
            }
        });

    }

    [HttpGet("{Id}")]
    public async Task<ActionResult<ApplicationUserDto>> GetOne(string Id)
    {

        var foundUser = await userManager.FindByIdAsync(Id);


        if (foundUser == null)
        {
            return NotFound("User not found!");
        }

        return Ok(mapper.Map<ApplicationUserDto>(foundUser));

    }

    [HttpDelete("{Id}")]
    public async Task<ActionResult> DeleteOne(string Id)
    {

        var foundUser = await userManager.FindByIdAsync(Id);

        if (foundUser == null)
        {
            return NotFound("User not found!");
        }

        await userManager.DeleteAsync(foundUser);


        return NoContent();

    }

    [HttpPost()]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApplicationUserDto>> CreateOne(CreateRestaurantDto user)
    {

        var foundUserByEmail = await userManager.FindByEmailAsync(user.Email);

        if (foundUserByEmail != null)
        {
            return BadRequest("User already exists!");
        }


        var useIdentity = await userManager.CreateAsync(new ApplicationUser()
        {
            Email = user.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = user.Email,
            Name = user.Name,
            PhoneNumber = user.PhoneNumber,
            Role = UserRole.RESTAURANT,
            EmailConfirmed = user.IsVerified
        }, user.Password);

        if (useIdentity.Succeeded)
        {
            var createdUser = await userManager.FindByEmailAsync(user.Email);
            await userManager.AddToRoleAsync(createdUser, UserRole.RESTAURANT);

            return Ok(mapper.Map<ApplicationUserDto>(createdUser));
        }
        return Unauthorized(useIdentity.Errors.First().Description);
    }

    [HttpPut()]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApplicationUserDto>> UpdateOne(UpdateRestaurantDto user)
    {

        var foundUser = await userManager.FindByIdAsync(user.Id);

        if (foundUser == null)
        {
            return NotFound("User not found!");
        }

        foundUser.Email = user.Email;
        foundUser.UserName = user.Email;
        foundUser.Name = user.Name;
        foundUser.PhoneNumber = user.PhoneNumber;

        foundUser.EmailConfirmed = user.IsVerified;


        var useIdentity = await userManager.UpdateAsync(foundUser);

        if (useIdentity.Succeeded)
        {
            return Ok(mapper.Map<ApplicationUserDto>(foundUser));
        }
        return Unauthorized(useIdentity.Errors.First().Description);
    }
}