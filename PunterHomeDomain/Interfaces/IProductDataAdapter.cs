using PunterHomeDomain.Models;
using PunterHomeDomain.ShoppingList;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PunterHomeDomain.Interfaces
{
    public interface ISpecification<T>
    {
        bool IsSatisfiedBy(T entity);
    }
    public interface IQueryRepository
    {

    }

    public interface IRecipeStepRepository 
    {
        Task DeleteAsync(Guid id);
        Task<IEnumerable<RecipeStepAggregate>> GetAllAsync();

        Task<RecipeStepAggregate> GetAsync(Guid id);

        Task SaveAsync(RecipeStepAggregate entity);

        Task<IEnumerable<RecipeStepAggregate>> GetAllAsync(ISpecification<RecipeStepAggregate> specification);
    }

    public interface IShoppingListRepository
    {
        Task DeleteAsync(Guid id);
        Task<IEnumerable<ShoppingListAggregate>> GetAllAsync();
        Task<IEnumerable<ShoppingListAggregate>> GetAllAsync(ISpecification<ShoppingListAggregate> specification);
        Task<ShoppingListAggregate> GetAsync(Guid id);
        Task SaveAsync(ShoppingListAggregate entity);
    }

    public interface IRecipeRepository 
    {

    }
    public interface IProductDataAdapter
    {
        Task<IEnumerable<LightProduct>> GetProducts();
        Task<IEnumerable<ProductDetails>> GetAllProductDetails();
        Task DeleteProduct(Guid id);
        Task AddQuantityToProduct(ProductQuantity value, Guid id);
        Task<bool> Update(Guid id, string newName);
        Task UpdateProductQuantity(int id, ProductQuantity productQuantity);
        Task DeleteProductQuantityById(int id);
        void AddBarcodeToQuantity(int id, string barcode);
        Task IncreaseProductQuantity(int id, int value);
        Task DereaseProductQuantity(int id, int value);
        ProductDetails GetProductById(Guid productId);
        Guid GetProductIdForBarcode(string barcode);
        void AddProduct(LightProduct product, out Guid newID);
    }
}
