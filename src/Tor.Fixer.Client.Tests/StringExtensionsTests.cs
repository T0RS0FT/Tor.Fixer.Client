using Tor.Fixer.Client.Extensions;

namespace Tor.Fixer.Client.Tests
{
    [TestClass]
    public class StringExtensionsTests
    {
        [DataTestMethod]
        [DataRow("", "", true)]
        [DataRow("usd", "usd", true)]
        [DataRow("usd", "USD", true)]
        [DataRow("usd", "Usd", true)]
        [DataRow("USD", "Usd", true)]
        [DataRow("USD", "eur", false)]
        [DataRow("USD", "usd ", false)]
        public void StringExtensionsIgnoreCaseEqualsTest(string str1, string str2, bool expectedResult)
        {
            Assert.AreEqual(expectedResult, str1.IgnoreCaseEquals(str2));
        }
    }
}
