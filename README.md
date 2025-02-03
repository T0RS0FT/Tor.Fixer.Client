# Tor.Currency.Fixer.Io.Client

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
builder.Services.AddFixer(options =>
{
    options.WithApiKey("Your API key");
});
```

#### Options

Setting the API key:

```text
builder.Services.AddFixer(options =>
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
builder.Services.AddFixer(options =>
{
    options.WithApiKeyFactory(() => SharedData.FixerApiKey);
});
```

If you use an alias accessing Fixer.io or the Fixer.io base address changes and this package is not updated yet, you can override the base address:

```text
builder.Services.AddFixer(options =>
{
    options.WithBaseUrl("Your URL");
});
```

Based on your design, you can choose a http error handling mode with the following code:

```text
builder.Services.AddFixer(options =>
{
    options.WithHttpErrorHandling(HttpErrorHandlingMode.ReturnsError);
});
```

There are two options (default: ReturnsError):
 - ReturnsError: when the http call ends with an errorcode, the error will be in the response, there will be no exceptions
 - ThrowsException: when http call ends with an errorcode, there will be an exception

Of course, you can combine these options for your needs except **WithApiKey** and **WithApiKeyFactory**.
