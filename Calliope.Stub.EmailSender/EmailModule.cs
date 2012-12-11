using Nancy;
using Nancy.ModelBinding;

namespace Calliope.Stub.EmailSender
{
	public class EmailModule:NancyModule
	{
		public EmailModule()
		{
			Post["/emails/"] = o =>
				                     {
					                     var request = this.Bind<Emails>();
					                     var receipt = CreateEmail(request);
					                     return Negotiate.WithStatusCode(HttpStatusCode.Created)
					                                     .WithModel(receipt);
				                     };

			Get["/emails/"] = o =>
				                    {
					                    var receipts = ReceiptStore.GetAll();
					                    return Negotiate.WithStatusCode(HttpStatusCode.OK)
					                                    .WithModel(receipts);
				                    };
		}

		private Emails CreateEmail(Emails emails)
		{
			ReceiptStore.Add(emails);
			return emails;
		}
	}
}