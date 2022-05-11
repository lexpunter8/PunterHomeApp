using PunterHomeDomain.Interfaces;
using PunterHomeDomain.ShoppingList;
using System;
using System.Collections.Generic;
using System.Text;

namespace PunterHomeDomain.Commands.RecipeStepCommand
{
    public class NameSpecification : ISpecification<ShoppingListAggregate>
    {
        private readonly string nameToSatisfy;

        public NameSpecification(string nameToSatisfy)
        {
            this.nameToSatisfy = nameToSatisfy;
        }
        public bool IsSatisfiedBy(ShoppingListAggregate entity)
        {
            return entity.Name == nameToSatisfy;
        }
    }
    public class EnsureValidEnvironmentCommand
    {
        private readonly IShoppingListRepository shoppingListRepository;

        public EnsureValidEnvironmentCommand(IShoppingListRepository shoppingListRepository)
        {
            this.shoppingListRepository = shoppingListRepository;
        }
        public void Ensure()
        {
            //shoppingListRepository.GetAllAsync(new NameSpecification("DEFAULT"));
            //shoppingListRepository.SaveAsync(ShoppingListAggregate.CreateNew("DEFAULT"));
        }
    }
}
