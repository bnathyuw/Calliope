using System.Net;
using System.Text;
using Calliope.Purchase.Model;
using Nancy.Json;

namespace Calliope.Purchase.Service
{
	internal static class PaymentServiceWrapper
	{
		public static void MakePayment(int amount, string reference, string cardToken)
		{
			var request = (HttpWebRequest) WebRequest.Create("http://localhost/calliope/payments/");
			request.Method = "POST";
			request.Accept = "application/json";
			var javaScriptSerializer = new JavaScriptSerializer();
			var bodyString = javaScriptSerializer.Serialize(new Payment {Amount = amount, Reference = reference, CardToken = cardToken});
			var bodyBytes = Encoding.UTF8.GetBytes(bodyString);
			request.ContentLength = bodyBytes.Length;
			request.ContentType = "application/json";
			using (var requestStream = request.GetRequestStream())
				requestStream.Write(bodyBytes, 0, bodyBytes.Length);
			request.GetResponse();
		}
	}
}