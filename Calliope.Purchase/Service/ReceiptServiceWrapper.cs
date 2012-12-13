using Calliope.Purchase.Model;
using Calliope.WebSupport;

namespace Calliope.Purchase.Service
{
	internal static class ReceiptServiceWrapper
	{
		public static void SendReceipt(int basketId, int userId)
		{
			const string url = "http://localhost/calliope/receipts/";
			var receipt = new Receipt {BasketId = basketId, UserId = userId};
			WebRequester.Post(url, receipt);
		}
	}
}