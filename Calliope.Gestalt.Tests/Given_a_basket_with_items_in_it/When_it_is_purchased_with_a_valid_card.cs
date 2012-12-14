using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Calliope.Gestalt.Tests.Model;
using Calliope.WebSupport;
using NUnit.Framework;

namespace Calliope.Gestalt.Tests.Given_a_basket_with_items_in_it
{
	[TestFixture]
	public class When_it_is_purchased_with_a_valid_card
	{
		private ApiResponse<Purchase> _postPurchaseResponse;
		private Purchase _purchase;
		private Basket _basket;
		private Poem[] _poems;
		private CardTransaction _cardTransaction;
		private int _amount;
		private string _basketUrl;
		private Card _card;
		private Email _email;
		private IEnumerable<FolioItem> _folio;
		private User _user;
		private const string ApplicationRoot = "http://localhost/calliope";
		private const string UserEmail = "matthew.butt@7digital.com";

		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			Given_a_basket();

			And_a_user();

			And_various_poems();

			And_three_items_in_the_basket();

			And_a_card();

			When_the_basket_is_purchased();

			And_the_most_recent_card_transaction_is_retrieved();

			And_the_most_recent_email_is_retrieved();

			And_the_users_folio_is_retrieved();
		}

		private void Given_a_basket()
		{
			var postBasketResponse = WebRequester.Post(ApplicationRoot + "/baskets/", new Basket(), Console.WriteLine);
			_basket = postBasketResponse.Body;
			_basketUrl = postBasketResponse["Location"];
		}

		private void And_a_user()
		{
			var response = WebRequester.Post(ApplicationRoot + "/users/", new User { Email = UserEmail }, Console.WriteLine);
			_user = response.Body;
		}

		private void And_various_poems()
		{
			_poems = WebRequester.Get<IEnumerable<Poem>>(ApplicationRoot + "/poems/", Console.WriteLine).Body.Take(3).ToArray();
		}

		private void And_three_items_in_the_basket()
		{
			foreach (var poem in _poems)
			{
				WebRequester.Post(_basketUrl + "items/", new Item { Id = poem.Id }, Console.WriteLine);
				_amount += poem.Price;
			}
		}

		private void And_a_card()
		{
			var response = WebRequester.Post(ApplicationRoot + "/stub/payment-provider/cards/", new Card { Number = "5454545454545454", ExpiryDate = "2020/10" }, Console.WriteLine);
			_card = response.Body;
		}

		private void When_the_basket_is_purchased()
		{
			_postPurchaseResponse = WebRequester.Post(ApplicationRoot + "/purchases/", new Purchase
				                                                                           {
					                                                                           BasketId = _basket.Id,
					                                                                           CardToken = _card.Token,
					                                                                           UserId = _user.Id
				                                                                           }, Console.WriteLine);
			_purchase = _postPurchaseResponse.Body;
		}

		private void And_the_most_recent_card_transaction_is_retrieved()
		{
			var cardTransactions = WebRequester.Get<IEnumerable<CardTransaction>>(ApplicationRoot + "/stub/payment-provider/cards/" + _card.Token + "/transactions/", Console.WriteLine).Body;
			_cardTransaction = cardTransactions.LastOrDefault();
		}

		private void And_the_most_recent_email_is_retrieved()
		{
			var response = WebRequester.Get<IEnumerable<Email>>(ApplicationRoot + "/stub/email-sender/emails/", Console.WriteLine);
			_email = response.Body.LastOrDefault();
		}

		private void And_the_users_folio_is_retrieved()
		{
			var response = WebRequester.Get<IEnumerable<FolioItem>>(ApplicationRoot + "/users/" + _user.Id + "/folio/", Console.WriteLine);
			_folio = response.Body;
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
		public void Then_a_transaction_is_made_against_the_users_card()
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

		[Test]
		public void Then_a_receipt_is_sent()
		{
			Assert.That(_email, Is.Not.Null);
		}

		[Test]
		public void Then_the_receipt_goes_to_the_correct_receipient()
		{
			Assert.That(_email.To, Is.EqualTo(UserEmail));
		}

		[Test]
		public void Then_the_receipt_comes_from_the_correct_sender()
		{
			Assert.That(_email.From, Is.EqualTo("sales@calliope.com"));
		}

		[Test]
		public void Then_the_receipt_has_the_correct_subject()
		{
			Assert.That(_email.Subject, Is.EqualTo("Thank you for your purchase from Calliope"));
		}

		[Test]
		public void Then_the_receipt_has_the_correct_content()
		{
			var stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(@"Dear {0}
Thank you for your purchase from Calliope; here is your receipt.
Items purchased:
", UserEmail);
			foreach (var poem in _poems)
			{
				stringBuilder.AppendFormat("* {0} '{1}' (¤{2})\n", poem.Poet, poem.Title, poem.Price);
			}
			stringBuilder.AppendFormat("Total: ¤{0}\nYours,\nCalliope", _amount);
			Assert.That(_email.Body, Is.EqualTo(stringBuilder.ToString()));
		}

		[Test]
		public void Then_the_users_folio_contains_the_correct_number_of_items()
		{
			Assert.That(_folio.Count(), Is.EqualTo(_poems.Count()));
		}

		[Test]
		public void Then_each_item_purchased_is_in_the_users_folio()
		{
			foreach (var poem in _poems)
			{
				Assert.That(_folio.Any(fi => fi.Title == poem.Title && fi.Poet == poem.Poet && fi.FirstLine == poem.FirstLine));
			}
		}

		[Test]
		public void Then_the_basket_can_no_longer_be_retrieved()
		{
			var response = WebRequester.Get<Basket>(_basketUrl);
			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
		}
	}
}