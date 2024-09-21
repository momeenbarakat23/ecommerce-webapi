using ecommerce.DTO;
using ecommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ecommerce.services
{
    public class CategCRUD : ICategCRUD
    {
        private readonly Appdbcontext appdbcontext;

        public CategCRUD(Appdbcontext appdbcontext)
        {
            this.appdbcontext = appdbcontext;
        }
        public async Task<string> AddData(CategoryDTO category)
        {
            var data = await appdbcontext.Categories.Where(x => x.CategoryName == category.Name).ToListAsync();
            if (category.Id == 0  && data.Count ==0)
            {
                var categoryADD = new Category();
                categoryADD.CategoryName = category.Name;
                appdbcontext.Categories.Add(categoryADD);
                await appdbcontext.SaveChangesAsync();
                return "Created";
            }
            return "error In Create";
        }

        public async Task<bool> Delete(string Name)
        {

             var data = await appdbcontext.Categories.SingleOrDefaultAsync(x => x.CategoryName == Name);
            if (data != null)
            {
                appdbcontext.Categories.Remove(data);
                await appdbcontext.SaveChangesAsync();
                return true;
            }
           return false;

            
        }

        public async Task<List<CategoryDTO>> GetALLData()
        {
           var list = new List<CategoryDTO>();
            
            var data = await appdbcontext.Categories.ToListAsync();
            if (data != null)
            {
                foreach (var category in data)
                {
                    var categoryDTO = await GetDataById(category.CategoryId);
                    list.Add(categoryDTO);
                }
            }
            return list;
        }

        public async Task<CategoryDTO> GetDataById(int Id)
        {
            var result = new CategoryDTO();
            var data = await appdbcontext.Categories.Include(x=>x.Items).SingleOrDefaultAsync(x=>x.CategoryId==Id);
            result.Id = data.CategoryId;
            result.Name = data.CategoryName;
            var listItem = data.Items.Where(x => x.CategoryId == Id).Select(x => x.Name).ToList();
            if (listItem is not null)
            {
                result.Items.AddRange(listItem);
            }
            return result;
            

        }

        public async Task<string> UpdateData(CategoryDTO category)
        {
            var categorydata = await appdbcontext.Categories.SingleOrDefaultAsync(x=>x.CategoryId==category.Id);
            if (categorydata is not null)
            {
                categorydata.CategoryName = category.Name;
                if (await appdbcontext.Categories.FirstOrDefaultAsync(x=>x.CategoryName==category.Name) is null)
                {
                    appdbcontext.Categories.Update(categorydata);
                    await appdbcontext.SaveChangesAsync();
                    return "success Update";
                }
                return "This Product Exists";
            }
            return "Faild";
            
            
        }
    }
}
