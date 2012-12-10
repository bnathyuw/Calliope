using System.Collections.Generic;
using System.Linq;
using Nancy;
using Nancy.ModelBinding;

namespace Calliope.Basket
{
	public class ItemModule:NancyModule
	{
		public ItemModule()
		{
			Post["/{basketid}/items/"] = o =>
				                             {
					                             var item = CreateItem(this.Bind<Item>(), (int) o.basketid);
					                             return Negotiate.WithStatusCode(HttpStatusCode.Created)
					                                             .WithModel(item);
				                             };
		}

		private static Item CreateItem(Item item, int basketId)
		{
			item.BasketId = basketId;
			ItemStore.Save(item);
			return item;
		}
	}

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

	public class Item
	{
		public int Id { get; set; }
		public int BasketId { get; set; }
	}
}