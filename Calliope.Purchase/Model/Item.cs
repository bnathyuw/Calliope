namespace Calliope.Purchase.Model
{
	public class Item
	{
		public int Id { get; set; }
		public int BasketId { get; set; }
		public string Title { get; set; }
		public string Poet { get; set; }
		public string FirstLine { get; set; }
		public int Price { get; set; }
	}
}