using ecommerce.DTO;

namespace ecommerce.services
{
    public interface Iorder
    {
        public Task<OrderitemDTO> AddtoCart(OrderitemDTO orderitemDTO);
        public Task<OrderDTO> ConfirmOrder();
    }
}
