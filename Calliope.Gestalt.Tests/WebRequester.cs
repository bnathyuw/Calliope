using System.IO;
using System.Net;
using NUnit.Framework;

namespace Calliope.Gestalt.Tests
{
	static internal class WebRequester
	{
		public static TestWebResponse DoRequest(string url, string method)
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
			return new TestWebResponse(response);
		}
	}
}