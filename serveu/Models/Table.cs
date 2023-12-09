using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace serveu.Models;

public class Table
{

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Name { get; set; }

    public string restaurant_id { get; set; }

    [DefaultValue("F")]
    public string Status { get; set; }

    //   FREE = 'F',
    //   PENDING = 'P',
    //   ORDERED = 'O',
    //   CONFIRMED = 'C',
    //   SERVED = 'S',

    [ForeignKey("restaurant_id")]
    public ApplicationUser Restaurant { get; set; }
}