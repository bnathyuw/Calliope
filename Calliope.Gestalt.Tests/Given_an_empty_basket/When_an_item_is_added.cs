﻿using System.Net;
using NUnit.Framework;

namespace Calliope.Gestalt.Tests.Given_an_empty_basket
{
	[TestFixture]
	public class When_an_item_is_added
	{
		private Item _item;
		private TestWebResponse<Item> _response;
		private string _itemLocation;
		private string _basketUrl;
		private const string ApplicationRoot = "http://localhost/calliope";
		private const string BasketRoot = ApplicationRoot + "/baskets";
		private const int ItemId = 1;

		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			var postBasketResponse = WebRequester.DoRequest<Basket>(BasketRoot + "/", "POST");

			_basketUrl = postBasketResponse["Location"];

			_response = WebRequester.DoRequest(BasketRoot + _basketUrl + "/items/", "POST", new Item { Id = ItemId });

			_item = _response.Body;
			_itemLocation = _response["Location"];

			Assert.That(_item.Id, Is.EqualTo(ItemId), "itemId");
		}

		[Test]
		public void Then_the_response_code_is_created()
		{
			Assert.That(_response.StatusCode, Is.EqualTo(HttpStatusCode.Created), "response.StatusCode");
		}

		[Test]
		public void Then_its_location_is_correct()
		{
			Assert.That(_itemLocation, Is.EqualTo(_basketUrl + "/items/" + ItemId), "itemLocation");
		}

		[Test]
		public void Then_the_item_shows_the_details_of_the_poem_selected()
		{
			Assert.That(_item.Id, Is.EqualTo(ItemId), "item.Id");
			Assert.That(_item.Title, Is.EqualTo("51"), "item.Title");
			Assert.That(_item.Poet, Is.EqualTo("Gaius Valerius Catullus"), "item.Poet");
			Assert.That(_item.FirstLine, Is.EqualTo("Ille mi par esse deo uidetur"), "item.FirstLine");
			Assert.That(_item.Price, Is.EqualTo(5), "item.Price");
		}
	}
}