using System.Collections.Generic;
using System.Linq;
using TestBot.Match;

namespace TestBot.Bowling
{
    public class HardRepository :IRepository
    {
        private readonly MatchContext _context;

        public HardRepository(MatchContext context)
        {
            _context = context;
        }


        public void InsertAnaytics(BallAnalytics ballAnalytics)
        {
            _context.BallByBallAnalytics.Add(ballAnalytics);
        }

        public List<BallAnalytics> getAnalytics()
        {
            return _context.BallByBallAnalytics.ToList();
        }

    }
}