using System.Net;

namespace Calliope.Gestalt.Tests.Web
{
	internal class TestWebResponse<T>
	{
		private readonly WebHeaderCollection _headers;
		private readonly T _responseEntity;
		private readonly HttpStatusCode _statusCode;

		public TestWebResponse(WebHeaderCollection headers, T responseEntity, HttpStatusCode statusCode)
		{
			_headers = headers;
			_responseEntity = responseEntity;
			_statusCode = statusCode;
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

		public HttpStatusCode StatusCode
		{
			get { return _statusCode; }
		}
	}
}