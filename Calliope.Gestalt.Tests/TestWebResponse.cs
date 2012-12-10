using System.IO;
using System.Net;
using System.Web.Script.Serialization;
using NUnit.Framework;

namespace Calliope.Gestalt.Tests
{
	internal class TestWebResponse
	{
		private readonly HttpWebResponse _response;
		private readonly string _responseBody;

		public TestWebResponse(HttpWebResponse response)
		{
			_response = response;
			var responseStream = response.GetResponseStream();
			Assert.That(responseStream != null, "responseStream != null");

			var streamReader = new StreamReader(responseStream);
			_responseBody = streamReader.ReadToEnd();
		}

		public string this[string key]
		{
			get { return _response.Headers[key]; }
		}

		public T Deserialize<T>()
		{
			return new JavaScriptSerializer().Deserialize<T>(_responseBody);
		}
	}
}