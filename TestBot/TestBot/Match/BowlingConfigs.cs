using TestBot.Bowling;

namespace TestBot.Match
{
    public class BowlingConfigs
    {
        internal bool isTried;

        public int id { get; set; }

        public BowlerTypes bowlerType { get; set; }

        public BowlingType bowlingType { get; set; }

        public int speed { get; set; }

        public BallPitchZone pitchZone { get; set; }

        public FieldSet fieldSet { get; set; }

    }
}