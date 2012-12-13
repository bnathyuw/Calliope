using System.Net;
using System.Text;
using Calliope.Receipt.Model;
using Nancy.Json;

namespace Calliope.Receipt.Service
{
	internal static class EmailSenderWrapper
	{
		public static void SendEmail(Email email)
		{
			var request = (HttpWebRequest)WebRequest.Create("http://localhost/calliope/stub/email-sender/emails/");
			request.Method = "POST";
			request.Accept = "application/json";
			var javaScriptSerializer = new JavaScriptSerializer();
			var bodyString = javaScriptSerializer.Serialize(email);
			var bodyBytes = Encoding.UTF8.GetBytes(bodyString);
			request.ContentLength = bodyBytes.Length;
			request.ContentType = "application/json";
			using (var requestStream = request.GetRequestStream())
				requestStream.Write(bodyBytes, 0, bodyBytes.Length);
			request.GetResponse();
		}
	}
}