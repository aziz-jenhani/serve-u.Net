using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace serveu.Models
{
    public class MenuItemEntities
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]

        public int MenuItemId { get; set; } // Ajoutez une clé primaire
        [Column("name")]
        public string Name { get; set; }

        [Column("price", TypeName = "double precision")]
        public float Price { get; set; }
        public int image_id { get; set; } // Clé étrangère pour Category

        [ForeignKey("image_id")]
        public FileEntities Image { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }
        public int category_id { get; set; } // Clé étrangère pour Category
        [ForeignKey("category_id")]
        public MenuCategoryEntities Category { get; set; }

        public string restaurant_id { get; set; } // Clé étrangère pour Restaurant
        [ForeignKey("restaurant_id")]
        public ApplicationUser Restaurant { get; set; }
        public MenuItemEntities()
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
