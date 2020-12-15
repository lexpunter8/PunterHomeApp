using BlazorPunterHomeApp.Data;
using PunterHomeDomain;
using PunterHomeDomain.Interfaces;
using PunterHomeDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static Enums;

namespace BlazorPunterHomeApp.ViewModels
{
    public class ProductViewModel : BaseViewModel
    {
        private ProductModel myOriginalProduct;
        public ProductViewModel(ProductDetailsModel product)
        {
            SetProduct(product);
            CurrentSelectedProduct.PropertyChanged += CurrentSelectedProduct_PropertyChanged;
        }

        private void CurrentSelectedProduct_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            
        }

        public ProductDetailsModel CurrentSelectedProduct { get; private set; }

        public void SetProduct(ProductDetailsModel product)
        {
            myOriginalProduct = new ProductModel
            {
                Id = product.Id,
                Name = product.Name.Clone() as string 
            };

            var l = new List<ProductQuantity>();
            foreach (var q in product.ProductQuantities)
            {
                l.Add(new ProductQuantity { ProductQuantityId = q.Id, Quantity = q.Quantity, MeasurementType = q.MeasurementType, UnitQuantityTypeVolume = q.UnitQuantityTypeVolume });
            }
            myOriginalProduct.ProductQuantities = l;
            CurrentSelectedProduct = product;
        }

        public bool IsDirty => CalculateIsDirty();

        private bool CalculateIsDirty()
        {
            var e = myOriginalProduct.Name != CurrentSelectedProduct.Name;

            var quantitiesChanged = false;

            foreach(var quantity in CurrentSelectedProduct.ProductQuantities)
            {
                var q = myOriginalProduct.ProductQuantities.FirstOrDefault(q => q.Id == quantity.Id);

                if (q == null)
                {
                    quantitiesChanged = true;
                    break;
                }

                if (q.Quantity != quantity.Quantity)
                {
                    quantitiesChanged = true;
                    break;
                }
            }
            return quantitiesChanged || e;
        }

        public void ChangeQuantity(int id, int n)
        {
            var quan = CurrentSelectedProduct.ProductQuantities.FirstOrDefault(q => q.Id == id);
            quan.Quantity += n;
        }

        [JsonIgnore]
        public static List<EUnitMeasurementType> SelectableUnitQuantityTypes => Enum.GetValues(typeof(EUnitMeasurementType)).Cast<EUnitMeasurementType>().ToList();

        [JsonIgnore]
        public ProductQuantity NewProductQuantity { get; set; } = new ProductQuantity();
    }
}
