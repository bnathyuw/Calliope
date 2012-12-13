using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;

namespace Calliope.WebSupport
{
	public class WebRequester
	{
		public static T Post<T>(string url, T body, Action<string> log = null)
		{
			var request = CreateRequest(url, "POST", log);
			WriteRequestBody(body, request, log);
			var response = (HttpWebResponse) request.GetResponse();
			if(log != null) 
				log(string.Format("\nRESPONSE {0:o}\n=====\nHTTP/1.1 {1} {2}\n{3}\n", DateTime.Now, (int)response.StatusCode, response.StatusCode, response.Headers));
			return ReadResponseBody<T>(response, log);
		}

		public static T Get<T>(string url, Action<string> log = null)
		{
			var request = CreateRequest(url, "GET", log);
			var response = request.GetResponse();
			return ReadResponseBody<T>(response, log);
		}

		public static void Delete(string url, Action<string> log = null)
		{
			var request = CreateRequest(url, "DELETE", log);
			request.GetResponse();
		}

		private static HttpWebRequest CreateRequest(string url, string method, Action<string> log)
		{
			var request = (HttpWebRequest) WebRequest.Create(url);
			request.Method = method;
			request.Accept = "application/json";
			if(log != null) 
				log(string.Format("REQUEST {0:o}\n=====\n{1} {2} HTTP/1.1\n{3}", DateTime.Now, method, url, request.Headers));
			return request;
		}

		private static void WriteRequestBody<T>(T body, WebRequest request, Action<string> log)
		{
			var javaScriptSerializer = new JavaScriptSerializer();
			var bodyString = javaScriptSerializer.Serialize(body);
			var bodyBytes = Encoding.UTF8.GetBytes(bodyString);
			request.ContentLength = bodyBytes.Length;
			request.ContentType = "application/json";
			using (var requestStream = request.GetRequestStream())
				requestStream.Write(bodyBytes, 0, bodyBytes.Length);
			if(log != null) 
				log(bodyString);
		}

		private static T ReadResponseBody<T>(WebResponse response, Action<string> log)
		{
			var responseStream = response.GetResponseStream();
			Debug.Assert(responseStream != null, "responseStream != null");
			var streamReader = new StreamReader(responseStream);
			var responseBody = streamReader.ReadToEnd();
			if (log != null) 
				log(responseBody);
			return new JavaScriptSerializer().Deserialize<T>(responseBody);
		}
	}
}
