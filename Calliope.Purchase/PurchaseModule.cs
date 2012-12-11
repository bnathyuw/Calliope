using Nancy;
using Nancy.ModelBinding;
using HttpStatusCode = Nancy.HttpStatusCode;

namespace Calliope.Purchase
{
	public class PurchaseModule:NancyModule
	{
		public PurchaseModule()
		{
			Post["/"] = o =>
				            {
					            var request = this.Bind<Purchase>();
					            var purchase = MakePurchase(request);
					            return Negotiate.WithStatusCode(HttpStatusCode.Created)
					                            .WithModel(purchase);
				            };
		}

		private static Purchase MakePurchase(Purchase purchase)
		{
			var basket = BasketServiceWrapper.Get(purchase.BasketId);
	
			PaymentServiceWrapper.MakePayment(basket.Total, "basket:" + basket.Id, purchase.CardToken);

			purchase.Total = basket.Total;
			purchase.Status = "successful";
			return purchase;
		}
	}
}