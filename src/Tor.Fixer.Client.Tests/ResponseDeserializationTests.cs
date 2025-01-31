using System.Text.Json;
using Tor.Fixer.Client.Internal;
using Tor.Fixer.Client.Internal.Models;

namespace Tor.Fixer.Client.Tests
{
    [TestClass]
    public class ResponseDeserializationTests
    {
        [TestMethod]
        public void SymbolsDeserializeTest()
        {
            var json = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "json", "symbols.json"));

            var model = JsonSerializer.Deserialize<SymbolsModel>(json, Constants.JsonSerializerOptions);

            var result = Mappers.Symbols.Invoke(model);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
            Assert.IsTrue(result.All(x => !string.IsNullOrWhiteSpace(x.Code)));
            Assert.IsTrue(result.All(x => !string.IsNullOrWhiteSpace(x.Name)));
            Assert.IsTrue(result.GroupBy(x => x.Code).All(x => x.Count() == 1));
        }

        [TestMethod]
        public void LatestRatesDeserializeTest()
        {
            var json = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "json", "latest.json"));

            var model = JsonSerializer.Deserialize<LatestRatesModel>(json, Constants.JsonSerializerOptions);

            var result = Mappers.LatestRates.Invoke(model);

            Assert.IsNotNull(result);
            Assert.IsTrue(!string.IsNullOrWhiteSpace(result.BaseCurrencyCode));
            Assert.IsTrue(result.Timestamp > 0);
            Assert.IsNotNull(result.Rates);
            Assert.IsTrue(result.Rates.Count > 0);
            Assert.IsTrue(result.Rates.All(x => !string.IsNullOrWhiteSpace(x.CurrencyCode)));
            Assert.IsTrue(result.Rates.All(x => x.ExchangeRate > 0));
            Assert.IsTrue(result.Rates.GroupBy(x => x.CurrencyCode).All(x => x.Count() == 1));
        }

        [TestMethod]
        public void HistoricalRatesDeserializeTest()
        {
            var json = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "json", "historical.json"));

            var model = JsonSerializer.Deserialize<HistoricalRatesModel>(json, Constants.JsonSerializerOptions);

            var result = Mappers.HistoricalRates.Invoke(model);

            Assert.IsNotNull(result);
            Assert.IsTrue(!string.IsNullOrWhiteSpace(result.BaseCurrencyCode));
            Assert.IsTrue(result.Timestamp > 0);
            Assert.IsTrue(result.Historical);
            Assert.IsNotNull(result.Rates);
            Assert.IsTrue(result.Rates.Count > 0);
            Assert.IsTrue(result.Rates.All(x => !string.IsNullOrWhiteSpace(x.CurrencyCode)));
            Assert.IsTrue(result.Rates.All(x => x.ExchangeRate > 0));
            Assert.IsTrue(result.Rates.GroupBy(x => x.CurrencyCode).All(x => x.Count() == 1));
        }

        [TestMethod]
        public void ErrorDeserializeTest()
        {
            var json = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "json", "error.json"));

            var model = JsonSerializer.Deserialize<HistoricalRatesModel>(json, Constants.JsonSerializerOptions);

            var error = model.Error?.ToFixerError();

            Assert.IsNotNull(model);
            Assert.IsFalse(model.Success);
            Assert.IsNotNull(error);
            Assert.IsTrue(!string.IsNullOrWhiteSpace(error.Info));
            Assert.IsTrue(!string.IsNullOrWhiteSpace(error.Type));
            Assert.IsTrue(error.Code > 0);
        }

        [TestMethod]
        public void ConvertDeserializeTest()
        {
            var json = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "json", "convert.json"));

            var model = JsonSerializer.Deserialize<ConvertModel>(json, Constants.JsonSerializerOptions);

            var result = Mappers.Convert.Invoke(model);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.Historical);
            Assert.IsTrue(result.Result > 0);
            Assert.IsTrue(result.Date > DateOnly.MinValue);
            Assert.IsNotNull(result.Query);
            Assert.IsTrue(!string.IsNullOrWhiteSpace(result.Query.SourceCurrencyCode));
            Assert.IsTrue(!string.IsNullOrWhiteSpace(result.Query.DestinationCurrencyCode));
            Assert.IsTrue(result.Query.Amount > 0);
            Assert.IsNotNull(result.Info);
            Assert.IsTrue(result.Info.Timestamp > 0);
            Assert.IsTrue(result.Info.Rate > 0);
        }
    }
}