using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PunterHomeAdapters;
using PunterHomeAdapters.Models;
using PunterHomeApp.Services;
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

        public void AddProduct(Product product)
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
                UnitQuantityType = value.MeasurementType
            });

            await context.SaveChangesAsync();
        }

        public async Task DeleteProduct(Guid id)
        {
            using var context = new HomeAppDbContext(myDbOptions);
            var toDelete = context.Products.Where(p => p.Id == id).Include(pq => pq.ProductQuantities);
            context.Products.RemoveRange(toDelete);
            await context.SaveChangesAsync();
        }

        public async Task DeleteProductQuantityById(int id)
        {
            using var context = new HomeAppDbContext(myDbOptions);

            var toDelete = context.ProductQuantities.FirstOrDefault(p => p.Id == id);
            context.ProductQuantities.Remove(toDelete);
            await context.SaveChangesAsync();
        }

        public async Task DereaseProductQuantity(int id, int value)
        {
            using var context = new HomeAppDbContext(myDbOptions);
            var quantity = context.ProductQuantities.FirstOrDefault(p => p.Id == id);

            if (quantity == null)
            {
                return;
            }

            quantity.UnitQuantity -= value;
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            using var context = new HomeAppDbContext(myDbOptions);
            List<DbProduct> products = await context.Products.Include(p => p.ProductQuantities).ThenInclude(pq => pq.ProductId).ToListAsync();

            var retval = new List<Product>();
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

        public async Task IncreaseProductQuantity(int id, int value)
        {
            using var context = new HomeAppDbContext(myDbOptions);
            var quantity = context.ProductQuantities.FirstOrDefault(p => p.Id == id);

            if (quantity == null)
            {
                return;
            }

            quantity.UnitQuantity += value;
            await context.SaveChangesAsync();
        }

        public async Task<bool> Update(Guid id, string newName)
        {
            using var context = new HomeAppDbContext(myDbOptions);

            var product = context.Products.FirstOrDefault(p => p.Id.Equals(id));

            if (product == null)
            {
                throw new KeyNotFoundException($"Could not find product with Id: {id}");
            }

            product.Name = newName;
            await context.SaveChangesAsync();
            return true;
        }

        public async Task UpdateProductQuantity(int id, ProductQuantity productQuantity)
        {
            using var context = new HomeAppDbContext(myDbOptions);

            var productToChange = context.ProductQuantities.FirstOrDefault(q => q.Id == id);

            productToChange.UnitQuantity = productQuantity.Quantity;

            await context.SaveChangesAsync();
        }

        // Conversions

        private List<DbProductQuantity> ConvertProductQuantities(IEnumerable<BaseMeasurement> p, DbProduct product)
        {
            var retval = new List<DbProductQuantity>();
            foreach (BaseMeasurement q in p)
            {
                retval.Add(new DbProductQuantity
                {
                    ProductId = product,
                    QuantityTypeVolume = q.UnitQuantityTypeVolume,
                    UnitQuantity = q.Quantity,
                    UnitQuantityType = q.MeasurementType
                });
            }
            return retval;
        }

        private List<BaseMeasurement> ConvertProductQuantities(IEnumerable<DbProductQuantity> p)
        {
            var retval = new List<BaseMeasurement>();
            foreach (DbProductQuantity q in p)
            {
                var measurement = BaseMeasurement.GetMeasurement(q.UnitQuantityType);
                measurement.Quantity = q.UnitQuantity;
                measurement.UnitQuantityTypeVolume = q.QuantityTypeVolume;
                measurement.ProductQuantityId = q.Id;
                retval.Add(measurement
                //    new ProductQuantity
                //{
                //    Id = q.Id,
                //    UnitQuantityTypeVolume = q.QuantityTypeVolume,
                //    Quantity = q.UnitQuantity,
                //    UnitQuantityType = q.UnitQuantityType,
                    
                //}
                    );
            }
            return retval;
        }
    }
}
