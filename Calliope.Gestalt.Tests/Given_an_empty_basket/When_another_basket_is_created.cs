using Calliope.Gestalt.Tests.Model;
using Calliope.WebSupport;
using NUnit.Framework;

namespace Calliope.Gestalt.Tests.Given_an_empty_basket
{
	[TestFixture]
	public class When_another_basket_is_created
	{
		private Basket _firstBasket;
		private Basket _secondBasket;
		private string _firstBasketUrl;
		private string _secondBasketUrl;
		private const string ApplicationRoot = "http://localhost/calliope";

		[TestFixtureSetUp]
		public void SetUp()
		{
			var firstBasketResponse = WebRequester.Post(ApplicationRoot + "/baskets/", new Basket());
			_firstBasket = firstBasketResponse.Body;
			_firstBasketUrl = firstBasketResponse["Location"];

			var secondBasketResponse = WebRequester.Post(ApplicationRoot + "/baskets/", new Basket());
			_secondBasket = secondBasketResponse.Body;
			_secondBasketUrl = secondBasketResponse["Location"];
		}

		[Test]
		public void Then_they_have_different_ids()
		{
			Assert.That(_secondBasket.Id, Is.Not.EqualTo(_firstBasket.Id));
		}

		[Test]
		public void Then_they_have_different_urls()
		{
			Assert.That(_secondBasketUrl, Is.Not.EqualTo(_firstBasketUrl));
		}


	}
}