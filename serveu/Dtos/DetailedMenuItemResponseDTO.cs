namespace serveu.Dtos
{
    public class DetailedMenuItemResponseDTO
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public ImageDTO Image { get; set; }
        public MenuCategoryDTO Category { get; set; }
    }
}
