using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using NUnit.Framework;

namespace Calliope.Gestalt.Tests
{
	static internal class WebRequester
	{
		public static TestWebResponse DoRequest(string url, string method, object body = null)
		{
			var request = WebRequest.Create(url) as HttpWebRequest;
			Assert.That(request != null, "webRequest != null");
			request.Method = method;
			request.Accept = "application/json";
			Console.WriteLine("REQUEST {0:o}\n=====\n{1} {2} HTTP/1.1\n{3}", DateTime.Now, method, url, request.Headers);
			if (method == "POST")
			{
				var bodyString = new JavaScriptSerializer().Serialize(body);
				var bodyBytes = Encoding.UTF8.GetBytes(bodyString);
				request.ContentLength = bodyBytes.Length;
				request.ContentType = "application/json";
				using (var requestStream = request.GetRequestStream())
					requestStream.Write(bodyBytes, 0, bodyBytes.Length);
				Console.WriteLine(bodyString);
			}
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
			Console.WriteLine();
			return new TestWebResponse(response.Headers, responseBody);
		}
	}
}