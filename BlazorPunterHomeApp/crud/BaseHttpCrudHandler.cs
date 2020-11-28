using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BlazorPunterHomeApp.crud
{
    public class BaseHttpCrudHandler<T> : ICrudHandler<T>
    {
        private readonly string myBaseApiUrl;
        public BaseHttpCrudHandler(string baseUrl)
        {
            myBaseApiUrl = baseUrl;
        }

        public async Task<bool> Create(string value)
        {
            try
            {
                var httpClient = new HttpClient();

                Uri uri = new Uri($"{myBaseApiUrl}/{value}");
                var response = await httpClient.PostAsync(uri, null);
                string responseString = await response.Content.ReadAsStringAsync();
                HandleResponseMessage(response);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> Create(T value)
        {

            var json = JsonConvert.SerializeObject(value);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            using var httpClient = new HttpClient();

            Uri uri = new Uri($"{myBaseApiUrl}/{value}");
            var response = await httpClient.PostAsync(uri, data);
            string responseString = await response.Content.ReadAsStringAsync();
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeletetById(Guid id)
        {
            using var httpClient = new HttpClient();
            Uri uri = new Uri(myBaseApiUrl);
            var response = await httpClient.DeleteAsync(uri);
            string responseString = await response.Content.ReadAsStringAsync();

            return HandleResponseMessage(response);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            using var httpClient = new HttpClient();
            Uri uri = new Uri(myBaseApiUrl);
            var response = await httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
            string responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<T[]>(responseString);

            return result;
        }

        public async Task<A> GetById<A>(Guid id) where A : class
        {
            using var httpClient = new HttpClient();
            Uri uri = new Uri($"{myBaseApiUrl}/{id}");
            var response = await httpClient.GetAsync(uri);
            string responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<A>(responseString);

            if (HandleResponseMessage(response))
            {
                return result;
            }
            return null;
        }

        public Task<bool> Update(T value)
        {
            throw new NotImplementedException();
        }

        private bool HandleResponseMessage(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                // Handle generic
                return true;
            }
            return false;
        }
    }
}
