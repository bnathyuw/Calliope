using System.Net;
using System.Text;
using System.Web.Script.Serialization;

namespace Calliope.Purchase.Service
{
	internal static class ReceiptServiceWrapper
	{
		public static void SendReceipt(int basketId, int userId)
		{
			var request = (HttpWebRequest)WebRequest.Create("http://localhost/calliope/receipts/");
			request.Method = "POST";
			request.Accept = "application/json";
			var javaScriptSerializer = new JavaScriptSerializer();
			var bodyString = javaScriptSerializer.Serialize(new Receipt {BasketId = basketId, UserId = userId});
			var bodyBytes = Encoding.UTF8.GetBytes(bodyString);
			request.ContentLength = bodyBytes.Length;
			request.ContentType = "application/json";
			using (var requestStream = request.GetRequestStream())
				requestStream.Write(bodyBytes, 0, bodyBytes.Length);
			request.GetResponse();
		}
	}
}