using System;
using System.Collections.Generic;
using Nancy;
using Nancy.ModelBinding;

namespace Calliope.Stub.PaymentProvider
{
	public class CardModule:NancyModule
	{
		public CardModule()
		{
			Post["/cards/"] = o =>
				                  {
					                  var request = this.Bind<Card>();
					                  var card = CreateCard(request);
					                  return Negotiate.WithStatusCode(HttpStatusCode.Created)
					                                  .WithModel(card);
				                  };
		}

		private Card CreateCard(Card card)
		{
			card.Token = Guid.NewGuid().ToString();
			CardStore.Add(card);
			return card;
		}
	}

	internal static class CardStore
	{
		private static readonly IList<Card> Store = new List<Card>();

		public static void Add(Card card)
		{
			Store.Add(card);
		}
	}

	public class Card
	{
		public string Number { get; set; }

		public string ExpiryDate { get; set; }

		public string Token { get; set; }
	}
}