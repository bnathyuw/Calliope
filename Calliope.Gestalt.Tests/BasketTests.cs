using NUnit.Framework;

namespace Calliope.Gestalt.Tests
{
	[TestFixture]
	public class BasketTests
	{
		private const string ApplicationRoot = "http://localhost/calliope";
		private const string BasketRoot = ApplicationRoot + "/baskets";

		[Test]
		public void CreateBasket_creates_a_basket_which_can_be_retrieved()
		{
			var postBasketResponse = WebRequester.DoRequest<Basket>(BasketRoot + "/", "POST");

			var postedBasket = postBasketResponse.Body;

			var basketUrl = postBasketResponse["Location"];
			Assert.That(basketUrl, Is.EqualTo("/" + postedBasket.Id));

			var getBasketResponse = WebRequester.DoRequest<Basket>(BasketRoot + basketUrl, "GET");
			
			var gotBasket = getBasketResponse.Body;
			Assert.That(gotBasket != null, "basket != null");

			Assert.That(gotBasket.Id, Is.EqualTo(postedBasket.Id), "basket.Id");
			Assert.That(gotBasket.Items, Is.EqualTo(postedBasket.Items), "basket.Items");
		}

		[Test]
		public void Creating_two_baskets_returns_different_ones()
		{
			var firstBasketResponse = WebRequester.DoRequest<Basket>(BasketRoot + "/", "POST");
			var firstBasket = firstBasketResponse.Body;

			var secondBasketResponse = WebRequester.DoRequest<Basket>(BasketRoot + "/", "POST");
			var secondBasket = secondBasketResponse.Body;

			Assert.That(secondBasket.Id, Is.Not.EqualTo(firstBasket.Id), "basket.Id");
		}

		[Test]
		public void Create_basket_and_add_an_item_to_it()
		{
			var postBasketResponse = WebRequester.DoRequest<Basket>(BasketRoot + "/", "POST");

			var basketUrl = postBasketResponse["Location"];

			const int itemId = 1;
			var postItemResponse = WebRequester.DoRequest(BasketRoot + basketUrl + "/items/", "POST", new Item {Id = itemId});

			var postedItem = postItemResponse.Body;

			Assert.That(postedItem.Id, Is.EqualTo(itemId), "itemId");

			var getBasketResponse = WebRequester.DoRequest<Basket>(BasketRoot + basketUrl, "GET");

			var gotBasket = getBasketResponse.Body;
			Assert.That(gotBasket != null, "basket != null");
			Assert.That(gotBasket.Total, Is.EqualTo(5), "basket.Total");

			Assert.That(gotBasket.Items.Length, Is.EqualTo(1), "basket.Items.Length");
			var item = gotBasket.Items[0];
			Assert.That(item.Id, Is.EqualTo(itemId), "item.Id");
			Assert.That(item.Title, Is.EqualTo("51"), "item.Title");
			Assert.That(item.Poet, Is.EqualTo("Gaius Valerius Catullus"), "item.Poet");
			Assert.That(item.FirstLine, Is.EqualTo("Ille mi par esse deo uidetur"), "item.FirstLine");
			Assert.That(item.Price, Is.EqualTo(5), "item.Price");
		}
	}
}