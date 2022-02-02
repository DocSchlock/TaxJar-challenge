using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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
        private SalesOrder _validSalesOrder, _malformedSalesOrder, _childSalesOrder;
        private TaxJarTaxRate _compareTaxRate;
        private TaxJarTotalTaxes _perfectReturnTaxObject;

        private TaxJarLocation[] _nexusAdds = new TaxJarLocation[1];
        private LineItem[] _lineItems = new LineItem[1];

        [TestInitialize]
        public void Setup()
        {
            _token = File.ReadAllText("configs/key.txt");
            _taxService = new TaxService(new TaxJarCalculator(_token));
            _validLocation = new TaxJarLocation() { Country = "US", ZipCode = "17701" };
            _malformedLocation = new TaxJarLocation() { Country = "TN", ZipCode = "ADFGH" };
            _validSalesOrder = new SalesOrder() { ToCountry = "US", Shipping = 100f, ToState = "PA", ToZip = "17701", Amount = 1000f };
            _malformedSalesOrder = new SalesOrder() { ToCountry = "US", Shipping = 100f, ToState = "", ToZip = "17701", Amount = 1000f };

            _nexusAdds[0] = new TaxJarLocation() { Id = "Main Location", Country = "US", State = "CA", City = "La Jolla", Street = "9500 Gilman Drive", ZipCode = "92093" };
            _lineItems[0] = new LineItem() { Id = "1", Quantity = 1, ProductTaxCode = "20010", UnitPrice = 15, Discount = 0 };

            _childSalesOrder = new SalesOrder()
            {
                FromCountry = "US",
                FromZip = "92093",
                FromState = "CA",
                FromCity = "La Jolla",
                FromStreet = "9500 Gilman Drive",
                ToCountry = "US",
                ToZip = "90002",
                ToState = "CA",
                ToCity = "Los Angeles",
                ToStreet = "1335 E 103rd St",
                Amount = 15,
                Shipping = 1.5f,
                NexusAddresses = _nexusAdds,
                LineItems = _lineItems
            };

            _perfectReturnTaxObject = new TaxJarTotalTaxes() { AmountToCollect = 0, FreightTaxable = true, HasNexus = false, OrderTotalAmount = 1100f, Rate = 0, TaxSource = null, Shipping = 100f, TaxableAmount = 0 };

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
            await _taxService.GetTaxRateByLocation(_malformedLocation);
        }

        [TestMethod]
        public async Task TestGetTaxRatesPass()
        {
            var taxRates = await _taxService.GetTotalTaxesOnOrder(_validSalesOrder);
            Assert.ReferenceEquals(_perfectReturnTaxObject, taxRates);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task TestGetTaxRatesNullSalesOrder()
        {
            await _taxService.GetTotalTaxesOnOrder(null);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpRequestException))]
        public async Task TestGetTaxRatesMalformedOrder()
        {
            await _taxService.GetTotalTaxesOnOrder(_malformedSalesOrder);
        }

        [TestMethod]
        public async Task TestGetTaxRatesJurisdiction()
        {
            var taxRates = await _taxService.GetTotalTaxesOnOrder(_childSalesOrder);
            Assert.IsNotNull(taxRates.Jurisdictions);
            Assert.IsNotNull(taxRates.Breakdown);
            Assert.AreEqual(taxRates.Breakdown.LineItems[0].CountyTaxableAmount, 15.0);
        }
    }
}