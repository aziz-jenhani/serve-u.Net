using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using serve_u.Helper;
using serveu.Context;
using serveu.Dtos;
using serveu.Interceptor;
using serveu.Models;


namespace serveu.Controllers
{
    [Route("api/web/menu-categories/")]
    [ApiController]
    [ServiceFilter(typeof(ApiResponseFormatFilter))]
    public class MenuCategoryEntitiesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public MenuCategoryEntitiesController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/MenuCategoryEntities
        [HttpGet]
        public async Task<ActionResult<PaginatedMenuCategoryResponseDTO>> GetMenuCategories(
     [FromQuery] int page = 1,
     [FromQuery] int limit = 10)
        {
            if (_context.MenuCategories == null)
            {
                return NotFound();
            }

            // Effectuez la pagination en utilisant Skip et Take
            var query = _context.MenuCategories
                .Skip((page - 1) * limit)
                .Take(limit);

            var menuCategories = await query.ToListAsync();

            // Récupérez le nombre total d'éléments pour la pagination
            var totalItems = await _context.MenuCategories.CountAsync();

            // Créez la liste de DTO à partir des entités
            var menuCategoryDTOs = menuCategories.Select(mc => _mapper.Map<MenuCategoryDTO>(mc)).ToList();

            // Créez le DTO de réponse de pagination
            var paginatedResponse = new PaginatedMenuCategoryResponseDTO
            {
                Items = menuCategoryDTOs,
                Meta = new PaginationMetaDTO
                {
                    TotalItems = totalItems,
                    ItemCount = menuCategoryDTOs.Count,
                    ItemsPerPage = limit,
                    TotalPages = (int)Math.Ceiling((double)totalItems / limit),
                    CurrentPage = page
                }
            };

            return Ok(paginatedResponse);
        }
        // GET: api/MenuCategoryEntities/5
        [HttpGet("{id}")]

        public async Task<ActionResult<MenuCategoryDTO>> GetMenuCategoryEntities(int id)
        {
            if (_context.MenuCategories == null)
            {
                return NotFound();
            }

            var menuCategoryEntities = await _context.MenuCategories.FindAsync(id);

            if (menuCategoryEntities == null)
            {
                return NotFound();
            }


            if (menuCategoryEntities == null)
            {
                return NotFound();
            }

            var menuCategoryDTO = _mapper.Map<MenuCategoryDTO>(menuCategoryEntities);

            return menuCategoryDTO;
        }


        [HttpPut("")]
        [Consumes("application/json")]
        public async Task<ActionResult<MenuCategoryDTO>> PutMenuCategoryEntities(updateMenuCategory updateRequest)
        {
            if (string.IsNullOrEmpty(updateRequest.Id))
            {
                return BadRequest();
            }

            // Convertir la chaîne ID en int pour la recherche dans la base de données
            if (!int.TryParse(updateRequest.Id, out int categoryId))
            {
                return BadRequest("Invalid ID format");
            }

            var menuCategoryEntities = await _context.MenuCategories.FindAsync(categoryId);

            if (menuCategoryEntities == null)
            {
                return NotFound();
            }
            menuCategoryEntities.Update();
            menuCategoryEntities.Name = updateRequest.Name;
            menuCategoryEntities.MenuCategoryId = int.Parse(updateRequest.Id);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MenuCategoryEntitiesExists(categoryId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            var updatedMenuCategoryDTO = _mapper.Map<MenuCategoryDTO>(menuCategoryEntities);
            updatedMenuCategoryDTO.Id = categoryId;
            // Retournez directement le DTO, ApiResponseFormatFilter va l'encapsuler dans la structure attendue
            return Ok(updatedMenuCategoryDTO);
        }
        public class updateMenuCategory
        {
            public string Name { get; set; }
            public string Id { get; set; }
        }

        // POST: api/MenuCategoryEntities
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(MenuCategoryDTO), StatusCodes.Status201Created)]
        public async Task<ActionResult<MenuCategoryDTO>> PostMenuCategoryEntities([FromBody] MenuCategoryRequest model)
        {
            if (_context.MenuCategories == null)
            {
                return Problem("Entity set 'AppDbContext.MenuCategories' is null.");
            }

            MenuCategoryEntities menuCategoryEntities = new MenuCategoryEntities
            {
                Name = model.Name
            };

            _context.MenuCategories.Add(menuCategoryEntities);
            await _context.SaveChangesAsync();

            var responseDTO = new MenuCategoryDTO
            {
                Name = menuCategoryEntities.Name,
                CreatedAt = menuCategoryEntities.CreatedAt,
                UpdatedAt = menuCategoryEntities.UpdatedAt,
                Id = menuCategoryEntities.MenuCategoryId
            };

            return CreatedAtAction("GetMenuCategoryEntities", new { id = menuCategoryEntities.MenuCategoryId }, responseDTO);
        }

        // Créez une classe pour représenter le modèle de la requête
        public class MenuCategoryRequest
        {
            public string Name { get; set; }
        }


        // DELETE: api/MenuCategoryEntities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMenuCategoryEntities(int id)
        {
            if (_context.MenuCategories == null)
            {
                return NotFound();
            }
            var menuCategoryEntities = await _context.MenuCategories.FindAsync(id);
            if (menuCategoryEntities == null)
            {
                return NotFound();
            }
            var deleteResponseDTO = new MenuCategoryDTO
            {
                CreatedAt = menuCategoryEntities.CreatedAt,
                UpdatedAt = menuCategoryEntities.UpdatedAt,
                Name = menuCategoryEntities.Name
            };
            _context.MenuCategories.Remove(menuCategoryEntities);
            await _context.SaveChangesAsync();

            return Ok(deleteResponseDTO);
        }

        private bool MenuCategoryEntitiesExists(int id)
        {
            return (_context.MenuCategories?.Any(e => e.MenuCategoryId == id)).GetValueOrDefault();
        }
    }
}
