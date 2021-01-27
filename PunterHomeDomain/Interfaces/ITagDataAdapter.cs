using PunterHomeDomain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PunterHomeDomain.Interfaces
{
    public interface ITagDataAdapter
    {
        Task<List<TagModel>> GetAllTags();
        Task<List<ProductTagModel>> GetTagsForProduct(Guid productID);
        void AddTagToProduct(Guid productId, Guid tagId);
    }
}
