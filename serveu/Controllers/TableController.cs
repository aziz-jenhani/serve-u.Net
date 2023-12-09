using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using serveu.Context;
using serveu.Dtos;
using serveu.Interceptor;
using serveu.Models;
using ServeU.Utils;


namespace serveu.Controllers;

[Route("/api/web/tables")]
[ApiController()]
[ServiceFilter(typeof(ApiResponseFormatFilter))]
[Authorize(Roles = UserRole.RESTAURANT)]
public class TableController : ControllerBase
{

    private readonly AppDbContext appDbContext;
    private readonly IMapper mapper;
    private readonly UserManager<ApplicationUser> userManager;

    private readonly AuthUtils authUtils;

    public TableController(AppDbContext _appDbContext, IMapper _mapper, AuthUtils _authUtils)
    {
        appDbContext = _appDbContext;
        mapper = _mapper;
        authUtils = _authUtils;
    }


    [HttpGet]
    public async Task<ActionResult<PaginatedItemsDto<Table>>> GetMany([FromQuery] int page = 1, [FromQuery] int limit = 10)
    {

        var currentUser = await authUtils.getAuthenticatedUserAsync(HttpContext.User);


        var tables = await appDbContext.Tables
                        .Where(u => u.restaurant_id == currentUser.Id)
                        .Skip((page - 1) * limit)
                        .Take(limit)
                        .ToListAsync();


        var totalItems = await appDbContext.Tables.Where(u => u.restaurant_id == currentUser.Id).CountAsync();

        return Ok(new PaginatedItemsDto<Table>()
        {
            Items = tables,
            Meta = new PaginatedItemsMetaDto()
            {
                TotalItems = totalItems,
                ItemCount = tables.Count,
                ItemsPerPage = limit,
                TotalPages = (int)Math.Ceiling((double)totalItems / limit),
                CurrentPage = page
            }
        });

    }

    [HttpGet("{Id}")]
    public async Task<ActionResult<Table>> GetOne(int Id)
    {
        var currentUser = await authUtils.getAuthenticatedUserAsync(HttpContext.User);

        var foundTable = await appDbContext.Tables
        .Where(item => item.Id == Id && item.restaurant_id == currentUser.Id)
        .FirstAsync();


        if (foundTable == null)
        {
            return NotFound("Table not found!");
        }

        return Ok(foundTable);

    }

    [HttpDelete("{Id}")]
    public async Task<ActionResult> DeleteOne(int Id)
    {

        var currentUser = await authUtils.getAuthenticatedUserAsync(HttpContext.User);

        var foundTable = await appDbContext.Tables
        .Where(item => item.Id == Id && item.restaurant_id == currentUser.Id)
        .FirstAsync();

        if (foundTable == null)
        {
            return NotFound("Table not found!");
        }

        appDbContext.Tables.Remove(foundTable);
        await appDbContext.SaveChangesAsync();

        return NoContent();

    }

    [HttpPost()]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApplicationUserDto>> CreateOne(CreateTableDto tableDto)
    {

        var currentUser = await authUtils.getAuthenticatedUserAsync(HttpContext.User);
        var foundTableByName = await appDbContext.Tables
            .Where((tb) => tb.Name == tableDto.Name && tb.restaurant_id == currentUser.Id)
            .FirstOrDefaultAsync();

        if (foundTableByName != null)
        {
            return BadRequest("Table already exists!");
        }

        var newTable = new Table()
        {
            Name = tableDto.Name,
            Status = "F",
            restaurant_id = currentUser.Id
        };
        await appDbContext.Tables.AddAsync(newTable);
        await appDbContext.SaveChangesAsync();

        return Ok(newTable);
    }

    [HttpPut()]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApplicationUserDto>> UpdateOne(UpdateTableDto tableDto)
    {

        var currentUser = await authUtils.getAuthenticatedUserAsync(HttpContext.User);

        var foundTable = await appDbContext.Tables
        .Where(item => item.Id == tableDto.Id && item.restaurant_id == currentUser.Id)
        .FirstAsync();


        if (foundTable == null)
        {
            return NotFound("Table not found!");
        }

        foundTable.Name = tableDto.Name;

        appDbContext.Update(foundTable);
        await appDbContext.SaveChangesAsync();

        return Ok(foundTable);

    }
}