using NUnit.Framework;

namespace Calliope.Gestalt.Tests
{
	[TestFixture]
	public class BasketTests
	{
		[Test]
		public void CreateBasket_creates_a_basket_which_can_be_retrieved()
		{
			var postBasketResponse = WebRequester.DoRequest("http://localhost/calliope/baskets/", "POST");

			var postBasket = postBasketResponse.Deserialize<Basket>();
			Assert.That(postBasket != null, "basket != null");

			var basketUrl = postBasketResponse["Location"];
			Assert.That(basketUrl, Is.EqualTo("/baskets/1"));

			var getBasketResponse = WebRequester.DoRequest("http://localhost/calliope" + basketUrl, "GET");
			
			var getBasket = getBasketResponse.Deserialize<Basket>();
			Assert.That(getBasket != null, "basket != null");

		}
	}
}