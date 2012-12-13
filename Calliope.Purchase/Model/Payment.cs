namespace Calliope.Purchase.Model
{
	internal class Payment
	{
		public int Amount { get; set; }

		public string Reference { get; set; }

		public string CardToken { get; set; }
	}
}