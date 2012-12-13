using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;

namespace Calliope.WebSupport
{
	public class WebRequester
	{
		public static void Post<T>(string url, T body)
		{
			var request = CreateRequest(url, "POST");
			WriteRequestBody(body, request);
			request.GetResponse();
		}

		public static T Get<T>(string url)
		{
			var request = CreateRequest(url, "GET");
			var response = request.GetResponse();
			return ReadResponseBody<T>(response);
		}

		private static T ReadResponseBody<T>(WebResponse response)
		{
			var responseStream = response.GetResponseStream();
			Debug.Assert(responseStream != null, "responseStream != null");
			var streamReader = new StreamReader(responseStream);
			var responseBody = streamReader.ReadToEnd();
			return new JavaScriptSerializer().Deserialize<T>(responseBody);
		}

		public static void Delete(string url)
		{
			var request = CreateRequest(url, "DELETE");
			request.GetResponse();
		}

		private static HttpWebRequest CreateRequest(string url, string method)
		{
			var request = (HttpWebRequest) WebRequest.Create(url);
			request.Method = method;
			request.Accept = "application/json";
			return request;
		}

		private static void WriteRequestBody<T>(T body, HttpWebRequest request)
		{
			var javaScriptSerializer = new JavaScriptSerializer();
			var bodyString = javaScriptSerializer.Serialize(body);
			var bodyBytes = Encoding.UTF8.GetBytes(bodyString);
			request.ContentLength = bodyBytes.Length;
			request.ContentType = "application/json";
			using (var requestStream = request.GetRequestStream())
				requestStream.Write(bodyBytes, 0, bodyBytes.Length);
		}
	}
}
