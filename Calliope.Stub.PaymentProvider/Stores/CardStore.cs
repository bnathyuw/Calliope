using System.Collections.Generic;
using Calliope.Stub.PaymentProvider.Models;

namespace Calliope.Stub.PaymentProvider.Stores
{
	internal static class CardStore
	{
		private static readonly IList<Card> Store = new List<Card>();

		public static void Add(Card card)
		{
			Store.Add(card);
		}
	}
}