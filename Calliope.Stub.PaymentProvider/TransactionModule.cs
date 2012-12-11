using System.Collections.Generic;
using System.Linq;
using Nancy;
using Nancy.ModelBinding;

namespace Calliope.Stub.PaymentProvider
{
	public class TransactionModule:NancyModule
	{
		public TransactionModule()
		{
			Get["/cards/{cardtoken}/transactions/"] = o =>
				                                          {
					                                          var cardTransactions = TransactionStore.GetAll((string) o.cardtoken);
					                                          return Negotiate.WithModel(cardTransactions);
				                                          };

			Post["/cards/{cardtoken}/transactions/"] = o =>
				                                           {
					                                           var cardTransaction = this.Bind<CardTransaction>();
					                                           cardTransaction.CardToken = (string) o.cardtoken;
					                                           TransactionStore.Add(cardTransaction);
					                                           return Negotiate.WithStatusCode(HttpStatusCode.Created)
					                                                           .WithModel(cardTransaction);
				                                           };
		} 
	}

	public static class TransactionStore
	{
		private static readonly IList<CardTransaction> Store = new List<CardTransaction>();

		public static IEnumerable<CardTransaction> GetAll(string cardToken)
		{
			return Store.Where(ct => ct.CardToken == cardToken);
		}

		public static void Add(CardTransaction cardTransaction)
		{
			Store.Add(cardTransaction);
		}
	}

	public class CardTransaction
	{
		public string CardToken { get; set; }
		public int Amount { get; set; }
		public string Reference { get; set; }
	}

}