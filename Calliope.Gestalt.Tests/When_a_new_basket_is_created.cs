using System.Net;
using NUnit.Framework;

namespace Calliope.Gestalt.Tests
{
	[TestFixture]
	public class When_a_new_basket_is_created
	{
		private Basket _basket;
		private TestWebResponse<Basket> _response;
		private string _basketUrl;
		private const string ApplicationRoot = "http://localhost/calliope";
		private const string BasketRoot = ApplicationRoot + "/baskets";

		[TestFixtureSetUp]
		public void SetUp()
		{
			_response = WebRequester.DoRequest<Basket>(BasketRoot + "/", "POST");

			_basket = _response.Body;

			_basketUrl = _response["Location"];
		}

		[Test]
		public void Then_the_response_code_is_created()
		{
			Assert.That(_response.StatusCode, Is.EqualTo(HttpStatusCode.Created), "response.StatusCode");
		}

		[Test]
		public void Then_its_location_is_correct()
		{
			Assert.That(_basketUrl, Is.EqualTo("/" + _basket.Id));
		}

		[Test]
		public void Then_its_total_is_0()
		{
			Assert.That(_basket.Total, Is.EqualTo(0), "basket.Total");
		}
	}
}