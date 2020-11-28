using PunterHomeApp.Services;

namespace PunterHomeDomain.Interfaces
{
    public interface IIngredientService
    {
        void InsertIngredient(IIngredient newIngredient);
        void DeleteIngredient(IIngredient newIngredient);
    }
}
