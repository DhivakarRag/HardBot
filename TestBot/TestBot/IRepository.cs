using System.Collections.Generic;
using TestBot.Match;

namespace TestBot
{
    public interface IRepository
    {
        public void InsertAnaytics(BallAnalytics ballAnalytics);


        public List<BallAnalytics> getAnalytics();
        
    }
}