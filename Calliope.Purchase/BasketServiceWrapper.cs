using System.IO;
using System.Net;
using Nancy.Json;

namespace Calliope.Purchase
{
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
}