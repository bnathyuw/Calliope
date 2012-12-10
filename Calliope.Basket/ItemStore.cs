using System.Collections.Generic;
using System.Linq;

namespace Calliope.Basket
{
	internal static class ItemStore
	{
		private static readonly IList<Item> Items = new List<Item>();

		public static void Save(Item item)
		{
			Items.Add(item);
		}

		public static IEnumerable<Item> FindForBasket(int basketId)
		{
			return Items.Where(i => i.BasketId == basketId);
		}
	}
}