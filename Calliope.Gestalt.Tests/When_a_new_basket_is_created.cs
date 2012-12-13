using System.Net;
using Calliope.Gestalt.Tests.Model;
using Calliope.WebSupport;
using NUnit.Framework;

namespace Calliope.Gestalt.Tests
{
	[TestFixture]
	public class When_a_new_basket_is_created
	{
		private Basket _basket;
		private ApiResponse<Basket> _response;
		private string _basketUrl;
		private const string ApplicationRoot = "http://localhost/calliope";

		[TestFixtureSetUp]
		public void SetUp()
		{
			_response = WebRequester.Post(ApplicationRoot + "/baskets/", new Basket());

			_basket = _response.Body;

			_basketUrl = _response["Location"];
		}

		[Test]
		public void Then_the_response_code_is_created()
		{
			Assert.That(_response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
		}

		[Test]
		public void Then_its_location_is_correct()
		{
			Assert.That(_basketUrl, Is.EqualTo(ApplicationRoot + "/baskets/" + _basket.Id + "/"));
		}

		[Test]
		public void Then_its_total_is_0()
		{
			Assert.That(_basket.Total, Is.EqualTo(0));
		}
	}
}