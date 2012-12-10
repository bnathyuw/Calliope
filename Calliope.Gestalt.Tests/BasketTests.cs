using NUnit.Framework;

namespace Calliope.Gestalt.Tests
{
	[TestFixture]
	public class BasketTests
	{
		private const string ApplicationRoot = "http://localhost/calliope";

		[Test]
		public void CreateBasket_creates_a_basket_which_can_be_retrieved()
		{
			var postBasketResponse = WebRequester.DoRequest(ApplicationRoot + "/baskets/", "POST");

			var postedBasket = postBasketResponse.Deserialize<Basket>();
			Assert.That(postedBasket != null, "basket != null");

			var basketUrl = postBasketResponse["Location"];
			Assert.That(basketUrl, Is.EqualTo("/baskets/" + postedBasket.Id));

			var getBasketResponse = WebRequester.DoRequest(ApplicationRoot + basketUrl, "GET");
			
			var gotBasket = getBasketResponse.Deserialize<Basket>();
			Assert.That(gotBasket != null, "basket != null");

			Assert.That(gotBasket.Id, Is.EqualTo(postedBasket.Id), "basket.Id");
			Assert.That(gotBasket.Items, Is.EqualTo(postedBasket.Items), "basket.Items");
		}
	}
}