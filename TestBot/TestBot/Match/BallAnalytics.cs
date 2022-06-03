using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestBot.Batting;
using TestBot.Bowling;

namespace TestBot.Match
{
    public class BallAnalytics
    {
        public int id { get; set; }

        public BowlerTypes bowlerType { get; set; }

        public BowlingType bowlingType { get; set; }

        public int speed { get; set; }

        public BallPitchZone pitchZone { get; set; }

        public int runScored { get; set; }

        public bool isWicket { get; set; }

        public FieldSet fieldSet { get; set; }

        public ComputedRating rating { get; set; }
    }
}
