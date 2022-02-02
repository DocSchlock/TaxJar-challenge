using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
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
    public class MockTestServiceTests
    {
        // private string _token;
        private TaxService _taxServiceMockText;

        private ITaxCalculator _tester;
        private Mock<ITaxCalculator> _mock;
        private TaxJarLocation _validLocation, _malformedLocation;
        private TaxJarTaxRate _compareTaxRate;
        private SalesOrder _validSalesOrder, _malformedSalesOrder;
        private TaxJarTotalTaxes _perfectReturnTaxObject;

        [TestInitialize]
        public void Setup()
        {
            //_token = File.ReadAllText("configs/key.txt");
            _validLocation = new TaxJarLocation() { Country = "US", ZipCode = "17701" };
            _malformedLocation = new TaxJarLocation() { Country = "TN", ZipCode = "ADFGH" };
            _validSalesOrder = new SalesOrder() { ToCountry = "US", Shipping = 100f, ToState = "PA", ToZip = "17701", Amount = 1000f };
            _malformedSalesOrder = new SalesOrder() { ToCountry = "US", Shipping = 100f, ToState = "", ToZip = "17701", Amount = 1000f };

            _perfectReturnTaxObject = new TaxJarTotalTaxes() { AmountToCollect = 0, FreightTaxable = true, HasNexus = false, OrderTotalAmount = 1100f, Rate = 0, TaxSource = null, Shipping = 100f, TaxableAmount = 0 };

            _compareTaxRate = new TaxJarTaxRate() { Country = "US", CountryRate = 0.0f, City = "BARBOURS", CityRate = 0.0f, FreightTaxable = true, CombinedDistrictRate = 0.0f, CombinedRate = 0.0f, ZipCode = "17701", State = "PA", StateRate = 0.0f, Street = null };

            _mock = new Mock<ITaxCalculator>();
            _tester = _mock.Object;

            _taxServiceMockText = new TaxService(_tester);

            _mock.Setup(t => t.GetTaxRateByLocation(_validLocation).Result).Returns(_compareTaxRate);
            _mock.Setup(t => t.GetTaxRateByLocation(null).Result).Throws<NullReferenceException>();
            _mock.Setup(t => t.GetTaxRateByLocation(_malformedLocation).Result).Throws<HttpRequestException>();

            _mock.Setup(t => t.GetTotalTaxesOnOrder(_validSalesOrder).Result).Returns(_perfectReturnTaxObject);
            _mock.Setup(t => t.GetTotalTaxesOnOrder(null)).Throws<ArgumentNullException>();
            _mock.Setup(t => t.GetTotalTaxesOnOrder(_malformedSalesOrder)).Throws<HttpRequestException>();
        }

        [TestMethod]
        public void MockGetTaxRateByLocationPass()
        {
            Assert.AreEqual(_compareTaxRate, _taxServiceMockText.GetTaxRateByLocation(_validLocation).Result);
            _mock.Verify(t => t.GetTaxRateByLocation(_validLocation), Times.AtMostOnce);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public async Task MockGetTaxRateByLocationNullException()
        {
            await _taxServiceMockText.GetTaxRateByLocation(null);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpRequestException))]
        public async Task MockGetTaxRateByLocationHTTPException()
        {
            await _taxServiceMockText.GetTaxRateByLocation(_malformedLocation);
        }

        [TestMethod]
        public async Task MockGetTotalTaxesPass()
        {
            var holder = await _taxServiceMockText.GetTotalTaxesOnOrder(_validSalesOrder);
            _mock.Verify(t => t.GetTotalTaxesOnOrder(_validSalesOrder), Times.AtMostOnce);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task MockGetTotalTaxesNullSO()
        {
            await _taxServiceMockText.GetTotalTaxesOnOrder(null);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpRequestException))]
        public async Task MockGetTotalTaxesNullLocation()
        {
            await _taxServiceMockText.GetTotalTaxesOnOrder(_malformedSalesOrder);
        }
    }
}