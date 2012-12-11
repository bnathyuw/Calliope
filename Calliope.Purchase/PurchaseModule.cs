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
			purchase.Status = "successful";
			purchase.Total = basket.Total;

			User user = UserServiceWrapper.GetUser(purchase.UserId);
			ReceiptSender.SendReceipt(purchase, basket, user);
			foreach (var item in basket.Items)
			{
				UserServiceWrapper.AddToLocker(item, purchase.UserId);
			}

			return purchase;
		}
	}

	internal static class UserServiceWrapper
	{
		public static FolioItem AddToLocker(Item item, int userId)
		{
			var request = (HttpWebRequest)WebRequest.Create("http://localhost/calliope/users/" + userId + "/folio/");
			request.Method = "POST";
			request.Accept = "application/json";
			var javaScriptSerializer = new JavaScriptSerializer();
			var bodyString = javaScriptSerializer.Serialize(new FolioItem());
			var bodyBytes = Encoding.UTF8.GetBytes(bodyString);
			request.ContentLength = bodyBytes.Length;
			request.ContentType = "application/json";
			using (var requestStream = request.GetRequestStream())
				requestStream.Write(bodyBytes, 0, bodyBytes.Length);
			var response = request.GetResponse();
			var responseStream = response.GetResponseStream();
			var streamReader = new StreamReader(responseStream);
			var responseBody = streamReader.ReadToEnd();
			return javaScriptSerializer.Deserialize<FolioItem>(responseBody);
		}

		public static User GetUser(int id)
		{
			var request = (HttpWebRequest)WebRequest.Create("http://localhost/calliope/users/" + id + "/");
			request.Method = "GET";
			request.Accept = "application/json";
			var response = request.GetResponse();
			var responseStream = response.GetResponseStream();
			var streamReader = new StreamReader(responseStream);
			var responseBody = streamReader.ReadToEnd();
			return new JavaScriptSerializer().Deserialize<User>(responseBody);
		}
	}

	internal class FolioItem
	{
	}
}