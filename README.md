# Tor.Fixer.Client

[![](https://img.shields.io/nuget/dt/Tor.Fixer.Client)](#) [![](https://img.shields.io/nuget/v/Tor.Fixer.Client)](https://www.nuget.org/packages/Tor.Fixer.Client)

A C# client library for [Fixer.io](http://fixer.io/) API with dependency injection support.

## Installation

```text
Install-Package Tor.Fixer.Client
```

## Usage

### Registering to .NET Core service collection

You have to register the **FixerClient** with the dependencies in the Program.cs file.

For the minimal registration, you have to add your Fixer.io API key to the options builder:

```text
services.AddFixer(options =>
{
    options.WithApiKey("Your API key");
});
```

#### Options

Setting the API key:

```text
services.AddFixer(options =>
{
    options.WithApiKey("Your Fixer.io API key");
});
```

If you want to implement some API key factory logic, f.e.: if you want to change your API key in runtime:

```text
public static class SharedData
{
    public static string FixerApiKey { get; set; } = "Your Fixer.io API key";
}
```

```text
services.AddFixer(options =>
{
    options.WithApiKeyFactory(() => SharedData.FixerApiKey);
});
```

If you use an alias accessing Fixer.io or the Fixer.io base address changes and this package is not updated yet, you can override the base address:

```text
services.AddFixer(options =>
{
    options.WithBaseUrl("Your URL");
});
```

Based on your design, you can choose a http error handling mode with the following code:

```text
services.AddFixer(options =>
{
    options.WithHttpErrorHandling(HttpErrorHandlingMode.ReturnsError);
});
```

There are two options (default: ReturnsError):
 - ReturnsError: when the http call ends with an errorcode, the error will be in the response, there will be no exceptions
 - ThrowsException: when http call ends with an errorcode, there will be an exception

Of course, you can combine these options for your needs except **WithApiKey** and **WithApiKeyFactory**.

### IFixerClient usage

You can get the **IFixerClient** via dependency injection:

```text
public class MyService
{
    public MyService(IFixerClient client)
    {
    }   
}
```

> **_NOTE:_**  Please note that depending on your subscription plan, certain API endpoints may or may not be available.

#### Response object

Every method call will return with the following **FixerResponse<TResult>** class:

```text
public class FixerResponse<TResult>
{
    public bool Success { get; set; }

    public TResult Result { get; set; }

    public FixerError Error { get; set; }
}
```

 - When the request succeed
   - Success: true
   - Error: null
   - Result: object
 - When the request failed
   - Success: false
   - Error: object
   - Result: null


#### IFixerClient.GetSymbolsAsync method

No method parameters.

Response:

| Property | Description                 |
| -------- | ----------------------------|
| Code     | Three letter currency code  |
| Name     | The name of the currency    |


#### IFixerClient.GetLatestRatesAsync method

Method parameters

| Parameter                 | Description                                              | Optional / Required |
| --------------------------|----------------------------------------------------------|---------------------|
| baseCurrencyCode          | Three letter base currency code                          | Optional            |
| destinationCurrencyCodes  | The codes of the expected result destination currencies  | Optional            |

Response:

| Property               | Description                     |
| ---------------------- | --------------------------------|
| BaseCurrencyCode       | Three letter base currency code |
| Date                   | The date of the data            |
| Timestamp              | The UNIX timestamp of the data  |
| Rates                  | List of the exchange rates      |
| Rates -> CurrencyCode  | Three letter currency code      |
| Rates -> ExchangeRate  | Exchange rate                   |

#### IFixerClient.GetHistoricalRatesAsync method

Method parameters

| Parameter                 | Description                                                 | Optional / Required |
| --------------------------|-------------------------------------------------------------|---------------------|
| date                      | A date in the past for which historical rates are requested | Required            |
| baseCurrencyCode          | Three letter base currency code                             | Optional            |
| destinationCurrencyCodes  | The codes of the expected result destination currencies     | Optional            |

Response:

| Property               | Description                     |
| ---------------------- | --------------------------------|
| Historical             | **true** / **false**            |
| BaseCurrencyCode       | Three letter base currency code |
| Date                   | The date of the data            |
| Timestamp              | The UNIX timestamp of the data  |
| Rates                  | List of the exchange rates      |
| Rates -> CurrencyCode  | Three letter currency code      |
| Rates -> ExchangeRate  | Exchange rate                   |

#### IFixerClient.ConvertAsync method

Method parameters

| Parameter                 | Description                                                 | Optional / Required |
| --------------------------|-------------------------------------------------------------|---------------------|
| sourceCurrencyCode        | Three letter source currency code                           | Required            |
| destinationCurrencyCode   | Three letter destination currency code                      | Required            |
| amount                    | Amount to exchange                                          | Required            |
| date                      | A date in the past for which exchange is requested          | Optional            |

Response:

| Property                           | Description                             |
| ---------------------------------- | ----------------------------------------|
| Historical                         | **true** / **false**                    |
| Date                               | The date of the data                    |
| Result                             | The converted amount                    |
| Query                              | Request query info                      |
| Query -> SourceCurrencyCode        | Three letter source currency code       |
| Query -> DestinationCurrencyCode   | Three letter destination currency code  |
| Query -> Amount                    | Amount to exchange                      |
| Info                               | Exchange info                           |
| Info -> Timestamp                  | The UNIX timestamp of the exchange rate |
| Info -> Rate                       | Exchange rate                           |

#### IFixerClient.GetTimeSeriesAsync method

// TODO

#### IFixerClient.GetFluctuationAsync method

// TODO
