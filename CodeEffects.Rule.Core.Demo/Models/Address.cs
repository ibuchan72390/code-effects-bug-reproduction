﻿using CodeEffects.Rule.Attributes;

namespace CodeEffects.Rule.Core.Demo.Models
{
	public class Address
	{
		[Parent("Home", "Home Street", "Current home street")]
		[Parent("Work", "Work Street", "Current work street")]
		[Field(Max = 50)]
		public string Street { get; set; }

		[Parent("Home", "Home City", "Current home city")]
		[Parent("Work", "Work City", "Current work city")]
		[Field(Max = 30)]
		public string City { get; set; }

		[Parent("Home", "Home State", "Current home state")]
		[Parent("Work", "Work State", "Current work state")]
		public State State { get; set; }

		[Parent("Home", "Home Zip", "Current home zip code")]
		[Parent("Work", "Work Zip", "Current work zip code")]
		[Field(Max = 5)]
		public string Zip { get; set; }

		public Address() { this.State = State.Unknown; }
	}
}