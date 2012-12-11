﻿using System.IO;
using System.Net;
using System.Text;
using Nancy.Json;

namespace Calliope.Purchase
{
	internal static class EmailSenderWrapper
	{
		public static Email SendEmail(string to, string @from)
		{
			var request = (HttpWebRequest)WebRequest.Create("http://localhost/calliope/stub/email-sender/emails/");
			request.Method = "POST";
			request.Accept = "application/json";
			var javaScriptSerializer = new JavaScriptSerializer();
			var bodyString = javaScriptSerializer.Serialize(new Email {To = to, From = @from});
			var bodyBytes = Encoding.UTF8.GetBytes(bodyString);
			request.ContentLength = bodyBytes.Length;
			request.ContentType = "application/json";
			using (var requestStream = request.GetRequestStream())
				requestStream.Write(bodyBytes, 0, bodyBytes.Length);
			var response = request.GetResponse();
			var responseStream = response.GetResponseStream();
			var streamReader = new StreamReader(responseStream);
			var responseBody = streamReader.ReadToEnd();
			return javaScriptSerializer.Deserialize<Email>(responseBody);
		}
	}
}