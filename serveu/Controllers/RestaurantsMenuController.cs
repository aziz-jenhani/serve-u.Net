using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using serveu.Context;
using serveu.Dtos;
using serveu.Interceptor;


namespace serveu.Controllers;

[Route("/api/mobile/restaurants")]
[ApiController()]
[ServiceFilter(typeof(ApiResponseFormatFilter))]
public class RestaurantMenuController : ControllerBase
{

    private readonly AppDbContext appDbContext;
    private readonly IMapper mapper;

    public RestaurantMenuController(AppDbContext _appDbContext, IMapper _mapper)
    {
        appDbContext = _appDbContext;
        mapper = _mapper;
    }




    [HttpGet("{Id}")]
    public async Task<ActionResult<ApplicationUserDto>> GetOne(string Id)
    {

        var foundRestaurant = await appDbContext.Users
        .Where(u => u.Id == Id)
        .Include((res) => res.MenuItems)
        .ThenInclude(menuItem => menuItem.Category)
           .Include((res) => res.MenuItems)
        .ThenInclude(menuItem => menuItem.Image)
        .FirstAsync();


        if (foundRestaurant == null)
        {
            return NotFound("Restaurant not found!");
        }

        return Ok(new ApplicationUserDto()
        {
            Id = foundRestaurant.Id,
            Email = foundRestaurant.Email,
            Name = foundRestaurant.Name,
            IsVerified = true,
            Role = foundRestaurant.Role,
            PhoneNumber = foundRestaurant.PhoneNumber,
            MenuItems = mapper.Map<MenuItemDTO[]>(foundRestaurant.MenuItems)
        });

    }

}