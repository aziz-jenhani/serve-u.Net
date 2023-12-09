namespace serveu.Dtos
{
    public class PaginatedItemsDto<T>
    {
        public List<T> Items { get; set; }
        public PaginatedItemsMetaDto Meta { get; set; }
    }
    public class PaginatedItemsMetaDto
    {
        public int TotalItems { get; set; }
        public int ItemCount { get; set; }
        public int ItemsPerPage { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }
}
