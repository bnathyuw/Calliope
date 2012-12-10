using NUnit.Framework;

namespace Calliope.Gestalt.Tests
{
	[TestFixture]
	public class BasketTests
	{
		private const string ApplicationRoot = "http://localhost/calliope";
		private const string BasketRoot = ApplicationRoot + "/baskets/";

		[Test]
		public void CreateBasket_creates_a_basket_which_can_be_retrieved()
		{
			var postBasketResponse = WebRequester.DoRequest(BasketRoot, "POST");

			var postedBasket = postBasketResponse.Deserialize<Basket>();
			Assert.That(postedBasket != null, "basket != null");

			var basketUrl = postBasketResponse["Location"];
			Assert.That(basketUrl, Is.EqualTo("/" + postedBasket.Id));

			var getBasketResponse = WebRequester.DoRequest(BasketRoot + basketUrl, "GET");
			
			var gotBasket = getBasketResponse.Deserialize<Basket>();
			Assert.That(gotBasket != null, "basket != null");

			Assert.That(gotBasket.Id, Is.EqualTo(postedBasket.Id), "basket.Id");
			Assert.That(gotBasket.Items, Is.EqualTo(postedBasket.Items), "basket.Items");
		}

		[Test]
		public void Creating_two_baskets_returns_different_ones()
		{
			var firstBasketResponse = WebRequester.DoRequest(BasketRoot, "POST");
			var firstBasket = firstBasketResponse.Deserialize<Basket>();

			var secondBasketResponse = WebRequester.DoRequest(BasketRoot, "POST");
			var secondBasket = secondBasketResponse.Deserialize<Basket>();

			Assert.That(secondBasket.Id, Is.Not.EqualTo(firstBasket.Id), "basket.Id");
		}
	}
}