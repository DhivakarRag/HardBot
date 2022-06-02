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
        int id { get; set; }

        BowlerTypes bowlerType { get; set; }

        BowlingType bowlingType { get; set; }

        Shots shotPlayed { get; set; }

        int runScored { get; set; }

        bool isWicket { get; set; }

        FieldSet fieldSet { get; set; }

        ComputedRating rating { get; set; }
    }
}
