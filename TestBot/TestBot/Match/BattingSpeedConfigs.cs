using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestBot.Batting;
using TestBot.Bowling;

namespace TestBot.Match
{
    public class BattingSpeedConfigs
    {
        int id { get; set; }
        BowlingType bowlingTypes { get; set; }
        BallPitchZone zone { get; set; }      
        int ballSpeed { get; set; }
        int batSpeed { get; set; }
        int runScored { get; set; }
        bool isMissed { get; set; }
        
    }
}
