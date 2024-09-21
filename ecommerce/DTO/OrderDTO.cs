namespace ecommerce.DTO
{
    public class OrderDTO
    {
        public int? OrderId { get; set; }

        public DateTime? OrderDate { get; set; }

        public string? OrderStatus { get; set; }

        public decimal? TotalAmount { get; set; }

        public string? UserName { get; set; }
    }
}
