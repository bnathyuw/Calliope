using Calliope.Purchase.Model;
using Calliope.Purchase.Service;
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
					            var request = this.Bind<Model.Purchase>();
					            var purchase = MakePurchase(request);
					            return Negotiate.WithStatusCode(HttpStatusCode.Created)
					                            .WithModel(purchase);
				            };
		}

		private static Model.Purchase MakePurchase(Model.Purchase purchase)
		{
			var basket = BasketServiceWrapper.Get(purchase.BasketId);
			PaymentServiceWrapper.MakePayment(basket.Total, "basket:" + basket.Id, purchase.CardToken);
			UpdatePurchase(purchase, basket);
			ReceiptServiceWrapper.SendReceipt(basket.Id, purchase.UserId);
			AddItemsToLocker(purchase, basket);
			BasketServiceWrapper.Delete(purchase.BasketId);
			return purchase;
		}

		private static void UpdatePurchase(Model.Purchase purchase, Basket basket)
		{
			purchase.Status = "successful";
			purchase.Total = basket.Total;
		}

		private static void AddItemsToLocker(Model.Purchase purchase, Basket basket)
		{
			foreach (var item in basket.Items)
			{
				UserServiceWrapper.AddToLocker(item, purchase.UserId);
			}
		}
	}
}