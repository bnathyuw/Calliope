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
			purchase.Status = "successful";
			purchase.Total = basket.Total;

			User user = UserServiceWrapper.GetUser(purchase.UserId);
			ReceiptSender.SendReceipt(purchase, basket, user);
			foreach (var item in basket.Items)
			{
				UserServiceWrapper.AddToLocker(item, purchase.UserId);
			}

			return purchase;
		}
	}
}