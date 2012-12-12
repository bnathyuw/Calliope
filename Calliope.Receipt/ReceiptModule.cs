using Nancy;
using Nancy.ModelBinding;

namespace Calliope.Receipt
{
	public class ReceiptModule:NancyModule
	{
		public ReceiptModule()
		{
			Post["/"] = o =>
				            {
					            var request = this.Bind<Receipt>();
					            ReceiptSender.SendReceipt(request.UserId, request.BasketId);
					            return Negotiate.WithStatusCode(HttpStatusCode.Created);
				            };
		}
	}

	public class Receipt
	{
		public int UserId { get; set; }
		public int BasketId { get; set; }
	}
}