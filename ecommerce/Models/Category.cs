using System.ComponentModel.DataAnnotations;

namespace ecommerce.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = null!;

        public virtual ICollection<Items> Items { get; set; } = new List<Items>();
    }
}
