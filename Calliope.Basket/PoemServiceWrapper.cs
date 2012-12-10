using System.IO;
using System.Net;
using Nancy.Json;

namespace Calliope.Basket
{
	public class PoemServiceWrapper
	{
		public static Poem Get(int id)
		{
			var webRequest = (HttpWebRequest)WebRequest.Create("http://localhost/calliope/poems/" + id + "/");
			webRequest.Method = "GET";
			webRequest.Accept = "application/json";
			var webResponse = webRequest.GetResponse();
			var responseStream = webResponse.GetResponseStream();
			var streamReader = new StreamReader(responseStream);
			var responseBody = streamReader.ReadToEnd();
			var poem = new JavaScriptSerializer().Deserialize<Poem>(responseBody);
			return poem;
		}
	}
}