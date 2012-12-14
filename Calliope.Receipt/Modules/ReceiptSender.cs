using System.Text;
using Calliope.Receipt.Models;
using Calliope.Receipt.Services;

namespace Calliope.Receipt.Modules
{
	static internal class ReceiptSender
	{
		public static void SendReceipt(int userId, int basketId)
		{
			var basket = BasketServiceWrapper.Get(basketId);
			var user = UserServiceWrapper.GetUser(userId);

			var body = BuildReceiptBody(basket, user, basket.Total);
			var email = new Email
				            {
					            To = user.Email,
					            From = "sales@calliope.com",
					            Subject = "Thank you for your purchase from Calliope",
					            Body = body
				            };
			EmailSenderWrapper.SendEmail(email);
		}

		private static string BuildReceiptBody(Basket basket, User user, int total)
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
			stringBuilder.AppendFormat("Total: ¤{0}\nYours,\nCalliope", total);
			var body = stringBuilder.ToString();
			return body;
		}
	}
}