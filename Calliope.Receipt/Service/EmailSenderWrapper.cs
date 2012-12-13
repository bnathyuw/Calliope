using Calliope.Receipt.Model;
using Calliope.WebSupport;

namespace Calliope.Receipt.Service
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