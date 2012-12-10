using System.Net;
using System.Web.Script.Serialization;

namespace Calliope.Gestalt.Tests
{
	internal class TestWebResponse
	{
		private readonly string _responseBody;
		private readonly WebHeaderCollection _headers;

		public TestWebResponse(WebHeaderCollection headers, string responseBody)
		{
			_headers = headers;
			_responseBody = responseBody;
		}

		public string this[string key]
		{
			get
			{
				return _headers[key];
			}
		}

		public T Deserialize<T>()
		{
			return new JavaScriptSerializer().Deserialize<T>(_responseBody);
		}
	}
}