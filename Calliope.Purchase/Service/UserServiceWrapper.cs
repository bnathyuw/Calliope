using Calliope.Purchase.Model;
using Calliope.WebSupport;

namespace Calliope.Purchase.Service
{
	internal static class UserServiceWrapper
	{
		public static void AddToLocker(Item item, int userId)
		{
			var url = "http://localhost/calliope/users/" + userId + "/folio/";
			var folioItem = new FolioItem
				                {
					                Title = item.Title,
					                Poet = item.Poet,
					                FirstLine = item.FirstLine
				                };
			WebRequester.Post(url, folioItem);
		}
	}
}