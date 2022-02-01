using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using TaxJar_challenge.models;
using TaxJar_challenge.Repositories;

namespace TaxJar_challenge.Tests
{
    [TestClass]
    public class TaxIntegrationTests
    {
        private string _token;
        private TaxService _taxService;
        private TaxJarLocation _validLocation, _malformedLocation;
        private readonly List<ISalesOrder> _salesOrders = new List<ISalesOrder>();
        private TaxJarTaxRate _compareTaxRate;

        [TestInitialize]
        public void Setup()
        {
            _token = File.ReadAllText("configs/key.txt");
            _taxService = new TaxService(new TaxJarCalculator(_token));
            _validLocation = new TaxJarLocation() { Country = "US", ZipCode = "17701" };
            _malformedLocation = new TaxJarLocation() { Country = "TN", ZipCode = "ADFGH" };
            _salesOrders.Add(new SalesOrder() { Id = new Guid(), OrderValue = 100.00f, Location = _validLocation });

            _compareTaxRate = new TaxJarTaxRate() { Country = "US", CountryRate = 0.0f, City = "BARBOURS", CityRate = 0.0f, FreightTaxable = true, CombinedDistrictRate = 0.0f, CombinedRate = 0.0f, ZipCode = "17701", State = "PA", StateRate = 0.0f, Street = null };
        }

        [TestMethod]
        public async Task TestGetLocationPass()
        {
            var holder = await _taxService.GetTaxRateByLocation(_validLocation);
            Assert.ReferenceEquals(holder, _compareTaxRate);
        }

        [TestMethod]
        public async Task TestGetLocationPassCity()
        {
            // tests the query generator and gets an alternate city in the same zip code
            var holder = await _taxService.GetTaxRateByLocation(new TaxJarLocation() { Country = "US", ZipCode = "17701", City = "Williamsport" });
            Assert.AreEqual(holder.County.ToUpper(), "LYCOMING");
        }

        [TestMethod]
        [ExpectedException(typeof(HttpRequestException))]
        public async Task TestGetLocationBadData()
        {
            var holder = await _taxService.GetTaxRateByLocation(_malformedLocation);
        }

        [TestMethod]
        public async Task TestGetTaxRatesPass()
        {
            var _taxRate = await _taxService.GetTaxRateByLocation(_salesOrders[0].Location);
            var taxRates = _taxService.GetTotalTaxRate(_salesOrders[0], _taxRate);
            Assert.AreEqual(5, taxRates.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void TestGetTaxRatesNullSalesOrder()
        {
            var taxRates = _taxService.GetTotalTaxRate(null, _compareTaxRate);
        }
    }
}