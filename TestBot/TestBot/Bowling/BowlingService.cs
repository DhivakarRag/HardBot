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

        public BallModel getBowlingData()
        {
            var ballToBowl = decideBallToBowl();

            _hardRepository.InsertAnaytics(new BallAnalytics
            {
                id = _hardRepository.getCountOfBalls()+1,
                bowlerType = ballToBowl.bowlerType,
                bowlingType=ballToBowl.bowingType,
                speed=ballToBowl.speed,
                pitchZone = ballToBowl.zone,
                runScored = null             
                
            }) ;

            return ballToBowl;
        }

        private BallModel decideBallToBowl()
        {
            if(_hardRepository.hasWicketBall())
            {
                return getBallModel(_hardRepository.getWicketBall());
            }
            else if (_hardRepository.hasDotBall())
            {
                return getBallModel(_hardRepository.getDotBall());
            }
            else if(_hardRepository.hasTriedEnough())
            {
                return getBallModel(_hardRepository.getAnalytics().OrderBy(x=>x.runScored).First());
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

        private BallModel getChronologicalBowling()
        {
            return _allBalls.Select(x=> new BallModel
            {
                bowingType = x.bowlingType,
                bowlerType =x.bowlerType,
                speed = x.speed,
                zone = x.pitchZone,
                bowlerName="Sachin"
            }).First();

        }

        public void postLastBallData(MatchProgressModel matchProgressModel)
        {
            _hardRepository.UpdateAnalytics(matchProgressModel);
        }
    }
}
