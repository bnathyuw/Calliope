using Calliope.Receipt.Models;
using Calliope.WebSupport;

namespace Calliope.Receipt.Services
{
	internal static class EmailSenderWrapper
	{
		public static void SendEmail(Email email)
		{
			const string url = "http://localhost/calliope/stub/email-sender/emails/";
			WebRequester.Post(url, email);
		}
	}
}