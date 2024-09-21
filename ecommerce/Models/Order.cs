using System.ComponentModel.DataAnnotations;

namespace ecommerce.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        public DateTime? OrderDate { get; set; }

        public string? OrderStatus { get; set; }

        public decimal? TotalAmount { get; set; }

        public string? UserName { get; set; }

        public virtual ICollection<Items> Items { get; set; } = new List<Items>();
    }
}
