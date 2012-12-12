using System.Collections.Generic;
using System.Linq;
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
						            .WithHeader("Location", this.GetBasketUrl(basket.Id))
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

		private static Basket DeleteBasket(int basketId)
		{
			var basket = BasketStore.Find(basketId);
			BasketStore.Delete(basket);
			return basket;
		}

		private static Basket RetrieveBasket(int basketId)
		{
			var basket = BasketStore.Find(basketId);
			if (basket == null) return null;
			var items = ItemStore.FindForBasket(basketId).ToList();
			basket.Items = items;
			basket.Total = items.Sum(i => i.Price);
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