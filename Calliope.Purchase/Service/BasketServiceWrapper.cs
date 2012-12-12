using System.Diagnostics;
using System.IO;
using System.Net;
using Calliope.Purchase.Model;
using Nancy.Json;

namespace Calliope.Purchase.Service
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
			Debug.Assert(responseStream != null, "responseStream != null");
			var streamReader = new StreamReader(responseStream);
			var responseBody = streamReader.ReadToEnd();
			return new JavaScriptSerializer().Deserialize<Basket>(responseBody);
		}

		public static void Delete(int id)
		{
			var request = (HttpWebRequest)WebRequest.Create("http://localhost/calliope/baskets/" + id + "/");
			request.Method = "DELETE";
			request.Accept = "application/json";
			request.GetResponse();
		}
	}
}