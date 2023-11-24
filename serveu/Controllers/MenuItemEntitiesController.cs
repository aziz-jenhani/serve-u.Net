using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using serveu.Context;
using serveu.Dtos;
using serveu.Interceptor;
using serveu.Models;

namespace serveu.Controllers
{
    [Route("api/web/menu-items/")]
    [ApiController]
    [ServiceFilter(typeof(ApiResponseFormatFilter))]
    public class MenuItemEntitiesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public MenuItemEntitiesController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }

        // GET: api/MenuItemEntities
        [HttpGet]
        public async Task<ActionResult<PaginatedMenuItemResponseDTO>> GetMenuItemsPaged(
     [FromQuery] int page = 1,
     [FromQuery] int limit = 10)
        {
            if (_context.MenuItems == null)
            {
                return NotFound();
            }

            // Effectuez la pagination en utilisant Skip et Take
            var query = _context.MenuItems
                .Include(mi => mi.Category)
                .Include(mi => mi.Image)
                .Skip((page - 1) * limit)
                .Take(limit);

            var menuItems = await query.ToListAsync();

            // Récupérez le nombre total d'éléments pour la pagination
            var totalItems = await _context.MenuItems.CountAsync();

            // Créez la liste de DTO à partir des entités
            var menuItemDTOs = menuItems.Select(mi => _mapper.Map<MenuItemDTO>(mi)).ToList();

            // Créez le DTO de réponse de pagination
            var paginatedResponse = new PaginatedMenuItemResponseDTO
            {
                Items = menuItemDTOs,
                Meta = new PaginationMetaDTO
                {
                    TotalItems = totalItems,
                    ItemCount = menuItemDTOs.Count,
                    ItemsPerPage = limit,
                    TotalPages = (int)Math.Ceiling((double)totalItems / limit),
                    CurrentPage = page
                }
            };

            return Ok(paginatedResponse);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<DetailedMenuItemResponseDTO>> GetMenuItemEntities(int id)
        {
            var menuItemEntities = await _context.MenuItems
                .Include(mi => mi.Image)
                .Include(mi => mi.Category)
                .FirstOrDefaultAsync(mi => mi.MenuItemId == id);

            if (menuItemEntities == null)
            {
                return NotFound();
            }

            var detailedMenuItemDTO = new DetailedMenuItemResponseDTO
            {
                CreatedAt = menuItemEntities.CreatedAt,
                UpdatedAt = menuItemEntities.UpdatedAt,
                Id = menuItemEntities.MenuItemId,
                Name = menuItemEntities.Name,
                Price = menuItemEntities.Price,
                Image = _mapper.Map<ImageDTO>(menuItemEntities.Image),
                Category = _mapper.Map<MenuCategoryDTO>(menuItemEntities.Category)
            };

            return Ok( detailedMenuItemDTO);
        }

        // PUT: api/MenuItemEntities/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // PUT: api/MenuItemEntities/{id}
        [HttpPut]
        public async Task<IActionResult> PutMenuItemEntities( UpdateMenuItemDTO updateMenuItemDTO)
        {
            if (updateMenuItemDTO == null || string.IsNullOrEmpty(updateMenuItemDTO.Id))
            {
                return BadRequest();
            }
            if (!int.TryParse(updateMenuItemDTO.Id, out int menuId))
            {
                return BadRequest("Invalid ID format");
            }

            var existingMenuItem = await _context.MenuItems.FindAsync(menuId);

            if (existingMenuItem == null)
            {
                return NotFound();
            }

            // Mettez à jour les propriétés de l'entité existante
            existingMenuItem.Name = updateMenuItemDTO.Name;
            existingMenuItem.Price = updateMenuItemDTO.Price;
            // ... mettez à jour d'autres propriétés

            existingMenuItem.Update(); // Assurez-vous de mettre à jour UpdatedAt

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MenuItemEntitiesExists(menuId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Retournez la réponse avec les données mises à jour
            var responseDTO = new MenuItemDTO
            {
                Id = existingMenuItem.MenuItemId,
                Name = existingMenuItem.Name,
                Price = existingMenuItem.Price,
                CreatedAt = existingMenuItem.CreatedAt,
                UpdatedAt = existingMenuItem.UpdatedAt,
                // ... autres propriétés
            };

            return Ok(responseDTO);
        }


        // POST: api/MenuItemEntities
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> PostMenuItemEntities(CreateMenuItemDTO createMenuItemDTO)
        {
            if (_context.MenuItems == null)
            {
                return Problem("Entity set 'AppDbContext.MenuItems' is null.");
            }

            // Créez une nouvelle entité MenuItemEntities à partir du DTO
            var menuItemEntities = new MenuItemEntities
            {
                Name = createMenuItemDTO.Name,
                Price = createMenuItemDTO.Price,
                image_id = int.Parse(createMenuItemDTO.ImageId),
                category_id = int.Parse(createMenuItemDTO.CategoryId),
               
                // Assurez-vous d'ajuster ces propriétés en fonction de votre modèle réel
            };

            _context.MenuItems.Add(menuItemEntities);
            await _context.SaveChangesAsync();

            var response = new
            {
                
                    restaurant = new { id = menuItemEntities.restaurant_id }, // Assurez-vous d'ajuster en fonction de votre modèle
                    category = new { id = menuItemEntities.category_id }, // Assurez-vous d'ajuster en fonction de votre modèle
                    image = new { id = menuItemEntities.image_id }, // Assurez-vous d'ajuster en fonction de votre modèle
                    menuItemEntities.Name,
                    menuItemEntities.Price,
                    menuItemEntities.CreatedAt,
                    menuItemEntities.UpdatedAt,
                    menuItemEntities.MenuItemId
                
            };

            return Ok(response);
        }
        // DELETE: api/MenuItemEntities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMenuItemEntities(int id)
        {
            if (_context.MenuItems == null)
            {
                return NotFound();
            }
            var menuItemEntities = await _context.MenuItems
                           .Include(mi => mi.Image)
                           .Include(mi => mi.Category)
                           .FirstOrDefaultAsync(mi => mi.MenuItemId == id);
            if (menuItemEntities == null)
            {
                return NotFound();
            }
            var detailedMenuItemDTO = new DetailedMenuItemResponseDTO
            {
                CreatedAt = menuItemEntities.CreatedAt,
                UpdatedAt = menuItemEntities.UpdatedAt,
                Id = menuItemEntities.MenuItemId,
                Name = menuItemEntities.Name,
                Price = menuItemEntities.Price,
                Image = _mapper.Map<ImageDTO>(menuItemEntities.Image),
                Category = _mapper.Map<MenuCategoryDTO>(menuItemEntities.Category)
            };

            _context.MenuItems.Remove(menuItemEntities);
            await _context.SaveChangesAsync();

            return Ok(detailedMenuItemDTO);
        }

        private bool MenuItemEntitiesExists(int id)
        {
            return (_context.MenuItems?.Any(e => e.MenuItemId == id)).GetValueOrDefault();
        }
    }
}
