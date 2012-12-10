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
						            .WithHeader("Location", "/baskets/1")
						            .WithModel(basket);
				            };

			Get["/{basketid}"] = o => Negotiate
				                          .WithModel(new Basket {Id = 1, Items = new string[] {}});
		}

		private static Basket CreateBasket()
		{
			var basket = new Basket {Items = new string[] {}};
			BasketStore.Save(basket);
			return basket;
		}
	}

	internal static class BasketStore
	{
		private static readonly IList<Basket> Store = new List<Basket>();

		public static void Save(Basket basket)
		{
			basket.Id = Store.Any() ? Store.Max(b => b.Id) + 1 : 1;
			Store.Add(basket);
		}
	}
}