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
            try
            {
                return await _taxcalc.GetTaxRateByLocation(location);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets a Dictionary containing the RateName : RateValueOfOrder for a given ISalesOrder and a given ITaxRate
        /// Uses the injected API to calculate
        /// </summary>
        /// <param name="order">ISalesOrder object</param>
        /// <param name="taxrate">ITaxRate object</param>
        /// <returns>IDictionary<string,float> of TaxRate : TaxAmount on the Order</returns>
        public IDictionary<string, float> GetTotalTaxRate(ISalesOrder order, ITaxRate taxrate)
        {
            return _taxcalc.GetTotalTaxRate(order, taxrate);
        }
    }
}