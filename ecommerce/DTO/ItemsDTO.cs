namespace ecommerce.DTO
{
    public class ItemsDTO
    {
        public int Id { get; set; }

        public string? Name { get; set; }
        public string? Message {  get; set; } 

        public int? price { get; set; } 

        public string? Description { get; set; }
        public string? CategoryName { get; set; }
        public int? CategoryId { get; set; }

    }
}
