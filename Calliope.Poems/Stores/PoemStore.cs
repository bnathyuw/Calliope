using System.Collections.Generic;
using System.Linq;
using Calliope.Poems.Models;

namespace Calliope.Poems.Stores
{
	public static class PoemStore
	{
		private static readonly IList<Poem> Store = new List<Poem>
			                                            {
				                                            new Poem {Id = 1, FirstLine = "Ille mi par esse deo uidetur", Poet = "Gaius Valerius Catullus", Price = 5, Title = "51"},
				                                            new Poem {Id = 2, FirstLine = "Φαίνεταί μοι κῆνος ἴσος θέοισιν", Poet = "Sappho", Price = 5, Title = "31"},
				                                            new Poem {Id = 3, FirstLine = "Shall I compare thee to a summer’s day?", Poet = "William Shakespeare", Price = 5, Title = "Sonnet 18"},
				                                            new Poem {Id = 4, FirstLine = "They fuck you up, your mum and dad.", Poet = "Philip Larkin", Price = 6, Title = "This be the verse"},
				                                            new Poem {Id = 5, FirstLine = "Mieleni minun tekevi, aivoni ajattelevi", Poet = "Elias Lönnrot", Price = 30, Title = "Kalevala"},
				                                            new Poem {Id = 6, FirstLine = "Hwæt we Gar-Dena in gear-dagum", Poet = null, Price = 30, Title = "Beowulf"},
				                                            new Poem {Id = 7, FirstLine = "Siþen þe sege and þe assaut watz sesed at Troye", Poet = null, Price = 30, Title = "Sir Gawayne and þe Grene Knight"},
				                                            new Poem {Id = 8, FirstLine = "Arma uirumque cano Troiae qui primus ab oris", Poet = "Publius Vergilius Maro", Price = 30, Title = "Aeneis"},
				                                            new Poem {Id = 9, FirstLine = "In nova fert animus mutatas dicere formas", Poet = "Publius Ovidius Naso", Price = 30, Title = "Metamorphoses"},
			                                            };

		public static IEnumerable<Poem> All()
		{
			return Store;
		}

		public static Poem Find(int id)
		{
			return Store.SingleOrDefault(p => p.Id == id);
		}
	}
}