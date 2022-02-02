using Newtonsoft.Json;
using System;

namespace TaxJar_challenge.models
{
    public interface ISalesOrder
    {
        // new fields
        string? FromCountry { get; set; }

        string? FromZip { get; set; }
        string? FromState { get; set; }
        string? FromCity { get; set; }
        string? FromStreet { get; set; }
        string ToCountry { get; set; } // required
        string? ToZip { get; set; } // must be there if country is US
        string? ToState { get; set; } // must be there if country is US
        string? ToCity { get; set; }
        string? ToStreet { get; set; }
        float? Amount { get; set; } // excludes shipping
        float Shipping { get; set; } // required

        string? CustomerId { get; set; }
        string? ExemptionType { get; set; } //  	Type of exemption for the order: wholesale, government, marketplace, other, or non_exempt.
        ILocation[]? NexusAddresses { get; set; }

        ILineItem[]? LineItems { get; set; }
    }

    [JsonConverter(typeof(InterfaceToConcreteConverter<ILineItem, LineItem>))]
    public interface ILineItem
    {
        string Id { get; set; }
        float Quantity { get; set; }
        string ProductTaxCode { get; set; }
        float UnitPrice { get; set; }
        float Discount { get; set; }
    }
}