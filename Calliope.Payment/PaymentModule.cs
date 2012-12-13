using Calliope.Payment.Service;
using Nancy;
using Nancy.ModelBinding;

namespace Calliope.Payment
{
	public class PaymentModule:NancyModule
	{
		public PaymentModule()
		{
			Post["/"] = o =>
				            {
					            var request = this.Bind<Model.Payment>();
					            PaymentProviderWrapper.MakePayment(request);
					            return Negotiate.WithStatusCode(HttpStatusCode.OK)
					                            .WithModel(request);
				            };
		}
	}
}