using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using serveu.Context;
using serveu.Interceptor;
using serveu.Models;

namespace serveu.Controllers
{
    [Route("api/web/files")]
    [ApiController]
    [ServiceFilter(typeof(ApiResponseFormatFilter))]
    public class FileEntitiesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        public FileEntitiesController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }


        // POST: api/FileEntities
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("upload/image")]
        public async Task<ActionResult<object>> UploadImage(IFormFile file)
        {
            if (file == null || file.Length <= 0)
            {
                return BadRequest("Le fichier n'est pas fourni.");
            }

            try
            {
                // Créez un nom de fichier unique pour éviter les conflits
                var fileName = $"{Guid.NewGuid()}_{file.FileName}";

                // Obtenez le chemin physique du dossier wwwroot
                var wwwrootPath = _environment.WebRootPath;

                // Construisez le chemin complet pour le dossier wwwroot/uploads/images
                var uploadsFolder = Path.Combine(wwwrootPath, "uploads", "images");

                // Assurez-vous que le dossier existe, sinon, créez-le
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Le chemin physique complet du fichier
                var filePath = Path.Combine(uploadsFolder, fileName);

                // Enregistrez le fichier sur le disque
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Créez une entité FileEntities pour enregistrer les détails du fichier
                var fileEntity = new FileEntities
                {
                    Path = filePath.Replace(wwwrootPath, string.Empty).Replace("\\", "/"), // Stockez le chemin relatif
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                    // Ajoutez d'autres propriétés selon vos besoins
                };

                // Ajoutez l'entité à la base de données
                _context.FileEntities.Add(fileEntity);
                await _context.SaveChangesAsync();

                // Formatez la réponse
                var response = new
                {
                    path = $"http://localhost:5139/{fileEntity.Path}",
                    fileEntity.CreatedAt,
                    fileEntity.UpdatedAt,
                    fileEntity.Id
                };

                // Retournez une réponse 201 (Created)
                return CreatedAtAction(nameof(UploadImage), response);
            }
            catch (Exception ex)
            {
                // Gérez les erreurs ici
                return StatusCode(500, $"Une erreur s'est produite lors de l'upload du fichier : {ex.Message}");
            }
        }




        private bool FileEntitiesExists(int id)
        {
            return (_context.FileEntities?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
