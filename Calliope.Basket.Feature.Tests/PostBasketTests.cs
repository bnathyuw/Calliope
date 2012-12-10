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
		private Browser _browser;

		[SetUp]
		public void SetUp()
		{
			Console.WriteLine("Testing {0}", typeof (BasketModule));
			_browser = new Browser(new DefaultNancyBootstrapper());
			var browserResponse = PostBasket();
			Assert.That(browserResponse != null, "browserResponse != null");
			_browserResponse = browserResponse;
		}

		private BrowserResponse PostBasket()
		{
			var browserResponse = _browser.Post("/", with =>
				                                        {
					                                        with.Body("");
					                                        with.Accept("application/json");
				                                        });
			return browserResponse;
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

		[Test]
		public void Adding_another_basket_returns_a_different_one()
		{
			var firstBasket = _browserResponse.Body.DeserializeJson<Basket>();
			var secondBasketResponse = PostBasket();
			var secondBasket = secondBasketResponse.Body.DeserializeJson<Basket>();
			Assert.That(secondBasket.Id, Is.Not.EqualTo(firstBasket.Id));
		}
	}
}