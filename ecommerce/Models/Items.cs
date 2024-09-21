using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ecommerce.Models
{
    public class Items
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public int? price { get; set; } 

        public string Description { get; set; } = null!;
        [ForeignKey("Category")]
        public int? CategoryId { get; set; }
        [JsonIgnore]
        public virtual Category? Category { get; set; }
        [JsonIgnore]
        public virtual ICollection<Order> Order { get; set; } = new List<Order>();
        [JsonIgnore]
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
