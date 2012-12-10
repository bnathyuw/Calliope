using System.Collections.Generic;
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
					                             int basketId = o.basketid;
					                             var item = this.Bind<Item>();
					                             var model = CreateItem(item, basketId);
					                             return Negotiate.WithStatusCode(HttpStatusCode.Created)
					                                             .WithModel(model);
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
	}

	public class Item
	{
		public int Id { get; set; }
		public int BasketId { get; set; }
	}
}