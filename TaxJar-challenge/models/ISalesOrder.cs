using System;

namespace TaxJar_challenge.models
{
    public interface ISalesOrder
    {
        Guid Id { get; }
        float OrderValue { get; }
        ILocation Location { get; }
    }
}