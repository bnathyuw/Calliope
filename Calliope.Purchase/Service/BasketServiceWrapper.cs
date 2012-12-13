﻿using Calliope.Purchase.Model;
using Calliope.WebSupport;

namespace Calliope.Purchase.Service
{
	internal static class BasketServiceWrapper
	{
		public static Basket Get(int id)
		{
			var url = "http://localhost/calliope/baskets/" + id + "/";
			return WebRequester.Get<Basket>(url);
		}

		public static void Delete(int id)
		{
			var url = "http://localhost/calliope/baskets/" + id + "/";
			WebRequester.Delete(url);
		}
	}
}