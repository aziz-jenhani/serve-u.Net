using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace serveu.Models
{
    public class MenuCategoryEntities
    {
        [Key] // Ajoutez cette annotation pour spécifier la clé primaire
        [Column("id")]
        public int MenuCategoryId { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [InverseProperty("Category")]
        public List<MenuItemEntities> MenuItems { get; set; }

        public MenuCategoryEntities()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        // Méthode appelée lors de la mise à jour de l'entité
        public void Update()
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }

}
