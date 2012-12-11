using System.Collections.Generic;

namespace Calliope.Stub.PaymentProvider
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