namespace Calliope.Purchase
{
	static internal class ReceiptSender
	{
		public static void SendReceipt(Purchase purchase)
		{
			var email = new Email
				            {
					            To = purchase.User,
					            From = "sales@calliope.com",
					            Subject = "Thank you for your purchase from Calliope"
				            };
			EmailSenderWrapper.SendEmail(email);
		}
	}
}