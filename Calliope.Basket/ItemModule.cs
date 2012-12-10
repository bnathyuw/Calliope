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