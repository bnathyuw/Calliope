using System.Net;
using NUnit.Framework;

namespace Calliope.Gestalt.Tests.Given_an_empty_basket
{
	[TestFixture]
	public class When_the_basket_is_retrieved
	{
		private Basket _basket;
		private TestWebResponse<Basket> _response;
		private const string ApplicationRoot = "http://localhost/calliope";

		[TestFixtureSetUp]
		public void SetUp()
		{
			var postResponse = WebRequester.DoRequest<Basket>(ApplicationRoot + "/baskets/", "POST");

			var basketUrl = postResponse["Location"];

			_response = WebRequester.DoRequest<Basket>(basketUrl + "/", "GET");

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