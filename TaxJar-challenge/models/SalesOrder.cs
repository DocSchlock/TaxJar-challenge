using System;

namespace TaxJar_challenge.models
{
    public class SalesOrder : ISalesOrder
    {
        public Guid Id { get; set; }

        public float OrderValue { get; set; }

        public ILocation Location { get; set; }
    }
}