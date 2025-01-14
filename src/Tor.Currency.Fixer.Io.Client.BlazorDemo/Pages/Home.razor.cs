namespace Tor.Currency.Fixer.Io.Client.BlazorDemo.Pages
{
    public partial class Home
    {
        private string apiKey
        {
            get
            {
                return Constants.FixerApiKey;
            }
            set
            {
                Constants.FixerApiKey = value;
            }
        }
    }
}
