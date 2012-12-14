using Calliope.Purchase.Models;
using Calliope.WebSupport;

namespace Calliope.Purchase.Services
{
	internal static class UserServiceWrapper
	{
		public static void AddToLocker(Item item, int userId)
		{
			var url = "http://localhost/calliope/users/" + userId + "/folio/";
			var folioItem = new FolioItem
				                {
									PoemId = item.Id,
					                Title = item.Title,
					                Poet = item.Poet,
					                FirstLine = item.FirstLine
				                };
			WebRequester.Post(url, folioItem);
		}
	}
}