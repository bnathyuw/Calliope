using System;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;
using NUnit.Framework;

namespace Calliope.Gestalt.Tests
{
	[TestFixture]
	public class BasketTests
	{
		[Test]
		public void CreateBasket_creates_a_basket_which_can_be_retrieved()
		{
			var postBasketRequest = WebRequest.Create("http://localhost/calliope/baskets/") as HttpWebRequest;
			Assert.That(postBasketRequest != null, "webRequest != null");
			postBasketRequest.Method = "POST";
			postBasketRequest.Accept = "application/json";
			new StreamWriter(postBasketRequest.GetRequestStream()).Write("");

			HttpWebResponse postBasketResponse;
			try
			{
				postBasketResponse = postBasketRequest.GetResponse() as HttpWebResponse;
			}
			catch (WebException ex)
			{
				postBasketResponse = ex.Response as HttpWebResponse;
			}
			Assert.That(postBasketResponse != null, "webResponse != null");

			var responseStream = postBasketResponse.GetResponseStream();
			Assert.That(responseStream != null, "responseStream != null");

			var streamReader = new StreamReader(responseStream);
			var responseBody = streamReader.ReadToEnd();
			Console.WriteLine(responseBody);

			var basket = new JavaScriptSerializer().Deserialize<Basket>(responseBody);
			Assert.That(basket != null, "basket != null");

			var basketUrl = postBasketResponse.Headers["Location"];
			Assert.That(basketUrl != null, "basketUrl != null");
		}
	}

	public class Basket
	{
		public int Id { get; set; }
		public string[] Items { get; set; }
	}
}