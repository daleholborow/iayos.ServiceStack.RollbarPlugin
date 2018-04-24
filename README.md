# iayos.ServiceStack.RollbarPlugin

Plugin to integrate ServiceStack IRequestLogger with Rollbar API for request/error monitoring.

The author is not affiliated with
- ServiceStack.net
- Rollbar.com

, just a fan.


This ServiceStack plugin logs requests to [Rollbar](https://rollbar.com). For more details view the [upcoming blog post...](http://inayearorso.io)

In particular, we are able to make use of the free tier to log up to 5,000 requests per month, to trial Rollbar and/or use on ultra-low-volume websites while getting established.

*NB. This version is compatible with ServiceStack v5.1.x. and Rollbar API v1*

# Installing

The package is available from nuget.org

`Install-Package iayos.ServiceStack.RollbarPlugin`

# Requirements

You must have an account registered with [Rollbar](https://rollbar.com).

> As @ 24.04.2018, Rollbar offer a free tier that includes up to 5,000 requests per month, after which, payment is required for higher tiers.

# Quick Start

In your `AppHost` class `Configure` method, add the plugin. By default configuration values are read from the registered `IAppSettings` instance. By default this will be an instance of `AppSettings`, if an alternative implementation of `IAppSettings` is to be used it must be registered prior to this plugin being registered.
Alternatively all configuration options are exposed as public properties of the feature class.

```csharp
public override void Configure(Container container)
{
	var rollbarSettings = host.AppSettings.Get<RollbarSettings>("RollbarPluginSettings");
	Plugins.Add(new RollbarLoggerPlugin
	{
		ApiKey = rollbarSettings.ApiKey,
		Enabled = rollbarSettings.Enabled,
		EnableErrorTracking = rollbarSettings.EnableErrorTracking,
		EnableRequestBodyTracking = rollbarSettings.EnableRequestBodyTracking,
		EnableResponseTracking = rollbarSettings.EnableResponseTracking,
		EnableSessionTracking = rollbarSettings.EnableSessionTracking,
		Environment = rollbarSettings.Environment,
		HideRequestBodyForRequestDtoTypes = new List<Type>(),
		ExcludeRequestDtoTypes = new List<Type>
		{
			typeof(RollbarLogConfigRequest),
			typeof(SwaggerResource),
			typeof(SwaggerApiDeclaration)
		},
		RequiredRoles = rollbarSettings.RequiredRoles,
		SkipLogging = IsRequestSkippedDuringRequestLogging
	});
    
}
```
### Configuration Options (appsettings.json)

```json
"RollbarPluginSettings": {
    "ApiKey": "somekeyyougetfromrollbar",
    "Enabled": true,
    "EnableErrorTracking": true,
    "EnableRequestBodyTracking": true,
    "EnableSessionTracking": true,
    "RequiredRoles": [
        'ADMIN', 'SYSADMIN', 'ETC'
    ]
}
```

And each property toggles the following functionalities:

| Property | Description | AppSettings key |
| --- | --- | --- |
| ApiKey | Rollbar Account Api Key | ApiKey (string)|
| Enabled | Default True | Enabled (bool)|
| EnableErrorTracking | Default True | EnableErrorTracking (string)|
| EnableRequestBodyTracking | Default False | EnableRequestBodyTracking (bool)|
| EnableSessionTracking | Default False | EnableSessionTracking (bool)|
| EnableResponseTracking | Default False | EnableResponseTracking (bool)|
| AppendProperties | Add additional properties to log | N/A|
| RawEventLogger | low evel delegate for custom logging, bypasses all other settings | EnableResponseTracking (bool)|
| Logger | Swap out seq logger for custom implementation | EnableResponseTracking (bool)|
| RequiredRoles | Restrict the runtime configuration to specific roles | RequiredRoles (string[])|
| HideRequestBodyForRequestDtoTypes | Type exclusions for body request logging | N/A|
| ExcludeRequestDtoTypes | Type exclusions for logging | N/A|
| SkipLogging | Skip logging for any custom IRequest conditions | N/A



### Runtime configuration

You can change the logging configuration at runtime 

```csharp
    var request = new RollbarLogConfigRequest
    {
        Enabled = false,
        EnableRequestBodyTracking = false,
        EnableErrorTracking = false,
        EnableSessionTracking = false,
        EnableResponseTracking = false
    };

    var client = new JsonServiceClient("http://myservice");
    client.Send(request);
```


### Logging in action

Once you start your `AppHost`, every request will be now logged to Rollbar using the default options or the options you provided.
Depending on your settings, the full RequestMessage, ResponseMessage, error details and session details are available to search within the Rollbar reporting tools.

For more info on Rollbar, see [their docs](https://docs.rollbar.com/reference#items)

