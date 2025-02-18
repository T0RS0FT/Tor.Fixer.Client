using Tor.Fixer.Client.Extensions;

namespace Tor.Fixer.Client.Tests
{
    [TestClass]
    public class DateOnlyExtensionsTests
    {
        [TestMethod]
        public void DateOnlyExtensionsToFixerFormatTest()
        {
            var date = DateOnly.FromDateTime(DateTime.UtcNow);

            var expected = $"{date.Year:D4}-{date.Month:D2}-{date.Day:D2}";

            Assert.AreEqual(expected, date.ToFixerFormat());
        }
    }
}
