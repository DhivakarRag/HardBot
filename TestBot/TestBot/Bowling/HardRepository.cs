using System;
using System.Collections.Generic;
using System.Linq;
using TestBot.Batting;
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

        public List<BowlingConfigs> getAnalytics()
        {
            return _context.BowlingConfigs.ToList();
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

            if (_context.BattingConfigs.Any())
            {
                var lastBallData = _context.BattingConfigs.OrderByDescending(x => x.id).First();
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
            return _context.BowlingConfigs.Where(x => x.gotWicketOnLastBall).Any();
        }

        public BowlingConfigs getWicketBall()
        {
            return _context.BowlingConfigs.Where(x => x.gotWicketOnLastBall).First();
        }

        public bool hasDotBall()
        {
            return _context.BowlingConfigs.Where(x => x.runsOnLastBall == 0).Any();
        }

        public BowlingConfigs getDotBall()
        {
            return _context.BowlingConfigs.Where(x=>x.runsOnLastBall == 0).First();
        }

        public List<BowlingConfigs> GetBowlingConfigs()
        {
            return _context.BowlingConfigs.Where(x => !x.isTried).ToList();
        }

        public bool hasTriedEnough()
        {
            return _context.BowlingConfigs.ToList().Where(x => x.isTried).Count() > 25;
        }

        internal List<BattingRecords> GetBattingConfigs()
        {
            return _context.BattingConfigs.ToList();
        }

        internal void UpdateBowlingConfig(int currentBowlingConfigId, bool gotWicket, int runsOnLastBall)
        {
            var currentBall = _context.BowlingConfigs.Where(x => x.id == currentBowlingConfigId).First();
            currentBall.isTried = true;
            currentBall.gotWicketOnLastBall = gotWicket;
            currentBall.runsOnLastBall = runsOnLastBall;
            currentBall.timesBowled++;

            _context.SaveChanges();
        }

        internal void InsertBattingAnaytics(Shots shot, int batSpeed, int ballSpeed)
        {
            _context.BattingConfigs.Add(new BattingRecords
            {
                id = _context.BattingConfigs.Count() + 1,
                ballSpeed = ballSpeed,
                batSpeed = batSpeed,
                shotPlayed = shot.ToString()
            }) ;
            _context.SaveChanges();
        }
    }
}