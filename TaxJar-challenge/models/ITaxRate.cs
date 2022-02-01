using Newtonsoft.Json;

namespace TaxJar_challenge.models
{
    // this is the mapper to get NewtonSoft to work with an interface via the Interface Nuget
    // you would normally control this via another path
    [JsonConverter(typeof(InterfaceToConcreteConverter<ITaxRate, TaxJarTaxRate>))]
    public interface ITaxRate
    {
        string Country { get; set; }
        float CountryRate { get; set; }
        string County { get; set; }
        float CountyRate { get; set; }
        string State { get; set; }
        float StateRate { get; set; }
        string City { get; set; }
        float CityRate { get; set; }
        string ZipCode { get; set; }
        string Street { get; set; }

        float CombinedDistrictRate { get; set; }
        float CombinedRate { get; set; }
        bool FreightTaxable { get; set; }
    }
}