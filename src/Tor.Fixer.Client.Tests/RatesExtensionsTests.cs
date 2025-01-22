using Tor.Fixer.Client.Extensions;
using Tor.Fixer.Client.Models;

namespace Tor.Fixer.Client.Tests
{
    [TestClass]
    public class RatesExtensionsTests
    {
        [DataTestMethod]
        [DataRow("EUR", "EUR", 1, true, 1)]
        [DataRow("eur", "EUR", 1, true, 1)]
        [DataRow("EUR", "eur", 1, true, 1)]
        [DataRow("eur", "eur", 1, true, 1)]
        [DataRow("EUR", "EUR", 10, true, 10)]
        [DataRow("EUR", "USD", 1, true, 1.04)]
        [DataRow("EUR", "USD", 2, true, 2.08)]
        [DataRow("USD", "EUR", 1, true, 0.96)]
        [DataRow("USD", "EUR", 5, true, 4.8)]
        [DataRow("USD", "GBP", 1, true, 0.81)]
        [DataRow("USD", "GBP", 10, true, 8.17)]
        [DataRow("USD", "USB", 1, false, 0)]
        [DataRow("USB", "USC", 1, false, 0)]
        [DataRow("USB", "USD", 1, false, 0)]
        [DataRow("", "USD", 1, false, 0)]
        [DataRow("USD", "", 1, false, 0)]
        [DataRow("", "", 1, false, 0)]
        [DataRow(null, "USD", 1, false, 0)]
        [DataRow("USD", null, 1, false, 0)]
        [DataRow(null, null, 1, false, 0)]
        [DataRow("  ", "USD", 1, false, 0)]
        [DataRow("USD", "   ", 1, false, 0)]
        [DataRow("  ", "    ", 1, false, 0)]
        public void RatesExtensionsConvertTest(
            string sourceCurrencyCode,
            string destinationCurrencyCode,
            double amount,
            bool success,
            double expectedResult)
        {
            var rates = new LatestRatesResult()
            {
                BaseCurrencyCode = "EUR",
                Rates =
                [
                    new CurrencyRateResult(){ CurrencyCode="USD", ExchangeRate=(decimal)1.04 },
                    new CurrencyRateResult(){ CurrencyCode="GBP", ExchangeRate=(decimal)0.85 }
                ]
            };

            try
            {
                var result = rates.Convert(sourceCurrencyCode, destinationCurrencyCode, (decimal)amount);

                Assert.IsTrue(Math.Abs((decimal)expectedResult - result) < 0.01m);
            }
            catch (Exception ex)
            {
                if (success)
                {
                    Assert.Fail(ex.Message);
                }
            }
        }

        [TestMethod]
        public void LatestRatesExtensionsChangeBaseCurrencyTest()
        {
            var rates = new LatestRatesResult()
            {
                BaseCurrencyCode = "EUR",
                Date = DateTime.Now.Date,
                Timestamp = (int)DateTime.Now.Date.Subtract(new DateTime(1970, 1, 1)).TotalSeconds,
                Rates =
                [
                    new CurrencyRateResult(){ CurrencyCode="USD", ExchangeRate=(decimal)1.04 },
                    new CurrencyRateResult(){ CurrencyCode="GBP", ExchangeRate=(decimal)0.85 }
                ]
            };

            var newRates = rates.ChangeBaseCurrency("USD");

            Assert.AreEqual("USD", newRates.BaseCurrencyCode);
            Assert.AreEqual(rates.Date, newRates.Date);
            Assert.AreEqual(rates.Timestamp, newRates.Timestamp);
            Assert.AreEqual(rates.Rates.Count, newRates.Rates.Count);
            Assert.IsTrue(Math.Abs(newRates.Rates.Single(x => x.CurrencyCode == "EUR").ExchangeRate - 0.96m) < 0.01m);
            Assert.IsTrue(Math.Abs(newRates.Rates.Single(x => x.CurrencyCode == "GBP").ExchangeRate - 0.81m) < 0.01m);
        }

        [TestMethod]
        public void HistoricalRatesExtensionsChangeBaseCurrencyTest()
        {
            var rates = new HistoricalRatesResult()
            {
                Historical = true,
                BaseCurrencyCode = "EUR",
                Date = DateTime.Now.Date,
                Timestamp = (int)DateTime.Now.Date.Subtract(new DateTime(1970, 1, 1)).TotalSeconds,
                Rates =
                [
                    new CurrencyRateResult(){ CurrencyCode="USD", ExchangeRate=(decimal)1.04 },
                    new CurrencyRateResult(){ CurrencyCode="GBP", ExchangeRate=(decimal)0.85 }
                ]
            };

            var newRates = rates.ChangeBaseCurrency("USD");

            Assert.AreEqual("USD", newRates.BaseCurrencyCode);
            Assert.AreEqual(rates.Historical, newRates.Historical);
            Assert.AreEqual(rates.Date, newRates.Date);
            Assert.AreEqual(rates.Timestamp, newRates.Timestamp);
            Assert.AreEqual(rates.Rates.Count, newRates.Rates.Count);
            Assert.IsTrue(Math.Abs(newRates.Rates.Single(x => x.CurrencyCode == "EUR").ExchangeRate - 0.96m) < 0.01m);
            Assert.IsTrue(Math.Abs(newRates.Rates.Single(x => x.CurrencyCode == "GBP").ExchangeRate - 0.81m) < 0.01m);
        }
    }
}
