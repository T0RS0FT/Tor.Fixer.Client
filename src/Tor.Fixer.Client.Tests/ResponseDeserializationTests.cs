using System.Text.Json;
using Tor.Fixer.Client.Internal;
using Tor.Fixer.Client.Internal.Models;

namespace Tor.Fixer.Client.Tests
{
    [TestClass]
    public class ResponseDeserializationTests
    {
        [TestMethod]
        public void ErrorDeserializeTest()
        {
            var json = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "json", "error.json"));

            var model = JsonSerializer.Deserialize<HistoricalRatesModel>(json, Constants.JsonSerializerOptions);

            var error = model.Error?.ToFixerError();

            Assert.IsNotNull(model);
            Assert.IsFalse(model.Success);
            Assert.IsNotNull(error);
            Assert.IsFalse(string.IsNullOrWhiteSpace(error.Info));
            Assert.IsFalse(string.IsNullOrWhiteSpace(error.Type));
            Assert.IsTrue(error.Code > 0);
        }

        [TestMethod]
        public void SymbolsDeserializeTest()
        {
            var json = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "json", "symbols.json"));

            var model = JsonSerializer.Deserialize<SymbolsModel>(json, Constants.JsonSerializerOptions);

            var result = Mappers.Symbols(model);

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

            var result = Mappers.LatestRates(model);

            Assert.IsNotNull(result);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.BaseCurrencyCode));
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

            var result = Mappers.HistoricalRates(model);

            Assert.IsNotNull(result);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.BaseCurrencyCode));
            Assert.IsTrue(result.Timestamp > 0);
            Assert.IsTrue(result.Historical);
            Assert.IsNotNull(result.Rates);
            Assert.IsTrue(result.Rates.Count > 0);
            Assert.IsTrue(result.Rates.All(x => !string.IsNullOrWhiteSpace(x.CurrencyCode)));
            Assert.IsTrue(result.Rates.All(x => x.ExchangeRate > 0));
            Assert.IsTrue(result.Rates.GroupBy(x => x.CurrencyCode).All(x => x.Count() == 1));
        }

        [TestMethod]
        public void ConvertDeserializeTest()
        {
            var json = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "json", "convert.json"));

            var model = JsonSerializer.Deserialize<ConvertModel>(json, Constants.JsonSerializerOptions);

            var result = Mappers.Convert(model);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.Historical);
            Assert.IsTrue(result.Result > 0);
            Assert.IsTrue(result.Date > DateOnly.MinValue);
            Assert.IsNotNull(result.Query);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.Query.SourceCurrencyCode));
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.Query.DestinationCurrencyCode));
            Assert.IsTrue(result.Query.Amount > 0);
            Assert.IsNotNull(result.Info);
            Assert.IsTrue(result.Info.Timestamp > 0);
            Assert.IsTrue(result.Info.Rate > 0);
        }

        [TestMethod]
        public void TimeSeriesDeserializeTest()
        {
            var json = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "json", "timeseries.json"));

            var model = JsonSerializer.Deserialize<TimeSeriesModel>(json, Constants.JsonSerializerOptions);

            var result = Mappers.TimeSeries(model);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.TimeSeries);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.BaseCurrencyCode));
            Assert.IsTrue(result.StartDate > DateOnly.MinValue);
            Assert.IsTrue(result.EndDate > DateOnly.MinValue);
            Assert.IsNotNull(result.Items);
            Assert.IsTrue(result.Items.Count > 0);
            result.Items.ForEach(item =>
            {
                Assert.IsTrue(item.Date > DateOnly.MinValue);
                Assert.IsNotNull(item.Rates);
                Assert.IsTrue(item.Rates.Count > 0);
                Assert.IsTrue(item.Rates.All(x => !string.IsNullOrWhiteSpace(x.CurrencyCode)));
                Assert.IsTrue(item.Rates.All(x => x.ExchangeRate > 0));
                Assert.IsTrue(item.Rates.GroupBy(x => x.CurrencyCode).All(x => x.Count() == 1));
            });
        }

        [TestMethod]
        public void FluctuationDeserializeTest()
        {
            var json = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "json", "fluctuation.json"));

            var model = JsonSerializer.Deserialize<FluctuationModel>(json, Constants.JsonSerializerOptions);

            var result = Mappers.Fluctuation(model);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Fluctuation);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.BaseCurrencyCode));
            Assert.IsTrue(result.StartDate > DateOnly.MinValue);
            Assert.IsTrue(result.EndDate > DateOnly.MinValue);
            Assert.IsNotNull(result.Rates);
            Assert.IsTrue(result.Rates.Count > 0);
            Assert.IsTrue(result.Rates.All(rate => !string.IsNullOrWhiteSpace(rate.CurrencyCode)));
            Assert.IsTrue(result.Rates.GroupBy(rate => rate.CurrencyCode).All(x => x.Count() == 1));
            Assert.IsTrue(result.Rates.All(rate => rate.StartRate != 0));
            Assert.IsTrue(result.Rates.All(rate => rate.EndRate != 0));
            Assert.IsTrue(result.Rates.All(rate => rate.Change != 0));
            Assert.IsTrue(result.Rates.All(rate => rate.ChangePercentage != 0));
        }
    }
}