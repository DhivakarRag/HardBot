using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestBot.Match
{
    public class MatchContext:DbContext
    {
        public MatchContext(DbContextOptions options):base(options)
        {

        }

        public DbSet<BallAnalytics> BallByBallAnalytics { get; set; }

        public DbSet<BowlingConfigs> BowlingConfigs { get; set; }


    }
}
