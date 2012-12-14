using System.Collections.Generic;
using Calliope.Stub.EmailSender.Models;

namespace Calliope.Stub.EmailSender.Stores
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