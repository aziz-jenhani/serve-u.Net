namespace serveu.Dtos
{
    public class CreateMenuItemDTO
    {
        public string Name { get; set; }
        public float Price { get; set; }
        public int ImageId { get; set; }
        public int CategoryId { get; set; }
    }
}
