using TestBot.Batting;
using TestBot.Match;

namespace TestBot.Controllers
{
    public interface IService
    {
        void postLastBallData(MatchProgressModel matchProgressModel,int currentBowlingConfigId, BatsmanModel batsmanModel);
    }
}