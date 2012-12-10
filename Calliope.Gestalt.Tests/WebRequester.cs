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
		public static TestWebResponse<T> DoRequest<T>(string url, string method, T body = default(T))
		{
			var request = CreateRequest(url, method);
			
			Console.WriteLine("REQUEST {0:o}\n=====\n{1} {2} HTTP/1.1\n{3}", DateTime.Now, method, url, request.Headers);
			
			if (method == "POST")
			{
				WriteBody(body, request);
			}
			
			var response = GetResponse(request);
			
			var responseBody = GetResponseBody(response);

			Console.WriteLine("\nRESPONSE {0:o}\n=====\nHTTP/1.1 {1} {2}\n{3}\n{4}\n", DateTime.Now, (int) response.StatusCode, response.StatusCode, response.Headers, responseBody);
			
			var responseEntity = GetResponseEntity<T>(responseBody);
			
			return new TestWebResponse<T>(response.Headers, responseEntity);
		}

		private static T GetResponseEntity<T>(string responseBody)
		{
			var responseEntity = new JavaScriptSerializer().Deserialize<T>(responseBody);
			return responseEntity;
		}

		private static string GetResponseBody(WebResponse response)
		{
			var responseStream = response.GetResponseStream();
			Assert.That(responseStream != null, "responseStream != null");
			var streamReader = new StreamReader(responseStream);
			return streamReader.ReadToEnd();
		}

		private static HttpWebResponse GetResponse(WebRequest request)
		{
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

		private static HttpWebRequest CreateRequest(string url, string method)
		{
			var request = WebRequest.Create(url) as HttpWebRequest;
			Assert.That(request != null, "webRequest != null");
			request.Method = method;
			request.Accept = "application/json";
			return request;
		}

		private static void WriteBody<T>(T body, WebRequest request)
		{
			var bodyString = new JavaScriptSerializer().Serialize(body);
			var bodyBytes = Encoding.UTF8.GetBytes(bodyString);
			request.ContentLength = bodyBytes.Length;
			request.ContentType = "application/json";
			using (var requestStream = request.GetRequestStream())
				requestStream.Write(bodyBytes, 0, bodyBytes.Length);
			Console.WriteLine(bodyString);
		}
	}
}