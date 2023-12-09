using System.ComponentModel.DataAnnotations;

namespace serveu.Dtos;

public class CreateTableDto
{
    [Required]
    [MaxLength(32)]
    [MinLength(2)]
    public string Name { get; set; }
}

public class UpdateTableDto : CreateTableDto
{

    [Required]
    public int Id
    {
        get; set;
    }
}