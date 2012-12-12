using System.Net;
using System.Text;
using Calliope.Purchase.Model;
using Nancy.Json;

namespace Calliope.Purchase.Service
{
	internal static class UserServiceWrapper
	{
		public static void AddToLocker(Item item, int userId)
		{
			var request = (HttpWebRequest)WebRequest.Create("http://localhost/calliope/users/" + userId + "/folio/");
			request.Method = "POST";
			request.Accept = "application/json";
			var javaScriptSerializer = new JavaScriptSerializer();
			var folioItem = new FolioItem
				                {
					                Title = item.Title,
					                Poet = item.Poet,
					                FirstLine = item.FirstLine
				                };
			var bodyString = javaScriptSerializer.Serialize(folioItem);
			var bodyBytes = Encoding.UTF8.GetBytes(bodyString);
			request.ContentLength = bodyBytes.Length;
			request.ContentType = "application/json";
			using (var requestStream = request.GetRequestStream())
				requestStream.Write(bodyBytes, 0, bodyBytes.Length);
			request.GetResponse();
		}
	}
}