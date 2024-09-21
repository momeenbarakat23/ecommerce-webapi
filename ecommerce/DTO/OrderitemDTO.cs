using ecommerce.Models;
using System.Text.Json.Serialization;

namespace ecommerce.DTO
{
    public class OrderitemDTO
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public int? Quantity { get; set; }
        public string? UserName { get; set; }
        public decimal? TotleAmount { get; set; }
        public int? ItemId { get; set; }
        public string? ItemName { get; set; }
        public string? ItemDescription { get; set; }
        public int? priceItem { get; set; }
        public string? message { get; set; }
    }
}
