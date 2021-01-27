using Newtonsoft.Json;
using PunterHomeDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RazorShared
{
    public interface IBlazorTagService
    {
        Task<List<TagModel>> GetAllTags();
        void AddTagToProduct(Guid productId, Guid tagId);
    }
    public class BlazorTagService : IBlazorTagService
    {
        public async void AddTagToProduct(Guid productId, Guid tagId)
        {
            var httpClient = new HttpClient();
            Uri uri = new Uri($"http://localhost:5005/api/ProductTag/{productId}/{tagId}");
            var response = await httpClient.PostAsync(uri, null);
            string responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<TagModel[]>(responseString);
        }

        public async Task<List<TagModel>> GetAllTags()
        {
            var httpClient = new HttpClient();
            Uri uri = new Uri("http://localhost:5005/api/ProductTag");
            var response = await httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
            string responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<TagModel[]>(responseString);
            var t = result.OrderBy(q => q.Name).ToList();

            return t;
        }
    }
}
