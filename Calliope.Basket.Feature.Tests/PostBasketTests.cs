using System;
using NUnit.Framework;
using Nancy;
using Nancy.Testing;

namespace Calliope.Basket.Feature.Tests
{
	[TestFixture]
	public class PostBasketTests
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
		public void Basket_is_returned_in_body()
		{
			var basket = _browserResponse.Body.DeserializeJson<Basket>();
			Assert.That(basket != null, "basket != null");
		}

		[Test]
		public void Location_is_returned_in_header()
		{
			Assert.That(_browserResponse.Headers["Location"] != null, "Location != null");
		}
	}
}