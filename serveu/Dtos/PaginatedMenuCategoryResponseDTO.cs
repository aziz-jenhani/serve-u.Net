namespace serveu.Dtos
{
    public class PaginatedMenuCategoryResponseDTO
    {
        public List<MenuCategoryDTO> Items { get; set; }
        public PaginationMetaDTO Meta { get; set; }
    }
    public class PaginationMetaDTO
    {
        public int TotalItems { get; set; }
        public int ItemCount { get; set; }
        public int ItemsPerPage { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }
}
