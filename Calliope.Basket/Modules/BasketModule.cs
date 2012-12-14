using System.Collections.Generic;
using System.Linq;
using Calliope.Basket.Models;
using Calliope.Basket.Stores;
using Nancy;

namespace Calliope.Basket.Modules
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
						            .WithHeader("Location", this.GetUrl(basket))
						            .WithModel(basket);
				            };

			Get["/{basketid}/"] = o =>
				                      {
					                      var basket = RetrieveBasket((int)o.basketid);
					                      return basket == null
						                             ? Negotiate.WithStatusCode(HttpStatusCode.NotFound)
						                             : Negotiate
							                               .WithStatusCode(HttpStatusCode.OK)
							                               .WithModel(basket);
				                      };

			Delete["/{basketid}/"] = o =>
				                         {
					                         var basket = DeleteBasket((int) o.basketid);
					                         return Negotiate
						                         .WithStatusCode(HttpStatusCode.OK)
						                         .WithModel(basket);
				                         };
		}

		private static Models.Basket DeleteBasket(int basketId)
		{
			var basket = BasketStore.Find(basketId);
			BasketStore.Delete(basket);
			return basket;
		}

		private static Models.Basket RetrieveBasket(int basketId)
		{
			var basket = BasketStore.Find(basketId);
			if (basket == null) return null;
			var items = ItemStore.FindForBasket(basketId).ToList();
			basket.Items = items;
			basket.Total = items.Sum(i => i.Price);
			return basket;
		}

		private static Models.Basket CreateBasket()
		{
			var basket = new Models.Basket {Items = new List<Item>()};
			BasketStore.Save(basket);
			return basket;
		}
	}
}