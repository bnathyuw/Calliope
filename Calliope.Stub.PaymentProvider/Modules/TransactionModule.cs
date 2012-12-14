using Calliope.Stub.PaymentProvider.Models;
using Calliope.Stub.PaymentProvider.Stores;
using Nancy;
using Nancy.ModelBinding;

namespace Calliope.Stub.PaymentProvider.Modules
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
}