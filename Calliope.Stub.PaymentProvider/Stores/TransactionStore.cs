using System.Collections.Generic;
using System.Linq;
using Calliope.Stub.PaymentProvider.Models;

namespace Calliope.Stub.PaymentProvider.Stores
{
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
}