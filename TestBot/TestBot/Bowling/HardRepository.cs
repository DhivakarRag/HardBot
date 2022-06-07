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

        public bool hasWicketBall()
        {
            return _context.BallByBallAnalytics.Where(x => x.isWicket).Any();
        }

        public BallAnalytics getWicketBall()
        {
            return _context.BallByBallAnalytics.Where(x => x.isWicket).First();
        }

        public bool hasDotBall()
        {
            return _context.BallByBallAnalytics.Where(x => x.runScored == 0).Any();
        }

        public BallAnalytics getDotBall()
        {
            return _context.BallByBallAnalytics.Where(x=>x.runScored == 0).First();
        }

        public List<BowlingConfigs> GetBowlingConfigs()
        {
            return _context.BowlingConfigs.Where(x => !x.isTried).ToList();
        }

        public bool hasTriedEnough()
        {
            return _context.BowlingConfigs.ToList().Where(x => x.isTried).Count() > 15;
        }

        internal void UpdateBowlingConfig(int currentBowlingConfigId)
        {
            var currentBall = _context.BowlingConfigs.Where(x => x.id == currentBowlingConfigId).First();
            currentBall.isTried = true;
            _context.SaveChanges();
        }
    }
}