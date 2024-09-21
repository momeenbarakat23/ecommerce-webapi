using ecommerce.DTO;
using ecommerce.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class orderController : ControllerBase
    {
        private readonly Iorder order;

        public orderController(Iorder order)
        {
            this.order = order;
        }
        [HttpPost("add-To-Cart")]
        [Authorize]
       
        public async Task<IActionResult> AddtoCart(OrderitemDTO orderitemDTO)
        {
            if (ModelState.IsValid)
            {
                var result = await order.AddtoCart(orderitemDTO);
                return Ok(result);
            }
            return BadRequest();

        }
        [HttpGet("ConfirmOrder")]
        [Authorize]
        public async Task<IActionResult> ConfirmOrder()
        {
            if (ModelState.IsValid)
            {
                var result = await order.ConfirmOrder();
                return Ok(result);
            }
            return BadRequest();

        }
    }
}
