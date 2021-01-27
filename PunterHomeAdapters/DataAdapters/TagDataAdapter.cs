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
    public class TagDataAdapter : ITagDataAdapter
    {

        private DbContextOptions<HomeAppDbContext> myDbOptions;

        public TagDataAdapter(DbContextOptions<HomeAppDbContext> options)
        {
            myDbOptions = options;
        }

        public void AddTagToProduct(Guid productId, Guid tagId)
        {
            using var context = new HomeAppDbContext(myDbOptions);

            var newTag = new DbProductTags
            {
                ProductId = productId,
                TagId = tagId
            };

            context.ProductTags.Add(newTag);

            context.SaveChanges();
        }

        public async Task<List<TagModel>> GetAllTags()
        {
            using var context = new HomeAppDbContext(myDbOptions);

            return await context.ProductTag.Select(p => new TagModel { Id = p.Id, Name = p.Name }).ToListAsync();

        }

        public async Task<List<ProductTagModel>> GetTagsForProduct(Guid productID)
        {
            using var context = new HomeAppDbContext(myDbOptions);

            return await context.ProductTags.Where(p => p.ProductId == productID).Include(p => p.Tag).Select(p => new ProductTagModel{ Id = p.Tag.Id, Name = p.Tag.Name, ProductId = p.ProductId }).ToListAsync();
        }
    }
}
