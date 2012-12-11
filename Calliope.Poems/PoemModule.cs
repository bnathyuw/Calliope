using Nancy;

namespace Calliope.Poems
{
	public class PoemModule : NancyModule
	{
		public PoemModule()
		{
			Get["/"] = o =>
				           {
					           var products = PoemStore.All();
					           return Negotiate.WithModel(products)
					                           .WithStatusCode(HttpStatusCode.OK);
				           };

			Get["/{poemid}/"] = o =>
				                   {
					                   var poem = PoemStore.Find((int) o.poemid);
					                   return Negotiate.WithModel(poem)
					                                   .WithStatusCode(HttpStatusCode.OK);
				                   };
		}
	}
}