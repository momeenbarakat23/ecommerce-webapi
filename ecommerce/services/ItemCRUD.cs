using ecommerce.DTO;
using ecommerce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ecommerce.services
{
    public class ItemCRUD : IitemsCRUD
    {
        private readonly UserManager<Appuser> userManager;
        private readonly Appdbcontext appdbcontext;

        public ItemCRUD(UserManager<Appuser> userManager ,Appdbcontext appdbcontext)
        {
            this.userManager = userManager;
            this.appdbcontext = appdbcontext;
        }

        public async Task<ItemsDTO> AddData(ItemsDTO items)
        {
             var item = new Items();
            item.price = items.price;
            item.Name = items.Name;
            item.CategoryId = items.CategoryId;
            item.Description = items.Description;
            await appdbcontext.Item.AddAsync(item);
            await appdbcontext.SaveChangesAsync();
            return items;
        }

        public async Task<bool> Delete(int id)
        {
            var data = await appdbcontext.Item.SingleOrDefaultAsync(x => x.Id == id);
            if (data != null)
            {
                appdbcontext.Remove(data);
                await appdbcontext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<ItemsDTO>> GetALLData()
        {
            
            var list = new List<ItemsDTO>();
            var items = await appdbcontext.Item.ToListAsync();
            if (items is not null)
            {
                foreach (var item in items)
                {
                    var data = new ItemsDTO();
                    data.Id = item.Id;
                    data.Name = item.Name;
                    data.price = item.price;
                    data.Description = item.Description;
                    data.CategoryId = item.CategoryId;
                    data.CategoryName = appdbcontext.Categories.Where(x=>x.CategoryId==data.CategoryId).Select(x=>x.CategoryName).Single();
                    list.Add(data);
                   
                }
                return list;


            }
            return list;
        }

        public async Task<ItemsDTO> GetDataById(int Id)
        {
            var data = new ItemsDTO();
            var item = await appdbcontext.Item.SingleOrDefaultAsync(x => x.Id == Id);
            if (item is not null)
            {
                data.Id = item.Id;
                data.Name = item.Name;
                data.price = item.price;
                data.Description = item.Description;
                data.CategoryId = item.CategoryId;
                data.CategoryName = appdbcontext.Categories.Where(x => x.CategoryId == data.CategoryId).Select(x => x.CategoryName).Single();
            }
            return data;
        }

        public async Task<ItemsDTO> UpdateData(ItemsDTO items)
        {
            var item = await appdbcontext.Item.SingleOrDefaultAsync(x=>x.Id==items.Id);
            if (item is not null)
            {
                item.Id = items.Id;
                if (items.Name is not null)
                    item.Name = items.Name;
                if (items.price is not null)
                    item.price = items.price;
                if (items.Description is not null)
                    item.Description = items.Description;
                if (items.CategoryId is not null)
                    item.CategoryId = items.CategoryId;
                appdbcontext.Item.Update(item);
                await appdbcontext.SaveChangesAsync();
            }

            return items;
        }
    }
}
