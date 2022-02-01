using Newtonsoft.Json;

namespace TaxJar_challenge.models
{
    public class TaxJarTaxRate : ITaxRate
    {
        public string Country { get; set; }

        [JsonProperty("country_rate")]
        public float CountryRate { get; set; }

        public string County { get; set; }

        [JsonProperty("county_rate")]
        public float CountyRate { get; set; }

        public string? State { get; set; }

        [JsonProperty("state_rate")]
        public float StateRate { get; set; }

        public string? City { get; set; }

        [JsonProperty("city_rate")]
        public float CityRate { get; set; }

        [JsonProperty("zip")]
        public string ZipCode { get; set; }

        public string? Street { get; set; }

        [JsonProperty("combined_district_rate")]
        public float CombinedDistrictRate { get; set; }

        [JsonProperty("combined_rate")]
        public float CombinedRate { get; set; }

        [JsonProperty("freight_taxable")]
        public bool FreightTaxable { get; set; }
    }
}