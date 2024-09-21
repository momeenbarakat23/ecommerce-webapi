using ecommerce.DTO;
using ecommerce.Models;

namespace ecommerce.services
{
    public interface IitemsCRUD
    {
        public Task<List<ItemsDTO>> GetALLData();
        public Task<ItemsDTO> GetDataById(int Id);
        public Task<bool> Delete(int id);
        public Task<ItemsDTO> UpdateData(ItemsDTO items);
        public Task<ItemsDTO> AddData(ItemsDTO items);
        
    }
}
