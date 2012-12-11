namespace Calliope.Purchase
{
	static internal class ReceiptSender
	{
		public static void SendReceipt(Purchase purchase)
		{
			EmailSenderWrapper.SendEmail(purchase.User, "sales@calliope.com");
		}
	}
}