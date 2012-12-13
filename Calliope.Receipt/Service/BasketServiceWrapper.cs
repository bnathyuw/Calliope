using Calliope.Receipt.Model;
using Calliope.WebSupport;

namespace Calliope.Receipt.Service
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