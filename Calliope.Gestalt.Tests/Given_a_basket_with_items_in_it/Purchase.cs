namespace Calliope.Gestalt.Tests.Given_a_basket_with_items_in_it
{
	public class Purchase
	{
		public int BasketId { get; set; }

		public int Total { get; set; }

		public string Status { get; set; }

		public string CardToken { get; set; }

		public string User { get; set; }
	}
}