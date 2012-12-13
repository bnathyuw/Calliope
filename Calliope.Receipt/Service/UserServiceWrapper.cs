using Calliope.Receipt.Model;
using Calliope.WebSupport;

namespace Calliope.Receipt.Service
{
	internal static class UserServiceWrapper
	{
		public static User GetUser(int id)
		{
			var url = "http://localhost/calliope/users/" + id + "/";
			return WebRequester.Get<User>(url);
		}
	}
}