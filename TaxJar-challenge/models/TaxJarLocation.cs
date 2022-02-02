using Newtonsoft.Json;

namespace TaxJar_challenge.models
{
    public class TaxJarLocation : ILocation
    {
        [JsonProperty("id")]
        public string? Id { get; set; } // this is omitted from the query string

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("zip_code")]
        public string ZipCode { get; set; }

        [JsonProperty("state")]
        public string? State { get; set; }

        [JsonProperty("city")]
        public string? City { get; set; }

        [JsonProperty("street")]
        public string? Street { get; set; }
    }
}