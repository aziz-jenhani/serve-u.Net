namespace serveu.Dtos
{
    public class PaginatedMenuItemResponseDTO
    {
        public List<MenuItemDTO> Items { get; set; }
        public PaginationMetaDTO Meta { get; set; }
    }

    public class MenuItemDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public MenuCategoryDTO Category { get; set; }
        public ImageDTO Image { get; set; }
    }

   

    public class ImageDTO
    {
        public int Id { get; set; }
        public string Path { get; set; }
    }
}
