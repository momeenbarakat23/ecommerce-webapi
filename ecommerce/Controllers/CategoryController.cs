using ecommerce.DTO;
using ecommerce.services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategCRUD categCRUD;

        public CategoryController(ICategCRUD categCRUD)
        {
            this.categCRUD = categCRUD;
        }
        [HttpGet("DisplayAllData")]
        public async Task<IActionResult> DisplayAllData()
        {
            if (ModelState.IsValid)
            {
                var result = await categCRUD.GetALLData();
                return Ok(result);
            }
            return BadRequest(ModelState);
        }

        [HttpGet("DisplayDataById/{id}")]
        public async Task<IActionResult> DisplayDataById(int id)
        {
            if (ModelState.IsValid)
            {
                var result = await categCRUD.GetDataById(id);
                return Ok(result);
            }
            return BadRequest(ModelState);
        }

        [HttpPost("AddData")]
        public async Task<IActionResult> AddData([FromForm] CategoryDTO category)
        {
            if (ModelState.IsValid)
            {
                var result = await categCRUD.AddData(category);
                return Ok(result);
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("deleteData/{Name}")]
        public async Task<IActionResult> deleteData(string Name)
        {
            if (ModelState.IsValid)
            {
                var result = await categCRUD.Delete(Name);
                return Ok(result);
            }
            return BadRequest(ModelState);
        }

        [HttpPut("Update/{id:int}")]
        public async Task<IActionResult> UpdateData(CategoryDTO category,int id)
        {
            if (ModelState.IsValid)
            {
                category.Id=id;
                var result = await categCRUD.UpdateData(category);
                return Ok(result);
            }
            return BadRequest(ModelState);
        }


    }
}
