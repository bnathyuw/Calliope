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
		[Test, Explicit]
		public void CreateBasket_creates_a_basket()
		{
			var webRequest = WebRequest.Create("http://localhost/calliope/baskets/");
			webRequest.Method = "POST";
			var requestStream = webRequest.GetRequestStream();
			var streamWriter = new StreamWriter(requestStream);
			streamWriter.Write("");

			HttpWebResponse webResponse;
			try
			{
				webResponse = webRequest.GetResponse() as HttpWebResponse;
			}
			catch (WebException ex)
			{
				webResponse = ex.Response as HttpWebResponse;
			}
			Assert.That(webResponse != null, "webResponse != null");

			var responseStream = webResponse.GetResponseStream();
			Assert.That(responseStream != null, "responseStream != null");

			var streamReader = new StreamReader(responseStream);
			var responseBody = streamReader.ReadToEnd();
			Console.WriteLine("responseBody");

			var basket = new JavaScriptSerializer().Deserialize<Basket>(responseBody);
			Assert.That(basket != null, "basket != null");
		}
	}

	public class Basket
	{
		public int Id { get; set; }
		public string[] Items { get; set; }
	}
}