using System.Text;

namespace Calliope.Purchase
{
	static internal class ReceiptSender
	{
		public static void SendReceipt(Purchase purchase, Basket basket)
		{
			var body = BuildReceiptBody(purchase, basket);
			var email = new Email
				            {
					            To = purchase.User,
					            From = "sales@calliope.com",
					            Subject = "Thank you for your purchase from Calliope",
					            Body = body
				            };
			EmailSenderWrapper.SendEmail(email);
		}

		private static string BuildReceiptBody(Purchase purchase, Basket basket)
		{
			var stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(@"Dear {0}
Thank you for your purchase from Calliope; here is your receipt.
Items purchased:
", purchase.User);
			foreach (var item in basket.Items)
			{
				stringBuilder.AppendFormat("* {0} '{1}' (¤{2})\n", item.Poet, item.Title, item.Price);
			}
			stringBuilder.AppendFormat("Total: ¤{0}\nYours,\nCalliope", purchase.Total);
			var body = stringBuilder.ToString();
			return body;
		}
	}
}