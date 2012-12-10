using System.Collections.Generic;
using Nancy;

namespace Calliope.Basket
{
	public class BasketModule:NancyModule
	{
		public BasketModule()
		{
			Post["/"] = o =>
				            {
					            var basket = CreateBasket();
					            return Negotiate
									.WithStatusCode(HttpStatusCode.Created)
						            .WithHeader("Location", "/" + basket.Id)
						            .WithModel(basket);
				            };

			Get["/{basketid}"] = o =>
				                     {
					                     var basket = RetrieveBasket((int)o.basketid);
					                     return Negotiate
						                     .WithStatusCode(HttpStatusCode.Found)
						                     .WithModel(basket);
				                     };
		}

		private static Basket RetrieveBasket(int basketId)
		{
			var basket = BasketStore.Find(basketId);
			var items = ItemStore.FindForBasket(basketId);
			basket.Items = items;
			return basket;
		}

		private static Basket CreateBasket()
		{
			var basket = new Basket {Items = new List<Item>()};
			BasketStore.Save(basket);
			return basket;
		}
	}
}