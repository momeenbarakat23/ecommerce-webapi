using ecommerce.DTO;
using ecommerce.Models;
using ecommerce.services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IitemsCRUD itemCRUD;
        private readonly Appdbcontext appdbcontext;

        public ItemsController(IitemsCRUD itemCRUD , Appdbcontext appdbcontext)
        {
            this.itemCRUD = itemCRUD;
            this.appdbcontext = appdbcontext;
        }

        [HttpGet("getdata")]
        public async Task<IActionResult> Get()
        {
            if (ModelState.IsValid)
            {
                var result = await itemCRUD.GetALLData();
                return Ok(result);
            }
            return BadRequest(ModelState);

        }
        [HttpGet("getDataById/{id}")]
        public async Task<IActionResult> GetDataById(int id)
        {
            if (ModelState.IsValid)
            {
                var result = await itemCRUD.GetDataById(id);
                return Ok(result);
            }
            return BadRequest(ModelState);
        }
        [HttpPut("EditData")]
        public async Task<IActionResult> UpdateData(ItemsDTO items)
        {
            if (ModelState.IsValid)
            {
                var result = await itemCRUD.UpdateData(items);
                return Ok(result);
            }
            return BadRequest(ModelState);
        }
        [HttpPost("AddItems")]
        public async Task<IActionResult> AddData(ItemsDTO items)
        {
            if (ModelState.IsValid) 
            {
                var check = await appdbcontext.Categories.FindAsync(items.CategoryId);
                if (check == null)
                {
                    return BadRequest("CategoryID is Wrong");
                }
                var result = await itemCRUD.AddData(items);
                return Ok(result);
            }
            return BadRequest(ModelState);

        }
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (ModelState.IsValid) 
            {
                var check = await appdbcontext.Item.FindAsync(id);
                if (check == null)
                {
                    return BadRequest("ID is Not Found");
                }
            var result = await itemCRUD.Delete(id);
            return Ok(result);
            }
            return NotFound();
        }
    }
}
