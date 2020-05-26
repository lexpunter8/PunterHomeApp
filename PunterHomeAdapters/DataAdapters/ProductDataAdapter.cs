using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PunterHomeAdapters;
using PunterHomeAdapters.Models;
using PunterHomeDomain.Interfaces;
using PunterHomeDomain.Models;

namespace PunterHomeApp.DataAdapters
{
    public class ProductDataAdapter : IProductDataAdapter
    {
        private DbContextOptions<HomeAppDbContext> myDbOptions;

        public ProductDataAdapter(DbContextOptions<HomeAppDbContext> options)
        {
            myDbOptions = options;
        }

        public void AddProduct(IProduct product)
        {
            DbProduct newProduct = new DbProduct
            {
                Id = product.Id,
                Name = product.Name
            };

            newProduct.ProductQuantities = ConvertProductQuantities(product.ProductQuantities, newProduct);

            
            using var context = new HomeAppDbContext(myDbOptions);

            context.Products.Add(newProduct);
            context.SaveChanges();
        }

        public async Task AddQuantityToProduct(ProductQuantity value, Guid id)
        {
            using var context = new HomeAppDbContext(myDbOptions);

            var prod = context.Products.FirstOrDefault(p => p.Id == id);

            if (prod == null)
            {
                // prod id does not exists
                return;
            }

            context.ProductQuantities.Add(new DbProductQuantity
            {
                ProductId = prod,
                QuantityTypeVolume = value.UnitQuantityTypeVolume,
                UnitQuantity = value.Quantity,
                UnitQuantityType = value.UnitQuantityType
            });

            await context.SaveChangesAsync();
        }

        public async Task DeleteProduct(Guid id)
        {
            using var context = new HomeAppDbContext(myDbOptions);
            var toDelete = context.Products.Where(p => p.Id == id).Include(pq => pq.ProductQuantities);

            context.Remove(toDelete);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<IProduct>> GetProducts()
        {
            using var context = new HomeAppDbContext(myDbOptions);
            List<DbProduct> products = await context.Products.Include(p => p.ProductQuantities).ThenInclude(pq => pq.ProductId).ToListAsync();

            var retval = new List<IProduct>();
            products.ForEach(p => 
            {
                retval.Add(new Product
                {
                    Id = p.Id,
                    Name = p.Name,
                    ProductQuantities = ConvertProductQuantities(p.ProductQuantities)
                });
            });

            return retval;
        }

        // Conversions

        private List<DbProductQuantity> ConvertProductQuantities(IEnumerable<IProductQuantity> p, DbProduct product)
        {
            var retval = new List<DbProductQuantity>();
            foreach (IProductQuantity q in p)
            {
                retval.Add(new DbProductQuantity
                {
                    Id = q.Id,
                    ProductId = product,
                    QuantityTypeVolume = q.UnitQuantityTypeVolume,
                    UnitQuantity = q.Quantity,
                    UnitQuantityType = q.UnitQuantityType
                });
            }
            return retval;
        }

        private List<IProductQuantity> ConvertProductQuantities(IEnumerable<DbProductQuantity> p)
        {
            var retval = new List<IProductQuantity>();
            foreach (DbProductQuantity q in p)
            {
                retval.Add(new ProductQuantity
                {
                    Id = q.Id,
                    UnitQuantityTypeVolume = q.QuantityTypeVolume,
                    Quantity = q.UnitQuantity,
                    UnitQuantityType = q.UnitQuantityType
                });
            }
            return retval;
        }
    }
}
