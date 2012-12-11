namespace Calliope.Purchase
{
	public class Purchase
	{
		public int BasketId { get; set; }
		public int Total { get; set; }
		public string Status { get; set; }
		public string CardToken { get; set; }
	}
}