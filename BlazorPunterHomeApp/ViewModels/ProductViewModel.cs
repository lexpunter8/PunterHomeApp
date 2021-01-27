using BlazorPunterHomeApp.Data;
using DataModels.Measurements;
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
    //public class ProductViewModel : BaseViewModel
    //{
    //    private ProductModel myOriginalProduct;
    //    public ProductViewModel(ProductDetailsViewModel product)
    //    {
    //        SetProduct(product);
    //        CurrentSelectedProduct.PropertyChanged += CurrentSelectedProduct_PropertyChanged;
    //    }

    //    private void CurrentSelectedProduct_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    //    {
            
    //    }

    //    public ProductDetailsViewModel CurrentSelectedProduct { get; private set; }

    //    public void SetProduct(ProductDetailsViewModel product)
    //    {
    //        myOriginalProduct = new ProductModel
    //        {
    //            Id = product.ProductModel.Id,
    //            Name = product.ProductModel.Name.Clone() as string 
    //        };

    //        var l = new List<BaseMeasurement>();
    //        foreach (var q in product.ProductModel.ProductQuantities)
    //        {
    //            l.Add(new BaseMeasurement(q.MeasurementType) { ProductQuantityId = q.ProductQuantityId, UnitQuantityTypeVolume = q.UnitQuantityTypeVolume });
    //        }
    //        myOriginalProduct.ProductQuantities = l;
    //        CurrentSelectedProduct = product;
    //    }

    //    public bool IsDirty => CalculateIsDirty();

    //    private bool CalculateIsDirty()
    //    {
    //        var e = myOriginalProduct.Name != CurrentSelectedProduct.ProductModel.Name;

    //        var quantitiesChanged = false;

    //        foreach(var quantity in CurrentSelectedProduct.ProductModel.ProductQuantities)
    //        {
    //            var q = myOriginalProduct.ProductQuantities.FirstOrDefault(q => q.ProductQuantityId == quantity.ProductQuantityId);

    //            if (q == null)
    //            {
    //                quantitiesChanged = true;
    //                break;
    //            }

    //            //if (q.Quantity != quantity.Quantity)
    //            //{
    //            //    quantitiesChanged = true;
    //            //    break;
    //            //}
    //        }
    //        return quantitiesChanged || e;
    //    }

    //    public void ChangeQuantity(int id, int n)
    //    {
    //        var quan = CurrentSelectedProduct.ProductModel.ProductQuantities.FirstOrDefault(q => q.Id == id);
    //        quan.Quantity += n;
    //    }

    //    [JsonIgnore]
    //    public static List<EUnitMeasurementType> SelectableUnitQuantityTypes => Enum.GetValues(typeof(EUnitMeasurementType)).Cast<EUnitMeasurementType>().ToList();

    //    [JsonIgnore]
    //    public ProductQuantity NewProductQuantity { get; set; } = new ProductQuantity();
    //}
}
