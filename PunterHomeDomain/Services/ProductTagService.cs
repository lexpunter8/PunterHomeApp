using DataModels;
using PunterHomeDomain.Interfaces;
using PunterHomeDomain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PunterHomeDomain.Services
{
    public interface IProductTagService
    {
        Task<List<ProductTagModel>> GetTagsForProductAsync(Guid productId);
        Task<List<TagModel>> GetAllTagsAsync();
        void AddTagToProduct(Guid productId, Guid tagId);
    }

    public class ProductTagService : IProductTagService
    {
        private readonly ITagDataAdapter tagDataAdapter;

        public ProductTagService(ITagDataAdapter tagDataAdapter)
        {
            this.tagDataAdapter = tagDataAdapter;
        }

        public void AddTagToProduct(Guid productId, Guid tagId)
        {
            tagDataAdapter.AddTagToProduct(productId, tagId);
        }

        public async Task<List<TagModel>> GetAllTagsAsync()
        {
            return await tagDataAdapter.GetAllTags();
        }

        public async Task<List<ProductTagModel>> GetTagsForProductAsync(Guid productId)
        {
            return await tagDataAdapter.GetTagsForProduct(productId);
        }
    }
}
