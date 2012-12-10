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

			var postResponseStream = postBasketResponse.GetResponseStream();
			Assert.That(postResponseStream != null, "responseStream != null");

			var postResponseStreamReader = new StreamReader(postResponseStream);
			var postResponseBody = postResponseStreamReader.ReadToEnd();
			Console.WriteLine(postResponseBody);

			var postBasket = new JavaScriptSerializer().Deserialize<Basket>(postResponseBody);
			Assert.That(postBasket != null, "basket != null");

			var basketUrl = postBasketResponse.Headers["Location"];
			Assert.That(basketUrl, Is.EqualTo("/baskets/1"));

			var getBasketRequest = WebRequest.Create("http://localhost/calliope" + basketUrl) as HttpWebRequest;
			Assert.That(getBasketRequest != null, "getBasketReqeust != null");
			getBasketRequest.Method = "GET";
			getBasketRequest.Accept = "application/json";

			HttpWebResponse getBasketResponse;
			try
			{
				getBasketResponse = getBasketRequest.GetResponse() as HttpWebResponse;
			}
			catch (WebException ex)
			{
				getBasketResponse = ex.Response as HttpWebResponse;
			}
			Assert.That(getBasketResponse != null, "getBasketResponse != null");

			var getResponseStream = getBasketResponse.GetResponseStream();
			Assert.That(getResponseStream != null, "responseStream != null");

			var getResponseStreamReader = new StreamReader(getResponseStream);
			var getResponseBody = getResponseStreamReader.ReadToEnd();
			Console.WriteLine(getResponseBody);

			var getBasket = new JavaScriptSerializer().Deserialize<Basket>(getResponseBody);
			Assert.That(getBasket != null, "basket != null");

		}
	}
}