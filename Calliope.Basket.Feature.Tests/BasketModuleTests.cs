using System;
using NUnit.Framework;
using Nancy;
using Nancy.Testing;

namespace Calliope.Basket.Feature.Tests
{
	[TestFixture]
	public class BasketModuleTests
	{
		[Test]
		public void Post_basket_returns_basket()
		{
			Console.WriteLine("Testing {0}", typeof (BasketModule));
			var browser = new Browser(new DefaultNancyBootstrapper());
			var browserResponse = browser.Post("/", with =>
				                                        {
					                                        with.Body("");
					                                        with.Accept("application/json");
				                                        });
			Assert.That(browserResponse != null, "browserResponse != null");
			var basket = browserResponse.Body.DeserializeJson<Basket>();
			Assert.That(basket != null, "basket != null");
		}
	}
	public class Basket
	{
		public int Id { get; set; }
		public string[] Items { get; set; }
	}
}