using System.Collections.Generic;

namespace Calliope.Stub.EmailSender
{
	internal static class ReceiptStore
	{
		private static readonly IList<Emails> Store = new List<Emails>();

		public static void Add(Emails emails)
		{
			Store.Add(emails);
		}

		public static IEnumerable<Emails> GetAll()
		{
			return Store;
		}
	}
}