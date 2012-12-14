using Calliope.Purchase.Models;
using Calliope.WebSupport;

namespace Calliope.Purchase.Services
{
	internal static class PaymentServiceWrapper
	{
		public static void MakePayment(int amount, string reference, string cardToken)
		{
			const string url = "http://localhost/calliope/payments/";
			var payment = new Payment {Amount = amount, Reference = reference, CardToken = cardToken};
			WebRequester.Post(url, payment);
		}
	}
}