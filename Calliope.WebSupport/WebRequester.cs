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
		public static ApiResponse<T> Post<T>(string url, T body, Action<string> log = null)
		{
			var request = CreateRequest(url, "POST", log);
			WriteRequestBody(body, request, log);
			var response = GetResponse(request);
			if(log != null) 
				log(string.Format("\nRESPONSE {0:o}\n=====\nHTTP/1.1 {1} {2}\n{3}", DateTime.Now, (int)response.StatusCode, response.StatusCode, response.Headers));
			var responseBody = ReadResponseBody<T>(response, log);
			return new ApiResponse<T>(response, responseBody);
		}

		public static ApiResponse<T> Get<T>(string url, Action<string> log = null)
		{
			var request = CreateRequest(url, "GET", log);
			var response = GetResponse(request);
			var responseBody = ReadResponseBody<T>(response, log);
			return new ApiResponse<T>(response, responseBody);
		}

		public static ApiResponse<T> Delete<T>(string url, Action<string> log = null)
		{
			var request = CreateRequest(url, "DELETE", log);
			var response = GetResponse(request);
			return new ApiResponse<T>(response);
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

		private static HttpWebResponse GetResponse(WebRequest request)
		{
			try
			{
				return (HttpWebResponse) request.GetResponse();
			}
			catch (WebException ex)
			{
				return (HttpWebResponse) ex.Response;
			}

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

	public class ApiResponse<T>
	{
		private readonly HttpStatusCode _statusCode;
		private readonly WebHeaderCollection _headers;
		private readonly T _body;

		public ApiResponse(HttpWebResponse response, T body = default(T))
		{
			_statusCode = response.StatusCode;
			_headers = response.Headers;
			_body = body;
		}

		public HttpStatusCode StatusCode
		{
			get { return _statusCode; }
		}

		public T Body
		{
			get { return _body; }
		}

		public string this[string key]
		{
			get { return _headers[key]; }
		}
	}
}
