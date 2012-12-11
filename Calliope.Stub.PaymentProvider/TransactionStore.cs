﻿using System.Collections.Generic;
using System.Linq;

namespace Calliope.Stub.PaymentProvider
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