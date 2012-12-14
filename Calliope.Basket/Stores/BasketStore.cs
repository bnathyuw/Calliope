using System.Collections.Generic;
using System.Linq;

namespace Calliope.Basket.Stores
{
	internal static class BasketStore
	{
		private static readonly IList<Models.Basket> Store = new List<Models.Basket>();

		public static void Save(Models.Basket basket)
		{
			basket.Id = Store.Any() ? Store.Max(b => b.Id) + 1 : 1;
			Store.Add(basket);
		}

		public static Models.Basket Find(int basketId)
		{
			return Store.SingleOrDefault(b => b.Id == basketId);
		}

		public static void Delete(Models.Basket basket)
		{
			Store.Remove(basket);
		}
	}
}