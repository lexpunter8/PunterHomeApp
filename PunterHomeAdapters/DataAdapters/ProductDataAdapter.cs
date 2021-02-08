using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataModels.Measurements;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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

        public void AddProduct(LightProduct product, out Guid newID)
        {
            if (string.IsNullOrEmpty(product.Name))
            {
                newID = Guid.Empty;
                return;
            }
            DbProduct newProduct = new DbProduct
            {
                Id = Guid.NewGuid(),
                Name = product.Name
            };

            //newProduct.ProductQuantities = ConvertProductQuantities(product.ProductQuantities, newProduct);

            newID = newProduct.Id;
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
                UnitQuantityType = value.MeasurementType,
                Barcode = value.Barcode
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
            var quantity = context.ProductQuantities.Include(x => x.ProductId).FirstOrDefault(p => p.Id == id);

            if (quantity.ProductId.MeasurementValues == null)
            {
                var values = new MeasurementClassObject
                {
                    Values = new List<MeasurementAmount>
                    {
                        new MeasurementAmount
                        {
                            Amount = quantity.QuantityTypeVolume,
                            Type = quantity.UnitQuantityType
                        }
                    }
                };
                quantity.ProductId.MeasurementValues = JsonConvert.SerializeObject(values);
                await context.SaveChangesAsync();
                return;
            }

            var measurementClass = JsonConvert.DeserializeObject<MeasurementClassObject>(quantity.ProductId.MeasurementValues);
            measurementClass.Add(new MeasurementAmount
            {
                Amount = quantity.QuantityTypeVolume * -1,
                Type = quantity.UnitQuantityType
            });
            //if (measurementClass.Values.Any(v => v.Type == quantity.UnitQuantityType))
            //{
            //    measurementClass.Values.First(v => v.Type == quantity.UnitQuantityType).Amount += quantity.QuantityTypeVolume;
            //    quantity.ProductId.MeasurementValues = JsonConvert.SerializeObject(measurementClass);
            //    await context.SaveChangesAsync();
            //    return;
            //}
            //measurementClass.Values.Add(new MeasurementAmount
            //{
            //    Amount = quantity.QuantityTypeVolume,
            //    Type = quantity.UnitQuantityType
            //});
            quantity.ProductId.MeasurementValues = JsonConvert.SerializeObject(measurementClass);
            await context.SaveChangesAsync();
            return;
        }

        public ProductDetails GetProductById(Guid productId)
        {
            using var context = new HomeAppDbContext(myDbOptions);
            DbProduct p = context.Products.Include(p => p.ProductQuantities).ThenInclude(pq => pq.ProductId).FirstOrDefault(p => p.Id == productId);

            if (p == null)
            {
                return null;
            }

            return new ProductDetails
            {
                Id = p.Id,
                MeasurementAmounts = JsonConvert.DeserializeObject<MeasurementClassObject>(p.MeasurementValues ?? string.Empty),
                Name = p.Name,
                ProductQuantities = ConvertProductQuantities(p.ProductQuantities)
            };
        }

        public async Task<IEnumerable<LightProduct>> GetProducts()
        {
            using var context = new HomeAppDbContext(myDbOptions);
            List<DbProduct> products = await context.Products.Include(p => p.ProductQuantities).ThenInclude(pq => pq.ProductId).ToListAsync();

            var retval = new List<LightProduct>();
            products.ForEach(p => 
            {
                retval.Add(new LightProduct
                {
                    Id = p.Id,
                    Name = p.Name,
                    Tags = context.ProductTags.Include(t => t.Tag).Where(t => t.ProductId == p.Id).Select(t => new ProductTagModel { Id = t.TagId, Name = t.Tag.Name}).ToList()
                });
            });

            return retval;
        }

        public async Task<IEnumerable<ProductDetails>> GetAllProductDetails()
        {
            using var context = new HomeAppDbContext(myDbOptions);
            List<DbProduct> products = await context.Products.Include(p => p.ProductQuantities).ThenInclude(pq => pq.ProductId).ToListAsync();

            var retval = new List<ProductDetails>();
            products.ForEach(p =>
            {
                retval.Add(new ProductDetails
                {
                    Id = p.Id,
                    Name = p.Name,
                    MeasurementAmounts = JsonConvert.DeserializeObject<MeasurementClassObject>(p.MeasurementValues ?? string.Empty),
                    ProductQuantities = ConvertProductQuantities(p.ProductQuantities)
                });
            });

            return retval;
        }

        public async Task IncreaseProductQuantity(int id, int value)
        {
            using var context = new HomeAppDbContext(myDbOptions);
            var quantity = context.ProductQuantities.Include(x => x.ProductId).FirstOrDefault(p => p.Id == id);

            if (quantity.ProductId.MeasurementValues == null)
            {
                var values = new MeasurementClassObject
                {
                    Values = new List<MeasurementAmount>
                    {
                        new MeasurementAmount
                        {
                            Amount = quantity.QuantityTypeVolume * value,
                            Type = quantity.UnitQuantityType
                        }
                    }
                };
                quantity.ProductId.MeasurementValues = JsonConvert.SerializeObject(values);
                await context.SaveChangesAsync();
                return;
            }

            var measurementClass = JsonConvert.DeserializeObject<MeasurementClassObject>(quantity.ProductId.MeasurementValues);
            measurementClass.Add(new MeasurementAmount
            {
                Amount = quantity.QuantityTypeVolume * value,
                Type = quantity.UnitQuantityType
            });
            //if (measurementClass.Values.Any(v => v.Type == quantity.UnitQuantityType))
            //{
            //    measurementClass.Values.First(v => v.Type == quantity.UnitQuantityType).Amount += quantity.QuantityTypeVolume;
            //    quantity.ProductId.MeasurementValues = JsonConvert.SerializeObject(measurementClass);
            //    await context.SaveChangesAsync();
            //    return;
            //}
            //measurementClass.Values.Add(new MeasurementAmount
            //{
            //    Amount = quantity.QuantityTypeVolume,
            //    Type = quantity.UnitQuantityType
            //});
            quantity.ProductId.MeasurementValues = JsonConvert.SerializeObject(measurementClass);
            await context.SaveChangesAsync();
            return;
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

        //private List<DbProductQuantity> ConvertProductQuantities(IEnumerable<BaseMeasurement> p, DbProduct product)
        //{
        //    var retval = new List<DbProductQuantity>();
        //    foreach (BaseMeasurement q in p)
        //    {
        //        retval.Add(new DbProductQuantity
        //        {
        //            ProductId = product,
        //            QuantityTypeVolume = q.UnitQuantityTypeVolume,
        //            UnitQuantity = q.Quantity,
        //            UnitQuantityType = q.MeasurementType
        //        });
        //    }
        //    return retval;
        //}

        private List<BaseMeasurement> ConvertProductQuantities(IEnumerable<DbProductQuantity> p)
        {
            var retval = new List<BaseMeasurement>();
            foreach (DbProductQuantity q in p)
            {
                var measurement = BaseMeasurement.GetMeasurement(q.UnitQuantityType);
                measurement.UnitQuantityTypeVolume = q.QuantityTypeVolume;
                measurement.ProductQuantityId = q.Id;
                measurement.Barcode = q.Barcode;
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

        public async void AddBarcodeToQuantity(int id, string barcode)
        {
            using var context = new HomeAppDbContext(myDbOptions);

            var prod = context.ProductQuantities.FirstOrDefault(p => p.Id == id);

            if (prod == null)
            {
                // prod id does not exists
                return;
            }
            prod.Barcode = barcode;

            await context.SaveChangesAsync();
        }

        public Guid GetProductIdForBarcode(string barcode)
        {
            using var context = new HomeAppDbContext(myDbOptions);

            var prod = context.ProductQuantities.Include(pq => pq.ProductId).FirstOrDefault(p => p.Barcode == barcode);

            if (prod == null)
            {
                // prod id does not exists
                return Guid.Empty;
            }
            return prod.ProductId.Id;
        }
    }
}
