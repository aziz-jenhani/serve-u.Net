using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace serveu.Models
{
    public class restaurant
    {



        [Key]
        public int RestaurantId { get; set; } // Ajoutez une clé primaire
        [Column("PhoneNumber")]
        public string PhoneNumber { get; set; }

        public List<MenuItemEntities> MenuItems { get; set; }
    }
}
