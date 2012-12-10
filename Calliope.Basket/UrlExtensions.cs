using System;
using Nancy;

namespace Calliope.Basket
{
	static internal class UrlExtensions
	{
		public static string GetItemUrl(this NancyModule module, int basketId, int itemId)
		{
			return String.Format("{0}://{1}{2}/{3}/items/{4}/", module.Request.Url.Scheme, module.Request.Url.HostName, module.Request.Url.BasePath, basketId, itemId);
		}

		public static string GetBasketUrl(this NancyModule module, int basketId)
		{
			return String.Format("{0}://{1}{2}/{3}/", module.Request.Url.Scheme, module.Request.Url.HostName, module.Request.Url.BasePath, basketId);
		}
	}
}