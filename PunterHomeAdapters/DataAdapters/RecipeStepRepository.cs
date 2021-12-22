using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PunterHomeAdapters.Models;
using PunterHomeDomain.Interfaces;
using PunterHomeDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PunterHomeAdapters.DataAdapters
{
    public class EfRecipeStepRepository : IRecipeStepRepository
    {

        private DbContextOptions<HomeAppDbContext> myDbOptions;
        private readonly IMapper mapper;

        public EfRecipeStepRepository(DbContextOptions<HomeAppDbContext> options, IMapper mapper)
        {
            myDbOptions = options;
            this.mapper = mapper;
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<RecipeStepAggregate>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<RecipeStepAggregate> GetAsync(Guid id)
        {
            using var context = new HomeAppDbContext(myDbOptions);
            var result = await context.RecipeSteps.Include(i => i.Ingredients).FirstOrDefaultAsync(s => s.Id == id);
            return mapper.Map<RecipeStepAggregate>(result);
        }

        public async Task SaveAsync(RecipeStepAggregate entity)
        {
            var efObj = mapper.Map<DbRecipeStep>(entity);

            using var context = new HomeAppDbContext(myDbOptions);

            var foundEntity = context.RecipeSteps.Include(i => i.Ingredients).FirstOrDefault(s => s.Id == entity.Id);

            if (foundEntity != null)
            {
                context.RecipeSteps.Remove(foundEntity);
                foundEntity = efObj;
                context.Add(foundEntity);
            }
            else
            {
                context.RecipeSteps.Add(efObj);
            }

            await context.SaveChangesAsync();
        }
    }
}
