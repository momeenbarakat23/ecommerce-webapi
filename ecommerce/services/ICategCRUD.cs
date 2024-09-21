using ecommerce.DTO;

namespace ecommerce.services
{
    public interface ICategCRUD
    {
        public Task<List<CategoryDTO>> GetALLData();
        public Task<CategoryDTO> GetDataById(int Id);
        public Task<bool> Delete(string Name);
        public Task<string> UpdateData(CategoryDTO category);
        public Task<string> AddData(CategoryDTO category);
    }
}
