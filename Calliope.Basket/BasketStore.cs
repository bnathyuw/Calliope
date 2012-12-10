using System.Collections.Generic;
using System.Linq;

namespace Calliope.Basket
{
	internal static class BasketStore
	{
		private static readonly IList<Basket> Store = new List<Basket>();

		public static void Save(Basket basket)
		{
			basket.Id = Store.Any() ? Store.Max(b => b.Id) + 1 : 1;
			Store.Add(basket);
		}

		public static Basket Find(int basketId)
		{
			return Store.SingleOrDefault(b => b.Id == basketId);
		}
	}
}