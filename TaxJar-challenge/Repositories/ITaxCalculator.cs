using System.Collections.Generic;
using System.Threading.Tasks;
using TaxJar_challenge.models;

namespace TaxJar_challenge.Repositories
{
    public interface ITaxCalculator
    {
        Task<ITaxRate> GetTaxRateByLocation(ILocation loc);

        Task<ITotalTaxes> GetTotalTaxesOnOrder(ISalesOrder order);
    }
}