namespace TaxJar_challenge.models
{
    public class TaxJarLocation : ILocation
    {
        public string Country { get; set; }
        public string ZipCode { get; set; }

        public string? State { get; set; }

        public string? City { get; set; }

        public string? Street { get; set; }
    }
}