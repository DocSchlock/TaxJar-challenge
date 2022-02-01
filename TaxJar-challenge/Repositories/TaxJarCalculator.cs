using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
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
        /// Gets the individual tax rates per each tax type
        /// </summary>
        /// <param name="order"></param>
        /// <param name="taxRate"></param>
        /// <returns></returns>
        public Dictionary<string, float> GetTotalTaxRate(ISalesOrder order, ITaxRate taxRate)
        {
            var taxList = new Dictionary<string, float>();

            taxList.Add("CountryRate", taxRate.CountryRate * order.OrderValue);
            taxList.Add("StateRate", taxRate.StateRate * order.OrderValue);
            taxList.Add("CombinedRate", taxRate.CombinedRate * order.OrderValue); // this is the main rate to use, but it's important we return all the rates
            taxList.Add("CombinedDistrictRate", taxRate.CombinedDistrictRate * order.OrderValue);
            taxList.Add("CityRate", taxRate.CityRate * order.OrderValue);

            return taxList;
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
    }
}