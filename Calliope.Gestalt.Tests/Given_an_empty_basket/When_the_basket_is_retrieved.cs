using System.Net;
using Calliope.Gestalt.Tests.Model;
using Calliope.WebSupport;
using NUnit.Framework;

namespace Calliope.Gestalt.Tests.Given_an_empty_basket
{
	[TestFixture]
	public class When_the_basket_is_retrieved
	{
		private Basket _basket;
		private ApiResponse<Basket> _response;
		private const string ApplicationRoot = "http://localhost/calliope";

		[TestFixtureSetUp]
		public void SetUp()
		{
			var postResponse = WebRequester.Post(ApplicationRoot + "/baskets/", new Basket());

			var basketUrl = postResponse["Location"];

			_response = WebRequester.Get<Basket>(basketUrl + "/");

			_basket = _response.Body;
		}

		[Test]
		public void Then_the_response_code_is_ok()
		{
			Assert.That(_response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
		}

		[Test]
		public void Then_its_total_is_0()
		{
			Assert.That(_basket.Total, Is.EqualTo(0));
		}
	}
}