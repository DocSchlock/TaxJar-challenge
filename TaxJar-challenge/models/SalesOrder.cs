using Newtonsoft.Json;

namespace TaxJar_challenge.models
{
    public class SalesOrder : ISalesOrder
    {
        [JsonProperty("from_country")]
        public string? FromCountry { get; set; }

        [JsonProperty("from_zip")]
        public string? FromZip { get; set; }

        [JsonProperty("from_state")]
        public string? FromState { get; set; }

        [JsonProperty("from_city")]
        public string? FromCity { get; set; }

        [JsonProperty("from_street")]
        public string? FromStreet { get; set; }

        [JsonProperty("to_country")]
        public string ToCountry { get; set; }

        [JsonProperty("to_zip")]
        public string? ToZip { get; set; }

        [JsonProperty("to_state")]
        public string? ToState { get; set; }

        [JsonProperty("to_city")]
        public string? ToCity { get; set; }

        [JsonProperty("to_street")]
        public string? ToStreet { get; set; }

        [JsonProperty("amount")]
        public float? Amount { get; set; }

        [JsonProperty("shipping")]
        public float Shipping { get; set; }

        [JsonProperty("customer_id")]
        public string? CustomerId { get; set; }

        [JsonProperty("exemption_type")]
        public string? ExemptionType { get; set; }

        [JsonProperty("nexus_addresses")]
        public ILocation[]? NexusAddresses { get; set; }

        [JsonProperty("line_items")]
        public ILineItem[]? LineItems { get; set; }
    }

    public class LineItem : ILineItem
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("quantity")]
        public float Quantity { get; set; }

        [JsonProperty("product_tax_code")]
        public string ProductTaxCode { get; set; }

        [JsonProperty("unit_price")]
        public float UnitPrice { get; set; }

        [JsonProperty("discount")]
        public float Discount { get; set; }
    }
}