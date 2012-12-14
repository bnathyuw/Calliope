using Calliope.Purchase.Models;
using Calliope.WebSupport;

namespace Calliope.Purchase.Services
{
	internal static class BasketServiceWrapper
	{
		public static Basket Get(int id)
		{
			var url = "http://localhost/calliope/baskets/" + id + "/";
			return WebRequester.Get<Basket>(url).Body;
		}

		public static void Delete(int id)
		{
			var url = "http://localhost/calliope/baskets/" + id + "/";
			WebRequester.Delete<Basket>(url);
		}
	}
}