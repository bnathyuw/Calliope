using System.Collections.Generic;
using System.Linq;
using Calliope.Gestalt.Tests.Model;
using Calliope.WebSupport;
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
			var postBasketResponse = WebRequester.Post(ApplicationRoot + "/baskets/", new Basket());

			_basketUrl = postBasketResponse["Location"];

			_poems = WebRequester.Get<IEnumerable<Poem>>(ApplicationRoot + "/poems/").Body;
		}

		[TestCase(0,1)]
		[TestCase(0,1,2,3)]
		[TestCase(0,2,4,6)]
		public void Then_the_correct_total_is_given(params int[] poemIndices)
		{
			var expectedTotal = 0;
			foreach (var poem in poemIndices.Select(poemIndex => _poems.ElementAt(poemIndex)))
			{
				WebRequester.Post(_basketUrl + "items/", new Item { Id = poem.Id });
				expectedTotal += poem.Price;
			}

			var response = WebRequester.Get<Basket>(_basketUrl);
			var basket = response.Body;
			Assert.That(basket.Total, Is.EqualTo(expectedTotal));
		}

	}
}