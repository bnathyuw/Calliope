﻿using Nancy;
using Nancy.ModelBinding;

namespace Calliope.Basket
{
	public class ItemModule:NancyModule
	{
		public ItemModule()
		{
			Post["/{basketid}/items/"] = o =>
				                             {
					                             var basketId = (int) o.basketid;
					                             var item = CreateItem(this.Bind<Item>(), basketId);
					                             return Negotiate.WithHeader("Location", this.GetItemUrl(basketId, item.Id))
					                                             .WithStatusCode(HttpStatusCode.Created)
					                                             .WithModel(item);
				                             };
		}

		private static Item CreateItem(Item item, int basketId)
		{
			item.BasketId = basketId;
			var poem = PoemServiceWrapper.Get(item.Id);
			item.FirstLine = poem.FirstLine;
			item.Poet = poem.Poet;
			item.Price = poem.Price;
			item.Title = poem.Title;
			ItemStore.Save(item);
			return item;
		}
	}
}