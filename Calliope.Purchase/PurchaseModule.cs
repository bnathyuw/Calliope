using System.IO;
using System.Net;
using System.Text;
using Nancy;
using Nancy.Json;
using Nancy.ModelBinding;
using HttpStatusCode = Nancy.HttpStatusCode;

namespace Calliope.Purchase
{
	public class PurchaseModule:NancyModule
	{
		public PurchaseModule()
		{
			Post["/"] = o =>
				            {
					            var request = this.Bind<Purchase>();
					            var purchase = MakePurchase(request);
					            return Negotiate.WithStatusCode(HttpStatusCode.Created)
					                            .WithModel(purchase);
				            };
		}

		private static Purchase MakePurchase(Purchase purchase)
		{
			var basket = BasketServiceWrapper.Get(purchase.BasketId);
	
			PaymentServiceWrapper.MakePayment(basket.Total, "basket:" + basket.Id, purchase.CardToken);

			EmailSenderWrapper.SendReceipt(purchase.User);

			purchase.Total = basket.Total;
			purchase.Status = "successful";
			return purchase;
		}
	}

	internal static class EmailSenderWrapper
	{
		public static Receipt SendReceipt(string to)
		{
			var request = (HttpWebRequest)WebRequest.Create("http://localhost/calliope/stub/email-sender/receipts/");
			request.Method = "POST";
			request.Accept = "application/json";
			var javaScriptSerializer = new JavaScriptSerializer();
			var bodyString = javaScriptSerializer.Serialize(new Receipt {To = to});
			var bodyBytes = Encoding.UTF8.GetBytes(bodyString);
			request.ContentLength = bodyBytes.Length;
			request.ContentType = "application/json";
			using (var requestStream = request.GetRequestStream())
				requestStream.Write(bodyBytes, 0, bodyBytes.Length);
			var response = request.GetResponse();
			var responseStream = response.GetResponseStream();
			var streamReader = new StreamReader(responseStream);
			var responseBody = streamReader.ReadToEnd();
			return javaScriptSerializer.Deserialize<Receipt>(responseBody);
		}
	}

	internal class Receipt
	{
		public string To { get; set; }
	}
}