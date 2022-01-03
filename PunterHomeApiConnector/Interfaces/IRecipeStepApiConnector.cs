using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PunterHomeApiConnector.Interfaces
{
    public interface IRecipeStepApiConnector
    {
        Task RemoveIngredient(Guid recipeStepId, Guid ingredientId);
    }
}
