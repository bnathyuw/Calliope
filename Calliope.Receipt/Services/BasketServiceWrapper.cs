using Calliope.Receipt.Models;
using Calliope.WebSupport;

namespace Calliope.Receipt.Services
{
	internal static class BasketServiceWrapper
	{
		public static Basket Get(int id)
		{
			var url = "http://localhost/calliope/baskets/" + id + "/";
			return WebRequester.Get<Basket>(url).Body;
		}
	}
}