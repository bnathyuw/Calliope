using Calliope.Payment.Model;
using Calliope.WebSupport;

namespace Calliope.Payment.Service
{
	internal static class PaymentProviderWrapper
	{
		public static void MakePayment(Model.Payment payment)
		{
			var url = "http://localhost/calliope/stub/payment-provider/cards/" + payment.CardToken + "/transactions/";
			var cardTransaction = new CardTransaction {Amount = payment.Amount, Reference = payment.Reference};
			WebRequester.Post(url, cardTransaction);
		}
	}
}