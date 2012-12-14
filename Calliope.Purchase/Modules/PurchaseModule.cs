using Calliope.Purchase.Models;
using Calliope.Purchase.Services;
using Nancy;
using Nancy.ModelBinding;

namespace Calliope.Purchase.Modules
{
	public class PurchaseModule:NancyModule
	{
		public PurchaseModule()
		{
			Post["/"] = o =>
				            {
					            var request = this.Bind<Models.Purchase>();
					            var purchase = MakePurchase(request);
					            return Negotiate.WithStatusCode(HttpStatusCode.Created)
					                            .WithModel(purchase);
				            };
		}

		private static Models.Purchase MakePurchase(Models.Purchase purchase)
		{
			var basket = BasketServiceWrapper.Get(purchase.BasketId);
			PaymentServiceWrapper.MakePayment(basket.Total, "basket:" + basket.Id, purchase.CardToken);
			UpdatePurchase(purchase, basket);
			ReceiptServiceWrapper.SendReceipt(basket.Id, purchase.UserId);
			AddItemsToLocker(purchase, basket);
			BasketServiceWrapper.Delete(purchase.BasketId);
			return purchase;
		}

		private static void UpdatePurchase(Models.Purchase purchase, Basket basket)
		{
			purchase.Status = "successful";
			purchase.Total = basket.Total;
		}

		private static void AddItemsToLocker(Models.Purchase purchase, Basket basket)
		{
			foreach (var item in basket.Items)
			{
				UserServiceWrapper.AddToLocker(item, purchase.UserId);
			}
		}
	}
}