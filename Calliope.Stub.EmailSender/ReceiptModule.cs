using System.Collections.Generic;
using Nancy;
using Nancy.ModelBinding;

namespace Calliope.Stub.EmailSender
{
	public class ReceiptModule:NancyModule
	{
		public ReceiptModule()
		{
			Post["/receipts/"] = o =>
				                     {
					                     var request = this.Bind<Receipt>();
					                     var receipt = CreateReceipt(request);
					                     return Negotiate.WithStatusCode(HttpStatusCode.Created)
					                                     .WithModel(receipt);
				                     };

			Get["/receipts/"] = o =>
				                    {
					                    var receipts = ReceiptStore.GetAll();
					                    return Negotiate.WithStatusCode(HttpStatusCode.OK)
					                                    .WithModel(receipts);
				                    };
		}

		private Receipt CreateReceipt(Receipt receipt)
		{
			ReceiptStore.Add(receipt);
			return receipt;
		}
	}

	internal static class ReceiptStore
	{
		private static readonly IList<Receipt> Store = new List<Receipt>();

		public static void Add(Receipt receipt)
		{
			Store.Add(receipt);
		}

		public static IEnumerable<Receipt> GetAll()
		{
			return Store;
		}
	}

	public class Receipt
	{
		public string To { get; set; }
		public string From { get; set; }
		public string Subject { get; set; }
		public string Body { get; set; }
	}
}