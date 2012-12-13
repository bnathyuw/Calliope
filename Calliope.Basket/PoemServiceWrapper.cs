using Calliope.WebSupport;

namespace Calliope.Basket
{
	public class PoemServiceWrapper
	{
		public static Poem Get(int id)
		{
			var url = "http://localhost/calliope/poems/" + id + "/";
			return WebRequester.Get<Poem>(url).Body;
		}
	}
}