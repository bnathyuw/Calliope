using Nancy;

namespace Calliope.Basket
{
	public class BasketModule:NancyModule
	{
		public BasketModule()
		{
			Post["/"] = o => Negotiate
				                 .WithHeader("Location", "/baskets/1")
				                 .WithModel(new Basket {Id = 1, Items = new string[] {}});

			Get["/{basketid}"] = o => Negotiate
				                          .WithModel(new Basket {Id = 1, Items = new string[] {}});
		}
	}
}