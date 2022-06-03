using TestBot.Match;

namespace TestBot.Controllers
{
    public interface IService
    {
        void getLastBallData(MatchProgressModel matchProgressModel);
    }
}