using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestBot.Controllers;
using TestBot.Match;

namespace TestBot.Bowling
{
    public class BowlingService: IService
    {
        private HardRepository _hardRepository;

        private static List<BowlingConfigs> _allBalls;

        public BowlingService(IRepository hardRepository)
        {
            _hardRepository = (HardRepository)hardRepository;
            _allBalls = hardRepository.GetBowlingConfigs();
        }

        public (BallModel,int) getBowlingData()
        {
            var ballToBowl = decideBallToBowl().Item1;

            _hardRepository.InsertAnaytics(new BallAnalytics
            {
                id = _hardRepository.getCountOfBalls()+1,
                bowlerType = ballToBowl.bowlerType,
                bowlingType=ballToBowl.bowingType,
                speed=ballToBowl.speed,
                pitchZone = ballToBowl.zone,
                runScored = null             
                
            }) ;

            return (ballToBowl,decideBallToBowl().Item2);
        }

        private (BallModel,int) decideBallToBowl()
        {
            if(_hardRepository.hasWicketBall())
            {
                return (getBallModel(_hardRepository.getWicketBall()),1);
            }
            else if (_hardRepository.hasDotBall())
            {
                return (getBallModel(_hardRepository.getDotBall()),1);
            }
            else if(_hardRepository.hasTriedEnough())
            {
                return (getBallModel(_hardRepository.getAnalytics().OrderBy(x=>x.runScored).First()),1);
            }

            return getChronologicalBowling();
        }

        private BallModel getBallModel(BallAnalytics ballAnalytics)
        {
            return new BallModel
            {
                bowlerType = ballAnalytics.bowlerType,
                bowingType = ballAnalytics.bowlingType,
                bowlerName = "Sachin",
                speed = ballAnalytics.speed,
                zone = ballAnalytics.pitchZone
            };
        }

        private (BallModel,int) getChronologicalBowling()

        { 
            return (_allBalls.Select(x=> new BallModel
            {
                bowingType = x.bowlingType,
                bowlerType =x.bowlerType,
                speed = x.speed,
                zone = x.pitchZone,
                bowlerName="Sachin"
            }).First(), _allBalls.First().id);

}

        public void postLastBallData(MatchProgressModel matchProgressModel,int currentBowlingConfigId)
        {
            _hardRepository.UpdateAnalytics(matchProgressModel);
            _hardRepository.UpdateBowlingConfig(currentBowlingConfigId);
        }
    }
}
