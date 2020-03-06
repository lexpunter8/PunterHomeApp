using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PunterHomeAdapters;
using PunterHomeAdapters.Models;
using PunterHomeApp.Interfaces;

namespace PunterHomeApp.DataAdapters
{
    public class ProductDataAdapter : IProductDataAdapter
    {
        private DbContextOptions<HomeAppDbContext> myDbOptions;

        public ProductDataAdapter(DbContextOptions<HomeAppDbContext> options)
        {
            myDbOptions = options;
        }

        public void AddProduct(DbProduct product)
        {
            using var context = new HomeAppDbContext(myDbOptions);
            context.Products.Add(product);
            context.SaveChanges();
        }

        public async Task<IEnumerable<IProduct>> GetProducts()
        {
            using var context = new HomeAppDbContext(myDbOptions);
            DbSet<DbProduct> products = context.Products;
            return await products.ToListAsync();
        }
    }
}
