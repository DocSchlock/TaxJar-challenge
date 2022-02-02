using Newtonsoft.Json;

namespace TaxJar_challenge.models
{
    [JsonConverter(typeof(InterfaceToConcreteConverter<ITotalTaxes, TaxJarTotalTaxes>))]
    public interface ITotalTaxes
    {
        // object for the Taxes endpoint payload

        float OrderTotalAmount { get; set; }
        float Shipping { get; set; }
        float TaxableAmount { get; set; }
        float AmountToCollect { get; set; }
        float Rate { get; set; }
        bool HasNexus { get; set; }
        bool FreightTaxable { get; set; }
        string TaxSource { get; set; }
        string ExemptionType { get; set; }
        IJurisdiction? Jurisdictions { get; set; }
        IBreakdown? Breakdown { get; set; }
    }

    [JsonConverter(typeof(InterfaceToConcreteConverter<IJurisdiction, Jurisdiction>))]
    public interface IJurisdiction
    {
        string Country { get; set; }
        string State { get; set; }
        string County { get; set; }
        string City { get; set; }
    }

    [JsonConverter(typeof(InterfaceToConcreteConverter<IBreakdown, Breakdown>))]
    public interface IBreakdown
    {
        float TaxableAmount { get; set; }
        float TaxCollectible { get; set; }
        float CombinedTaxRate { get; set; }
        float StateTaxableAmount { get; set; }
        float StateTaxRate { get; set; }
        float StateTaxCollectable { get; set; }
        float CountyTaxableAmount { get; set; }
        float CountyTaxRate { get; set; }
        float CountyTaxCollectable { get; set; }
        float CityTaxableAmount { get; set; }
        float CityTaxRate { get; set; }
        float CityTaxCollectable { get; set; }
        float SpecialDistrictTaxableAmount { get; set; }
        float SpecialDistrictTaxRate { get; set; }
        float SpecialDistrictTaxCollectable { get; set; }

        IBreakdown? Shipping { get; set; }
        IBreakdown?[] LineItems { get; set; }
    }
}