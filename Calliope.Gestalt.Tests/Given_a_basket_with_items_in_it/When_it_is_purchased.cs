﻿using System.Collections.Generic;
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
		private string _basketUrl;
		private Card _card;
		private const string ApplicationRoot = "http://localhost/calliope";

		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			Given_a_basket();

			And_various_poems_exist();

			And_three_items_are_added_to_the_basket();

			And_a_card_is_added();

			When_the_basket_is_purchased();

			And_the_most_recent_card_transaction_is_retrieved();
		}

		private void Given_a_basket()
		{
			var postBasketResponse = WebRequester.DoRequest<Basket>(ApplicationRoot + "/baskets/", "POST");
			_basket = postBasketResponse.Body;
			_basketUrl = postBasketResponse["Location"];
		}

		private void And_various_poems_exist()
		{
			_poems = WebRequester.DoRequest<IEnumerable<Poem>>(ApplicationRoot + "/poems/", "GET").Body.Take(3).ToArray();
		}

		private void And_three_items_are_added_to_the_basket()
		{
			foreach (var poem in _poems)
			{
				WebRequester.DoRequest(_basketUrl + "items/", "POST", new Item { Id = poem.Id });
				_amount += poem.Price;
			}
		}

		private void And_a_card_is_added()
		{
			var response = WebRequester.DoRequest(ApplicationRoot + "/stub/payment-provider/cards/", "POST", new Card { Number = "5454545454545454", ExpiryDate = "2020/10" });
			_card = response.Body;
		}

		private void When_the_basket_is_purchased()
		{
			_postPurchaseResponse = WebRequester.DoRequest(ApplicationRoot + "/purchases/", "POST", new Purchase {BasketId = _basket.Id, CardToken = _card.Token});
			_purchase = _postPurchaseResponse.Body;
		}

		private void And_the_most_recent_card_transaction_is_retrieved()
		{
			var cardTransactions = WebRequester.DoRequest<IEnumerable<CardTransaction>>(ApplicationRoot + "/stub/payment-provider/cards/" + _card.Token + "/transactions/", "GET").Body;
			_cardTransaction = cardTransactions.LastOrDefault();
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
			Assert.That(_cardTransaction, Is.Not.Null);
		}

		[Test]
		public void Then_the_transaction_has_the_correct_reference()
		{
			Assert.That(_cardTransaction.Reference, Is.EqualTo("basket:" + _basket.Id));
		}

		[Test]
		public void Then_the_transaction_amount_is_correct()
		{
			Assert.That(_cardTransaction.Amount, Is.EqualTo(_amount));
		}
	}
}