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
        private string _token;
        private TaxService _taxServiceMockText;
        private ITaxCalculator _tester;
        private Mock<ITaxCalculator> _mock;
        private Dictionary<string, float> _perfectTaxDict;
        private TaxJarLocation _validLocation, _malformedLocation;
        private TaxJarTaxRate _compareTaxRate;
        private readonly List<ISalesOrder> _salesOrders = new List<ISalesOrder>();

        [TestInitialize]
        public void Setup()
        {
            _token = File.ReadAllText("configs/key.txt");
            _validLocation = new TaxJarLocation() { Country = "US", ZipCode = "17701" };
            _malformedLocation = new TaxJarLocation() { Country = "TN", ZipCode = "ADFGH" };
            _salesOrders.Add(new SalesOrder() { Id = new Guid(), OrderValue = 100.00f, Location = _validLocation });

            _compareTaxRate = new TaxJarTaxRate() { Country = "US", CountryRate = 0.0f, City = "BARBOURS", CityRate = 0.0f, FreightTaxable = true, CombinedDistrictRate = 0.0f, CombinedRate = 0.0f, ZipCode = "17701", State = "PA", StateRate = 0.0f, Street = null };

            _perfectTaxDict = new Dictionary<string, float>();

            _perfectTaxDict.Add("CountryRate", _compareTaxRate.CountryRate * _salesOrders[0].OrderValue);
            _perfectTaxDict.Add("StateRate", _compareTaxRate.StateRate * _salesOrders[0].OrderValue);
            _perfectTaxDict.Add("CombinedRate", _compareTaxRate.CombinedRate * _salesOrders[0].OrderValue); // this is the main rate to use, but it's important we return all the rates
            _perfectTaxDict.Add("CombinedDistrictRate", _compareTaxRate.CombinedDistrictRate * _salesOrders[0].OrderValue);
            _perfectTaxDict.Add("CityRate", _compareTaxRate.CityRate * _salesOrders[0].OrderValue);

            _mock = new Mock<ITaxCalculator>();
            _tester = _mock.Object;

            _taxServiceMockText = new TaxService(_tester);

            _mock.Setup(t => t.GetTaxRateByLocation(_salesOrders[0].Location).Result).Returns(_compareTaxRate);
            _mock.Setup(t => t.GetTaxRateByLocation(null).Result).Throws<NullReferenceException>();
            _mock.Setup(t => t.GetTaxRateByLocation(_malformedLocation).Result).Throws<HttpRequestException>();

            _mock.Setup(t => t.GetTotalTaxRate(_salesOrders[0], _compareTaxRate)).Returns(_perfectTaxDict);
            _mock.Setup(t => t.GetTotalTaxRate(null, _compareTaxRate)).Throws<NullReferenceException>();
            _mock.Setup(t => t.GetTotalTaxRate(_salesOrders[0], null)).Throws<NullReferenceException>();
        }

        [TestMethod]
        public void MockGetTaxRateByLocationPass()
        {
            Assert.AreEqual(_compareTaxRate, _taxServiceMockText.GetTaxRateByLocation(_salesOrders[0].Location).Result);
            _mock.Verify(t => t.GetTaxRateByLocation(_salesOrders[0].Location), Times.AtMostOnce);
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
        public void MockGetTotalTaxesPass()
        {
            Assert.AreEqual(_perfectTaxDict, _taxServiceMockText.GetTotalTaxRate(_salesOrders[0], _compareTaxRate));
            _mock.Verify(t => t.GetTotalTaxRate(_salesOrders[0], _compareTaxRate), Times.AtMostOnce);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void MockGetTotalTaxesNullSO()
        {
            _taxServiceMockText.GetTotalTaxRate(null, _compareTaxRate);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void MockGetTotalTaxesNullLocation()
        {
            _taxServiceMockText.GetTotalTaxRate(_salesOrders[0], null);
        }
    }
}