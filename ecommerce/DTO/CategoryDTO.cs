using ecommerce.Models;

namespace ecommerce.DTO
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string>? Items { get; set; } = new List<string>();
    }
}
