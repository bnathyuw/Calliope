using System.Collections.Generic;
using System.Linq;
using System.Net;
using NUnit.Framework;

namespace Calliope.Gestalt.Tests.Given_an_empty_basket
{
	[TestFixture]
	public class When_an_item_is_added
	{
		private const string ApplicationRoot = "http://localhost/calliope";
		private Item _item;
		private TestWebResponse<Item> _response;
		private string _itemLocation;
		private string _basketUrl;
		private IEnumerable<Poem> _poems;
		private Poem _poem;

		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			var postBasketResponse = WebRequester.DoRequest<Basket>(ApplicationRoot + "/baskets/", "POST");

			_basketUrl = postBasketResponse["Location"];

			_poems = WebRequester.DoRequest<IEnumerable<Poem>>(ApplicationRoot + "/poems/", "GET").Body;
			_poem = _poems.ElementAt(2);

			_response = WebRequester.DoRequest(_basketUrl + "items/", "POST", new Item {Id = _poem.Id});

			_item = _response.Body;
			_itemLocation = _response["Location"];
		}

		[Test]
		public void Then_the_response_code_is_created()
		{
			Assert.That(_response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
		}

		[Test]
		public void Then_its_location_is_correct()
		{
			Assert.That(_itemLocation, Is.EqualTo(_basketUrl + "items/" + _poem.Id + "/"));
		}

		[Test]
		public void Then_the_item_shows_the_correct_id()
		{
			Assert.That(_item.Id, Is.EqualTo(_poem.Id));
		}

		[Test]
		public void Then_the_item_shows_the_correct_title()
		{
			Assert.That(_item.Title, Is.EqualTo(_poem.Title));
		}

		[Test]
		public void Then_the_item_shows_the_correct_poet()
		{
			Assert.That(_item.Poet, Is.EqualTo(_poem.Poet));
		}

		[Test]
		public void Then_the_item_shows_the_correct_first_line()
		{
			Assert.That(_item.FirstLine, Is.EqualTo(_poem.FirstLine));
		}

		[Test]
		public void Then_the_item_shows_the_correct_price()
		{
			Assert.That(_item.Price, Is.EqualTo(_poem.Price));
		}
	}
}