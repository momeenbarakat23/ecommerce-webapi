using ecommerce.services;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ecommerce.Models
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }

        public int? Quantity { get; set; }

        public int? Price { get; set; }
        public decimal? TotalAmountperItem { get; set; }

        [ForeignKey("Order")]
        public int? OrderId { get; set; }
        [ForeignKey("Items")]
        public int? ItemsId { get; set; }
        [ForeignKey("users")]
        public string? UserId { get; set; }

        public virtual Order? Order { get; set; }

        public virtual Items? Items { get; set; }
        public virtual Appuser? users { get; set; }

    }
}
