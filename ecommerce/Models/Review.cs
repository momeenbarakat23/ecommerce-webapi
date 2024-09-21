using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ecommerce.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }

        public int? Rating { get; set; }

        public string? Comment { get; set; }

        public DateTime? ReviewDate { get; set; }
        [ForeignKey("Items")]
        public int? ItemId { get; set; }

        public virtual Items? Items { get; set; }
    }
}
