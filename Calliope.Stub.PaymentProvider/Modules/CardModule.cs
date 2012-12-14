using System;
using Calliope.Stub.PaymentProvider.Models;
using Calliope.Stub.PaymentProvider.Stores;
using Nancy;
using Nancy.ModelBinding;

namespace Calliope.Stub.PaymentProvider.Modules
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

		private static Card CreateCard(Card card)
		{
			card.Token = Guid.NewGuid().ToString();
			CardStore.Add(card);
			return card;
		}
	}
}