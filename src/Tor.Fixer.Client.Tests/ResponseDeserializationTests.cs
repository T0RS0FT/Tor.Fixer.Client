using System.Text.Json;
using Tor.Fixer.Client.Internal;
using Tor.Fixer.Client.Internal.Models;

namespace Tor.Fixer.Client.Tests
{
    [TestClass]
    public class ResponseDeserializationTests
    {
        private readonly JsonSerializerOptions jsonSerializerOptions = new()
        {
            IgnoreReadOnlyProperties = false,
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        };

        [TestMethod]
        public void SymbolsDeserializeTest()
        {
            var json = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "json", "symbols.json"));

            var model = JsonSerializer.Deserialize<SymbolsModel>(json, jsonSerializerOptions);

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

            var model = JsonSerializer.Deserialize<LatestRatesModel>(json, jsonSerializerOptions);

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

            var model = JsonSerializer.Deserialize<HistoricalRatesModel>(json, jsonSerializerOptions);

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

            var model = JsonSerializer.Deserialize<HistoricalRatesModel>(json, jsonSerializerOptions);

            var error = model.Error?.ToFixerError();

            Assert.IsNotNull(model);
            Assert.IsFalse(model.Success);
            Assert.IsNotNull(error);
            Assert.IsTrue(!string.IsNullOrWhiteSpace(error.Info));
            Assert.IsTrue(!string.IsNullOrWhiteSpace(error.Type));
            Assert.IsTrue(error.Code > 0);
        }
    }
}