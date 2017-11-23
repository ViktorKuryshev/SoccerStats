﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerStats
{
    public class GameResult
    {
        public DateTime GameDate { get; set; }
		public string TeamName { get; set; }
		public HomeOrAway HomeOrAway { get; set; }
    }

	public enum HomeOrAway //Can change type for example by doing so : byte
	{
		Home,
		Away
	}
}
