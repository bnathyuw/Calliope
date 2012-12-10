using System;
using NUnit.Framework;
using Nancy;
using Nancy.Testing;

namespace Calliope.Basket.Feature.Tests
{
	[TestFixture]
	public class BasketModuleTests
	{
		private BrowserResponse _browserResponse;

		[SetUp]
		public void SetUp()
		{
			Console.WriteLine("Testing {0}", typeof (BasketModule));
			var browser = new Browser(new DefaultNancyBootstrapper());
			var browserResponse = browser.Post("/", with =>
				                                        {
					                                        with.Body("");
					                                        with.Accept("application/json");
				                                        });
			Assert.That(browserResponse != null, "browserResponse != null");
			_browserResponse = browserResponse;
		}

		[Test]
		public void Post_basket_returns_basket()
		{
			var basket = _browserResponse.Body.DeserializeJson<Basket>();
			Assert.That(basket != null, "basket != null");
		}

		[Test]
		public void Post_basket_gives_location()
		{
			Assert.That(_browserResponse.Headers["Location"] != null, "Location != null");
		}
	}
	public class Basket
	{
		public int Id { get; set; }
		public string[] Items { get; set; }
	}
}