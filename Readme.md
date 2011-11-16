Samurai
=======

If you are an online merchant and using samurai.feefighers.com, this library will
make your life easy. Integrate with the samuari.feefighters.com portal and 
process transactions.

Installation
------------

Just use NuGet to install Samurai-Client-DotNet:

	Install-Package Samurai

Or you can download source code and compile Samurai project then add Samurai.dll to your project references.
If you want to move Samurai.dll also move RestSharp.dll and Newtonsoft.Json.dll.

Configuration
-------------

Set the Samurai.Samurai.Options before you'll use this API.

	Samurai.Samurai.Options = new SamuraiOptions()
	{
		MerchantKey = "your_merchant_key",
		MerchantPassword = "your_merchant_password",
		ProcessorToken = "your_default_processor_token"
	};

If you're using ASP.NET MVC Framework put this code in Global.aspx file into 
Application_Start() method so this method should be something like this (for MVC 3):

	protected void Application_Start()
	{
		AreaRegistration.RegisterAllAreas();

		RegisterGlobalFilters(GlobalFilters.Filters);
		RegisterRoutes(RouteTable.Routes);

		Samurai.Samurai.Options = new SamuraiOptions()
		{
			MerchantKey = "your_merchant_key",
			MerchantPassword = "your_merchant_password",
			ProcessorToken = "your_default_processor_token"
		};
	}

The ProcessorToken property is optional. If you set it,
`Processor.TheProcessor` will return the processor with this token. You
can always call `new Processor("an_arbitrary_processor_token")` to
retrieve any of your processors.

### Samurai API Reference

See the [API Reference](https://samurai.feefighters.com/developers/dotnet/api-reference) for a full explanation of how this library works with the Samurai API.
    
