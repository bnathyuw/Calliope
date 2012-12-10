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
						            .WithHeader("Location", "/" + basket.Id)
						            .WithModel(basket);
				            };

			Get["/{basketid}"] = o =>
				                     {
					                     var basket = RetrieveBasket((int)o.basketid);
					                     return Negotiate
						                     .WithModel(basket);
				                     };
		}

		private static Basket RetrieveBasket(int basketId)
		{
			var basket = BasketStore.Find(basketId);
			return basket;
		}

		private static Basket CreateBasket()
		{
			var basket = new Basket {Items = new string[] {}};
			BasketStore.Save(basket);
			return basket;
		}
	}
}