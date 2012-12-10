using System;
using NUnit.Framework;
using Nancy;
using Nancy.Testing;

namespace Calliope.Basket.Feature.Tests
{
	[TestFixture]
	public class GetBasketTests
	{
		private BrowserResponse _browserResponse;

		[SetUp]
		public void SetUp()
		{
			Console.WriteLine("Testing {0}", typeof(BasketModule));
			var browser = new Browser(new DefaultNancyBootstrapper());
			var postResponse = browser.Post("/", with =>
				                                     {
					                                     with.Body("");
					                                     with.Accept("application/json");
				                                     });
			Assert.That(postResponse != null, "browserResponse != null");
			var basketUrl = postResponse.Headers["Location"];

			_browserResponse = browser.Get(basketUrl, with => with.Accept("application/json"));
		}

		[Test]
		public void Basket_is_returned_in_body()
		{
			var basket = _browserResponse.Body.DeserializeJson<Basket>();
			Assert.That(basket != null, "basket != null");
		}

	}
}