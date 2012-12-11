using System.Text;

namespace Calliope.Purchase
{
	static internal class ReceiptSender
	{
		public static void SendReceipt(Purchase purchase, Basket basket, User user)
		{
			var body = BuildReceiptBody(purchase, basket, user);
			var email = new Email
				            {
					            To = user.Email,
					            From = "sales@calliope.com",
					            Subject = "Thank you for your purchase from Calliope",
					            Body = body
				            };
			EmailSenderWrapper.SendEmail(email);
		}

		private static string BuildReceiptBody(Purchase purchase, Basket basket, User user)
		{
			var stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(@"Dear {0}
Thank you for your purchase from Calliope; here is your receipt.
Items purchased:
", user.Email);
			foreach (var item in basket.Items)
			{
				stringBuilder.AppendFormat("* {0} '{1}' (¤{2})\n", item.Poet, item.Title, item.Price);
			}
			stringBuilder.AppendFormat("Total: ¤{0}\nYours,\nCalliope", purchase.Total);
			var body = stringBuilder.ToString();
			return body;
		}
	}

	internal class User
	{
		public int Id { get; set; }
		public string Email { get; set; }
	}
}