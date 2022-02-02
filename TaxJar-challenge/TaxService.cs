using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaxJar_challenge.models;
using TaxJar_challenge.Repositories;

namespace TaxJar_challenge
{
    public class TaxService
    {
        private readonly ITaxCalculator _taxcalc;

        public TaxService(ITaxCalculator taxcalc)
        {
            _taxcalc = taxcalc;
        }

        /// <summary>
        /// gets the tax rate object by the input ILocation using the injected API
        /// </summary>
        /// <param name="location">an object that implements ILocation</param>
        /// <returns>ITaxRate containing the tax rates at the location</returns>
        public async Task<ITaxRate> GetTaxRateByLocation(ILocation location)
        {
            return await _taxcalc.GetTaxRateByLocation(location);
        }

        public async Task<ITotalTaxes> GetTotalTaxesOnOrder(ISalesOrder order)
        {
            return await _taxcalc.GetTotalTaxesOnOrder(order);
        }
    }
}