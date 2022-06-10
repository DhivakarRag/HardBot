namespace TestBot.Match
{
    public class BattingRecords
    {
        public int id{ get; set; }
        public int ballSpeed{ get; set; }
        public int batSpeed{ get; set; }
        public int runScored{ get; set; }
        public bool isWicket{ get; set; }
        internal string shotPlayed{ get; set; }
    }
}