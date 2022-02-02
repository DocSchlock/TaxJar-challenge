namespace TaxJar_challenge.models
{
    public interface ILocation
    {
        string Id { get; set; } // for nexus
        string Country { get; set; }
        string ZipCode { get; set; }

        string? State { get; set; }

        string? City { get; set; }

        string? Street { get; set; }
    }
}