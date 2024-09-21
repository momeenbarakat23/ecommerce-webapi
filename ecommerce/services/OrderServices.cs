using ecommerce.DTO;
using ecommerce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ecommerce.services
{
    public class OrderServices : Iorder
    {
        private readonly Appdbcontext appdbcontext;
        private readonly UserManager<Appuser> userManager;
        private readonly IHttpContextAccessor httpContextAccessor;

        public int? OrderId { get; set; }

        public OrderServices(Appdbcontext appdbcontext , UserManager<Appuser> userManager , IHttpContextAccessor httpContextAccessor)
        {
            this.appdbcontext = appdbcontext;
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
        }
        public async Task<OrderitemDTO> AddtoCart(OrderitemDTO orderitemDTO)
        {
            var User = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name).Value;
            var userId = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var item = await appdbcontext.Item.SingleOrDefaultAsync(x=>x.Name==orderitemDTO.ItemName);
            if (item is null)
            {
                return new OrderitemDTO() { message = "not Found" };

            }
            //var order = await appdbcontext.Order.SingleOrDefaultAsync(x=>x.UserName==User);
            var Orderitem = await appdbcontext.OrderItems.FirstOrDefaultAsync(x=>x.UserId==userId);
            if (Orderitem== null) 
            {
                var neworder = new Order();
                neworder.UserName = User;
                neworder.OrderDate = DateTime.Now;
                neworder.OrderStatus = "wait Confirm Order";
                neworder.TotalAmount = 0;
               await appdbcontext.Order.AddAsync(neworder);
                await appdbcontext.SaveChangesAsync();
                var Neworderdata = await appdbcontext.OrderItems.SingleOrDefaultAsync(x => x.UserId == userId);
                OrderId= neworder.OrderId;
            }
            else
            {
                OrderId =Orderitem.OrderId;
            }
            
            if (User == null)
            { return new OrderitemDTO() { message = "Not Authorized" }; }
            var itemInCart = await appdbcontext.OrderItems.SingleOrDefaultAsync(x => x.ItemsId == item.Id && x.UserId==userId);
            if (itemInCart != null &&  Orderitem != null)
            { itemInCart.Quantity += 1;
                itemInCart.TotalAmountperItem = itemInCart.Quantity * itemInCart.Price;
                appdbcontext.OrderItems.Update(itemInCart);
                await appdbcontext.SaveChangesAsync();
                orderitemDTO.Quantity = itemInCart.Quantity;
                orderitemDTO.TotleAmount = itemInCart.TotalAmountperItem;

            }
            else
            {

                var newitem = new OrderItem();
                newitem.ItemsId = item.Id;
                newitem.OrderId = OrderId;
                newitem.UserId = userId;
                newitem.Price=item.price;
                newitem.Quantity = 1;
                newitem.TotalAmountperItem =newitem.Quantity * newitem.Price;
                appdbcontext.OrderItems.Add(newitem);
                await appdbcontext.SaveChangesAsync();
                orderitemDTO.Quantity = newitem.Quantity;
                orderitemDTO.TotleAmount = newitem.Price;
                


            }
            
            orderitemDTO.message = "successed AddToCart";
            orderitemDTO.ItemDescription = item.Description;
            orderitemDTO.priceItem = item.price;
            orderitemDTO.ItemName = item.Name;
            orderitemDTO.UserName = User;
            orderitemDTO.ItemId = item.Id;


            return orderitemDTO;
        }

        public async Task<OrderDTO> ConfirmOrder()
        {
            var user = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            var userID = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var cart = await appdbcontext.OrderItems.Where(x=>x.UserId==userID).Include(x=>x.Items).ToListAsync();
            var orderiD = await appdbcontext.OrderItems.Where(x=>x.UserId==userID).Select(x=>x.OrderId).FirstOrDefaultAsync();
           
            
            if (cart.Count==0||orderiD==null)
            {
                return new OrderDTO() { OrderStatus = "Empty Cart" };

            }
            var orderDTO = new OrderDTO();
            orderDTO.OrderStatus = "Pending";
            orderDTO.OrderDate = DateTime.Now;
            orderDTO.TotalAmount = 0;
            foreach (var item in cart)
            {
                orderDTO.TotalAmount = orderDTO.TotalAmount+ item.TotalAmountperItem;
            }
            
            orderDTO.UserName = user;
            orderDTO.OrderId = orderiD;

            var order = new Order();
            order.OrderId =(int) orderiD;
            order.OrderDate = orderDTO.OrderDate;
            order.OrderStatus = orderDTO.OrderStatus;
            order.UserName = user;
            order.TotalAmount = orderDTO.TotalAmount;
            appdbcontext.Order.Update(order);
            appdbcontext.OrderItems.RemoveRange(cart);
            await appdbcontext.SaveChangesAsync();


            return orderDTO;
        }
    }
}
