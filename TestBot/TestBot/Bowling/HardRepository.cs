using System;
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
            _context.SaveChanges();
        }

        public List<BallAnalytics> getAnalytics()
        {
            return _context.BallByBallAnalytics.ToList();
        }

        public void UpdateAnalytics(MatchProgressModel matchProgress)
        {
            if(_context.BallByBallAnalytics.Any())
            {
                var lastBallData = _context.BallByBallAnalytics.OrderByDescending(x => x.id).First();
                lastBallData.isWicket = matchProgress.iswicketlost;
                lastBallData.runScored = matchProgress.runonlastball;
                _context.SaveChanges();
            }          

        }

        public int getCountOfBalls()
        {
            return _context.BallByBallAnalytics.Count();
        }
    }
}