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
		private CardTransaction _cardTransaction;
		private int _amount;
		private const string ApplicationRoot = "http://localhost/calliope";

		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			var postBasketResponse = WebRequester.DoRequest<Basket>(ApplicationRoot + "/baskets/", "POST");
			_basket = postBasketResponse.Body;
			var basketUrl = postBasketResponse["Location"];

			_poems = WebRequester.DoRequest<IEnumerable<Poem>>(ApplicationRoot + "/poems/", "GET").Body.ToArray();
			_amount = _poems.Take(3).Sum(p => p.Price);

			WebRequester.DoRequest(basketUrl + "items/", "POST", new Item { Id = _poems[0].Id });
			WebRequester.DoRequest(basketUrl + "items/", "POST", new Item { Id = _poems[1].Id });
			WebRequester.DoRequest(basketUrl + "items/", "POST", new Item { Id = _poems[2].Id });

			const string cardToken = "123456";
			_postPurchaseResponse = WebRequester.DoRequest(ApplicationRoot + "/purchases/", "POST", new Purchase {BasketId = _basket.Id, CardToken = cardToken});
			_purchase = _postPurchaseResponse.Body;

			var cardTransactions = WebRequester.DoRequest<IEnumerable<CardTransaction>>(ApplicationRoot + "/stub/payment-provider/cards/" + cardToken + "/transactions/", "GET").Body;
			_cardTransaction = cardTransactions.Last();
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
			Assert.That(_purchase.Total, Is.EqualTo(_amount));
		}

		[Test]
		public void Then_the_purchase_shows_success()
		{
			Assert.That(_purchase.Status, Is.EqualTo("successful"));
		}

		[Test]
		public void Then_a_transaction_is_made_against_my_card()
		{
			Assert.That(_cardTransaction.Reference, Is.EqualTo("basket:" + _basket.Id));
		}

		[Test]
		public void Then_the_transaction_amount_is_correct()
		{
			Assert.That(_cardTransaction.Amount, Is.EqualTo(_amount));
		}
	}

	public class CardTransaction
	{
		public string Reference { get; set; }

		public int Amount { get; set; }
	}

	public class Purchase
	{
		public int BasketId { get; set; }

		public int Total { get; set; }

		public string Status { get; set; }

		public string CardToken { get; set; }
	}
}