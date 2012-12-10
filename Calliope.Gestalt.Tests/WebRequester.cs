using System;
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
			Console.WriteLine("REQUEST {0:o}\n=====\n{1} {2} HTTP/1.1\n{3}", DateTime.Now, method, url, request.Headers);
			Console.WriteLine();
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
			var responseStream = response.GetResponseStream();
			Assert.That(responseStream != null, "responseStream != null");

			var streamReader = new StreamReader(responseStream);
			var responseBody = streamReader.ReadToEnd();
			Console.WriteLine("RESPONSE {0:o}\n=====\nHTTP/1.1 {1} {2}\n{3}\n{4}", DateTime.Now, (int) response.StatusCode, response.StatusCode, response.Headers, responseBody);
			return new TestWebResponse(response.Headers, responseBody);
		}
	}
}