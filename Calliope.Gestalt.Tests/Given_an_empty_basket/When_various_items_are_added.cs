using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Calliope.Gestalt.Tests.Given_an_empty_basket
{
	[TestFixture]
	public class When_various_items_are_added
	{
		private const string ApplicationRoot = "http://localhost/calliope";
		private string _basketUrl;
		private IEnumerable<Poem> _poems;

		[SetUp]
		public void TestFixtureSetUp()
		{
			var postBasketResponse = WebRequester.DoRequest<Basket>(ApplicationRoot + "/baskets/", "POST");

			_basketUrl = postBasketResponse["Location"];

			_poems = WebRequester.DoRequest<IEnumerable<Poem>>(ApplicationRoot + "/poems/", "GET").Body;
		}

		[TestCase(0,1)]
		[TestCase(0,1,2,3)]
		[TestCase(0,2,4,6)]
		public void Then_the_correct_total_is_given(params int[] poemIndices)
		{
			var expectedTotal = 0;
			foreach (var poem in poemIndices.Select(poemIndex => _poems.ElementAt(poemIndex)))
			{
				WebRequester.DoRequest(_basketUrl + "items/", "POST", new Item { Id = poem.Id });
				expectedTotal += poem.Price;
			}

			var response = WebRequester.DoRequest<Basket>(_basketUrl, "GET");
			var basket = response.Body;
			Assert.That(basket.Total, Is.EqualTo(expectedTotal));
		}

	}
}