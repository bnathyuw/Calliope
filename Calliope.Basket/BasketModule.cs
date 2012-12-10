using Nancy;

namespace Calliope.Basket
{
	public class BasketModule:NancyModule
	{
		public BasketModule()
		{
			Post["/"] = o => Negotiate.WithModel(new Basket {Id = 1, Items = new string[] {}});
		}
	}
	public class Basket
	{
		public int Id { get; set; }
		public string[] Items { get; set; }
	}
}