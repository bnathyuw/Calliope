using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using Calliope.Purchase.Model;
using Nancy.Json;

namespace Calliope.Purchase
{
	internal static class UserServiceWrapper
	{
		public static FolioItem AddToLocker(Item item, int userId)
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
			var response = request.GetResponse();
			var responseStream = response.GetResponseStream();
			Debug.Assert(responseStream != null, "responseStream != null");
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
			Debug.Assert(responseStream != null, "responseStream != null");
			var streamReader = new StreamReader(responseStream);
			var responseBody = streamReader.ReadToEnd();
			return new JavaScriptSerializer().Deserialize<User>(responseBody);
		}
	}
}