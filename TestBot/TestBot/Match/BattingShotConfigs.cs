using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestBot.Batting;
using TestBot.Bowling;

namespace TestBot.Match
{
    public class BattingShotConfigs
    {
        int id { get; set; }
        BowlingType bowlingTypes { get; set; }
        BallPitchZone zone { get; set; }      
        Shots shots { get; set; }
        bool isShotValid { get; set; }
        int runScored { get; set; }
    }
}
