using System.Collections.Generic;
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
					            var purchase = this.Bind<Purchase>();
					            MakePurchase(purchase);
					            return Negotiate.WithStatusCode(HttpStatusCode.Created)
					                            .WithModel(purchase);
				            };
		}

		private static void MakePurchase(Purchase purchase)
		{
			var basket = BasketServiceWrapper.Get(purchase.BasketId);
			PaymentServiceWrapper.MakePayment(basket.Total, "basket:" + basket.Id, purchase.CardToken);
			purchase.Total = basket.Total;

			purchase.Status = "successful";
		}
	}

	internal static class PaymentServiceWrapper
	{
		public static CardTransaction MakePayment(int amount, string reference, string cardToken)
		{
			var request = (HttpWebRequest) WebRequest.Create("http://localhost/calliope/stub/payment-provider/cards/" + cardToken + "/transactions/");
			request.Method = "POST";
			request.Accept = "application/json";
			var javaScriptSerializer = new JavaScriptSerializer();
			var bodyString = javaScriptSerializer.Serialize(new CardTransaction {Amount = amount, Reference = reference});
			var bodyBytes = Encoding.UTF8.GetBytes(bodyString);
			request.ContentLength = bodyBytes.Length;
			request.ContentType = "application/json";
			using (var requestStream = request.GetRequestStream())
				requestStream.Write(bodyBytes, 0, bodyBytes.Length);
			var response = request.GetResponse();
			var responseStream = response.GetResponseStream();
			var streamReader = new StreamReader(responseStream);
			var responseBody = streamReader.ReadToEnd();
			return javaScriptSerializer.Deserialize<CardTransaction>(responseBody);
		}
	}

	internal class CardTransaction
	{
		public int Amount { get; set; }

		public string Reference { get; set; }
	}

	internal static class BasketServiceWrapper
	{
		public static Basket Get(int id)
		{
			var request = (HttpWebRequest)WebRequest.Create("http://localhost/calliope/baskets/" + id + "/");
			request.Method = "GET";
			request.Accept = "application/json";
			var response = request.GetResponse();
			var responseStream = response.GetResponseStream();
			var streamReader = new StreamReader(responseStream);
			var responseBody = streamReader.ReadToEnd();
			return new JavaScriptSerializer().Deserialize<Basket>(responseBody);
		}
	}

	public class Basket
	{
		public int Id { get; set; }
		public IEnumerable<Item> Items { get; set; }
		public int Total { get; set; }
	}

	public class Item
	{
		public int Id { get; set; }
		public int BasketId { get; set; }
		public string Title { get; set; }
		public string Poet { get; set; }
		public string FirstLine { get; set; }
		public int Price { get; set; }
	}

	public class Purchase
	{
		public int BasketId { get; set; }
		public int Total { get; set; }
		public string Status { get; set; }
		public string CardToken { get; set; }
	}
}