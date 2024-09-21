using ecommerce.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ecommerce.services
{
    public class Appdbcontext : IdentityDbContext<Appuser>
    {
        public Appdbcontext(DbContextOptions<Appdbcontext> options):base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Items>().HasMany(x => x.Order).WithMany(x => x.Items)
               .UsingEntity<OrderItem>();
        }

        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<OrderItem> OrderItems { get; set; }

        public virtual DbSet<Order> Order { get; set; }

        public virtual DbSet<Items> Item { get; set; }

        public virtual DbSet<Review> Reviews { get; set; }

    }
}
