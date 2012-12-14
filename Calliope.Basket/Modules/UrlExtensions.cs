using System;
using Calliope.Basket.Models;
using Nancy;

namespace Calliope.Basket.Modules
{
	static internal class UrlExtensions
	{
		public static string GetUrl(this NancyModule module, Item item)
		{
			return String.Format("{0}://{1}{2}/{3}/items/{4}/", module.Request.Url.Scheme, module.Request.Url.HostName, module.Request.Url.BasePath, item.BasketId, item.Id);
		}

		public static string GetUrl(this NancyModule module, Models.Basket basket)
		{
			return String.Format("{0}://{1}{2}/{3}/", module.Request.Url.Scheme, module.Request.Url.HostName, module.Request.Url.BasePath, basket.Id);
		}
	}
}