using Newtonsoft.Json;

namespace TaxJar_challenge.models
{
    public class TaxJarTotalTaxes : ITotalTaxes
    {
        [JsonProperty("order_total_amount")]
        public float OrderTotalAmount { get; set; }

        [JsonProperty("shipping")]
        public float Shipping { get; set; }

        [JsonProperty("taxable_amount")]
        public float TaxableAmount { get; set; }

        [JsonProperty("amount_to_collect")]
        public float AmountToCollect { get; set; }

        [JsonProperty("rate")]
        public float Rate { get; set; }

        [JsonProperty("has_nexus")]
        public bool HasNexus { get; set; }

        [JsonProperty("freight_taxable")]
        public bool FreightTaxable { get; set; }

        [JsonProperty("tax_source")]
        public string TaxSource { get; set; }

        [JsonProperty("exemption_type")]
        public string ExemptionType { get; set; }

        [JsonProperty("jurisdictions")]
        public IJurisdiction? Jurisdictions { get; set; }

        [JsonProperty("breakdown")]
        public IBreakdown? Breakdown { get; set; }
    }

    public class Jurisdiction : IJurisdiction
    {
        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("county")]
        public string County { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }
    }

    public class Breakdown : IBreakdown
    {
        [JsonProperty("taxable_amount")]
        public float TaxableAmount { get; set; }

        [JsonProperty("tax_collectable")]
        public float TaxCollectible { get; set; }

        [JsonProperty("combined_tax_rate")]
        public float CombinedTaxRate { get; set; }

        [JsonProperty("state_taxable_amount")]
        public float StateTaxableAmount { get; set; }

        [JsonProperty("state_tax_rate")]
        public float StateTaxRate { get; set; }

        [JsonProperty("state_tax_collectable")]
        public float StateTaxCollectable { get; set; }

        [JsonProperty("county_taxable_amount")]
        public float CountyTaxableAmount { get; set; }

        [JsonProperty("county_tax_rate")]
        public float CountyTaxRate { get; set; }

        [JsonProperty("county_tax_collectable")]
        public float CountyTaxCollectable { get; set; }

        [JsonProperty("city_taxable_amount")]
        public float CityTaxableAmount { get; set; }

        [JsonProperty("city_tax_rate")]
        public float CityTaxRate { get; set; }

        [JsonProperty("city_tax_collectable")]
        public float CityTaxCollectable { get; set; }

        [JsonProperty("special_district_taxable_amount")]
        public float SpecialDistrictTaxableAmount { get; set; }

        [JsonProperty("special_district_tax_rate")]
        public float SpecialDistrictTaxRate { get; set; }

        [JsonProperty("special_district_tax_collectable")]
        public float SpecialDistrictTaxCollectable { get; set; }

        [JsonProperty("shipping")]
        public IBreakdown? Shipping { get; set; }

        [JsonProperty("line_items")]
        public IBreakdown[]? LineItems { get; set; }
    }
}