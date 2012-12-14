namespace Calliope.Stub.PaymentProvider.Models
{
	public class CardTransaction
	{
		public string CardToken { get; set; }
		public int Amount { get; set; }
		public string Reference { get; set; }
	}
}