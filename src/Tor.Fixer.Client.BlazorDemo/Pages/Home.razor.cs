namespace Tor.Fixer.Client.BlazorDemo.Pages
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
