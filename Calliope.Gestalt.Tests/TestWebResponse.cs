using System.Net;

namespace Calliope.Gestalt.Tests
{
	internal class TestWebResponse<T>
	{
		private readonly WebHeaderCollection _headers;
		private readonly T _responseEntity;

		public TestWebResponse(WebHeaderCollection headers, T responseEntity)
		{
			_headers = headers;
			_responseEntity = responseEntity;
		}

		public string this[string key]
		{
			get
			{
				return _headers[key];
			}
		}

		public T Body
		{
			get
			{
				return _responseEntity;
			}
		}
	}
}