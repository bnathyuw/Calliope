using System.Collections.Generic;

namespace Calliope.Purchase.Model
{
	public class Basket
	{
		public int Id { get; set; }
		public IEnumerable<Item> Items { get; set; }
		public int Total { get; set; }
	}
}