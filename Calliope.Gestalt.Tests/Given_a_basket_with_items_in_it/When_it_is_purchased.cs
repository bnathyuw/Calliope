using System.Collections.Generic;
using System.Linq;
using System.Net;
using NUnit.Framework;

namespace Calliope.Gestalt.Tests.Given_a_basket_with_items_in_it
{
	[TestFixture]
	public class When_it_is_purchased
	{
		private TestWebResponse<Purchase> _postPurchaseResponse;
		private Purchase _purchase;
		private Basket _basket;
		private const string ApplicationRoot = "http://localhost/calliope";

		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			var postBasketResponse = WebRequester.DoRequest<Basket>(ApplicationRoot + "/baskets/", "POST");
			_basket = postBasketResponse.Body;
			var basketUrl = postBasketResponse["Location"];

			var poems = WebRequester.DoRequest<IEnumerable<Poem>>(ApplicationRoot + "/poems/", "GET").Body.ToArray();
			
			WebRequester.DoRequest(basketUrl + "items/", "POST", new Item { Id = poems[0].Id });
			WebRequester.DoRequest(basketUrl + "items/", "POST", new Item { Id = poems[1].Id });
			WebRequester.DoRequest(basketUrl + "items/", "POST", new Item { Id = poems[2].Id });

			_postPurchaseResponse = WebRequester.DoRequest(ApplicationRoot + "/purchases/", "POST", new Purchase {BasketId = _basket.Id});
			_purchase = _postPurchaseResponse.Body;
		}

		[Test]
		public void Then_the_response_code_is_created()
		{
			Assert.That(_postPurchaseResponse.StatusCode, Is.EqualTo(HttpStatusCode.Created));
		}

		[Test]
		public void Then_the_purchase_echoes_back_the_basket_id()
		{
			Assert.That(_purchase.BasketId, Is.EqualTo(_basket.Id));
		}
	}

	public class Purchase
	{
		public int BasketId { get; set; }
	}
}