using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TaxJar_challenge.models;

namespace TaxJar_challenge.Repositories
{
    public class TaxJarCalculator : ITaxCalculator
    {
        private readonly HttpClient _httpClient;

        private const string API_URL = "https://api.taxjar.com/v2/";

        public TaxJarCalculator(string token)
        {
            _httpClient = new HttpClient(); // create the singleton client - reuse is best practice
            _httpClient.BaseAddress = new Uri(API_URL); // set the base path
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token); // set the always-on auth header
        }

        /// <summary>
        /// Gets the rate payload from the TaxJar API for a given location
        /// </summary>
        /// <param name="loc"></param>
        /// <returns></returns>
        /// <exception cref="HttpRequestException">throws if the API returns a unsuccessful status code</exception>
        /// <exception cref="JsonReaderException">throws if data payload cannot be deserialized</exception>
        /// <exception cref="NullReferenceException">throws if the payload is somehow null</exception>
        public async Task<ITaxRate> GetTaxRateByLocation(ILocation loc)
        {
            var urlFields = await BuildRateQueryString(loc); // create the parameter string from the location

            var response = await _httpClient.GetAsync($"rates/{loc.ZipCode}" + (string.IsNullOrWhiteSpace(urlFields) ? "" : $"?{urlFields}")); // send the request

            if (response != null && response.IsSuccessStatusCode) // if successful, move on
            {
                //read the content
                var content = await response.Content.ReadAsStringAsync();

                // deserialize the content into our rate class and return
                var deserializedContent = DeserializeIgnoringRoot<ITaxRate>(content, "rate");
                return deserializedContent;
            }
            else
            {
                // if the response is null or failed to succeed, throw an exception
                throw new HttpRequestException($"Failure in GetTaxRateByLocation Code:{response?.StatusCode} Response: {response?.ReasonPhrase}");
            }
        }

        /// <summary>
        /// Allows use to remove a root wrapper in a JSON payload to deserialize directly to the class
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="content"></param>
        /// <param name="root"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        private static T DeserializeIgnoringRoot<T>(string content, string root) where T : class
        {
            var jo = JObject.Parse(content);
            return jo.SelectToken(root, false)?.ToObject<T>() ?? throw new NullReferenceException($"content parameter could not be deserialized because the Parse was null on root: {root} content : {content}");
        }

        /// <summary>
        /// builds the correct URL to push query parameters
        /// </summary>
        /// <param name="loc"></param>
        /// <returns></returns>
        private static async Task<string> BuildRateQueryString(ILocation loc)
        {
            var fields = new Dictionary<string, string>();

            if (!string.IsNullOrWhiteSpace(loc.Country))
                fields.Add("country", loc.Country);
            if (!string.IsNullOrWhiteSpace(loc.State))
                fields.Add("state", loc.State);
            if (!string.IsNullOrWhiteSpace(loc.City))
                fields.Add("city", loc.City);
            if (!string.IsNullOrWhiteSpace(loc.Street))
                fields.Add("street", loc.Street);

            return await new FormUrlEncodedContent(fields).ReadAsStringAsync();
        }

        public async Task<ITotalTaxes> GetTotalTaxesOnOrder(ISalesOrder order)
        {
            // check the order object for null and malformations
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            // serialize it to the post object
            var json = JsonConvert.SerializeObject(order);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // post to the end point
            var response = await _httpClient.PostAsync("taxes", content);

            // if good response, deserialize the return
            if (response != null && response.IsSuccessStatusCode)
            {
                //read the content
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var deserializedContent = DeserializeIgnoringRoot<ITotalTaxes>(jsonResponse, "tax");
                return deserializedContent;
            }
            // else throw exception
            else
                throw new HttpRequestException($"Failure in GetTotalTaxesOnOrder Code:{response?.StatusCode} Response: {response?.ReasonPhrase}");
        }
    }
}