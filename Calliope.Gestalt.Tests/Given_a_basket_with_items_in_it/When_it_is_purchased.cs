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
		private Poem[] _poems;
		private const string ApplicationRoot = "http://localhost/calliope";

		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			var postBasketResponse = WebRequester.DoRequest<Basket>(ApplicationRoot + "/baskets/", "POST");
			_basket = postBasketResponse.Body;
			var basketUrl = postBasketResponse["Location"];

			_poems = WebRequester.DoRequest<IEnumerable<Poem>>(ApplicationRoot + "/poems/", "GET").Body.ToArray();
			
			WebRequester.DoRequest(basketUrl + "items/", "POST", new Item { Id = _poems[0].Id });
			WebRequester.DoRequest(basketUrl + "items/", "POST", new Item { Id = _poems[1].Id });
			WebRequester.DoRequest(basketUrl + "items/", "POST", new Item { Id = _poems[2].Id });

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

		[Test]
		public void Then_the_purchase_shows_the_correct_total()
		{
			Assert.That(_purchase.Total, Is.EqualTo(_poems.Take(3).Sum(p => p.Price)));
		}

		[Test]
		public void Then_the_purchase_shows_success()
		{
			Assert.That(_purchase.Status, Is.EqualTo("successful"));
		}
	}

	public class Purchase
	{
		public int BasketId { get; set; }

		public int Total { get; set; }

		public string Status { get; set; }
	}
}