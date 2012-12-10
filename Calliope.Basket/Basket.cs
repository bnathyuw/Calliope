using System.Collections.Generic;

namespace Calliope.Basket
{
	public class Basket
	{
		public int Id { get; set; }
		public IEnumerable<Item> Items { get; set; }
	}
}