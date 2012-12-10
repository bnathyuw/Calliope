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
			var postBasketResponse = DoRequest("http://localhost/calliope/baskets/", "POST");

			var postResponseStream = postBasketResponse.GetResponseStream();
			Assert.That(postResponseStream != null, "responseStream != null");

			var postResponseStreamReader = new StreamReader(postResponseStream);
			var postResponseBody = postResponseStreamReader.ReadToEnd();
			Console.WriteLine(postResponseBody);

			var postBasket = new JavaScriptSerializer().Deserialize<Basket>(postResponseBody);
			Assert.That(postBasket != null, "basket != null");

			var basketUrl = postBasketResponse.Headers["Location"];
			Assert.That(basketUrl, Is.EqualTo("/baskets/1"));

			var getBasketResponse = DoRequest("http://localhost/calliope" + basketUrl, "GET");
			
			var getResponseStream = getBasketResponse.GetResponseStream();
			Assert.That(getResponseStream != null, "responseStream != null");

			var getResponseStreamReader = new StreamReader(getResponseStream);
			var getResponseBody = getResponseStreamReader.ReadToEnd();
			Console.WriteLine(getResponseBody);

			var getBasket = new JavaScriptSerializer().Deserialize<Basket>(getResponseBody);
			Assert.That(getBasket != null, "basket != null");

		}

		private static HttpWebResponse DoRequest(string url, string method)
		{
			var request = WebRequest.Create(url) as HttpWebRequest;
			Assert.That(request != null, "webRequest != null");
			request.Method = method;
			request.Accept = "application/json";
			if (method == "POST") new StreamWriter(request.GetRequestStream()).Write("");

			HttpWebResponse response;
			try
			{
				response = request.GetResponse() as HttpWebResponse;
			}
			catch (WebException ex)
			{
				response = ex.Response as HttpWebResponse;
			}
			Assert.That(response != null, "webResponse != null");
			return response;
		}
	}
}