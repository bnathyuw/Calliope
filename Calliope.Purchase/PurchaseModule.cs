using Nancy;
using Nancy.ModelBinding;

namespace Calliope.Purchase
{
	public class PurchaseModule:NancyModule
	{
		public PurchaseModule()
		{
			Post["/"] = o =>
				            {
					            var purchase = this.Bind<Purchase>();
					            return Negotiate.WithStatusCode(HttpStatusCode.Created)
					                            .WithModel(purchase);
				            };
		}
	}

	public class Purchase
	{
		public int BasketId { get; set; }
	}
}