﻿using Nancy;

namespace Calliope.Basket
{
	public class BasketModule:NancyModule
	{
		public BasketModule()
		{
			Post["/"] = o => Negotiate
				                 .WithHeader("Location", "/1")
				                 .WithModel(new Basket {Id = 1, Items = new string[] {}});
		}
	}
	public class Basket
	{
		public int Id { get; set; }
		public string[] Items { get; set; }
	}
}