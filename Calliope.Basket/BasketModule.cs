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
						            .WithHeader("Location", GetBasketUrl(basket.Id))
						            .WithModel(basket);
				            };

			Get["/{basketid}"] = o =>
				                     {
					                     var basket = RetrieveBasket((int)o.basketid);
					                     return Negotiate
						                     .WithStatusCode(HttpStatusCode.OK)
						                     .WithModel(basket);
				                     };
		}

		private string GetBasketUrl(int basketId)
		{
			return string.Format("{0}://{1}{2}/{3}/", Request.Url.Scheme, Request.Url.HostName, Request.Url.BasePath, basketId);
		}

		private static Basket RetrieveBasket(int basketId)
		{
			var basket = BasketStore.Find(basketId);
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