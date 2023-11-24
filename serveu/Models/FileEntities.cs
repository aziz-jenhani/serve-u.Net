using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace serveu.Models
{
    public class FileEntities
    {
        [Key]
        [Column("id")]

        public int Id { get; set; }
        [Column("path")]

        public string Path { get; set; }
        [Column("created_at")]

        public DateTime CreatedAt { get; set; }
        [Column("updated_at")]

        public DateTime UpdatedAt { get; set; }
        public FileEntities()
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
