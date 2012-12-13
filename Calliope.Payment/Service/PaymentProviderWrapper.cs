using System.Net;
using System.Text;
using Calliope.Payment.Model;
using Nancy.Json;

namespace Calliope.Payment.Service
{
	internal static class PaymentProviderWrapper
	{
		public static void MakePayment(Model.Payment payment)
		{
			var request = (HttpWebRequest) WebRequest.Create("http://localhost/calliope/stub/payment-provider/cards/" + payment.CardToken + "/transactions/");
			request.Method = "POST";
			request.Accept = "application/json";
			var javaScriptSerializer = new JavaScriptSerializer();
			var bodyString = javaScriptSerializer.Serialize(new CardTransaction {Amount = payment.Amount, Reference = payment.Reference});
			var bodyBytes = Encoding.UTF8.GetBytes(bodyString);
			request.ContentLength = bodyBytes.Length;
			request.ContentType = "application/json";
			using (var requestStream = request.GetRequestStream())
				requestStream.Write(bodyBytes, 0, bodyBytes.Length);
			request.GetResponse();
		}
	}
}