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

        public BowlingService(IRepository hardRepository)
        {
            _hardRepository = (HardRepository)hardRepository;
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

            return getRandomBowling();
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

        private BallModel getRandomBowling()
        {
            IDictionary<BowlingConfig, BallModel> allBowlingConfig= new Dictionary<BowlingConfig, BallModel>();
            allBowlingConfig.Add(BowlingConfig.RAF_Inswinger, getBallData(BowlerTypes.RAF, BowlingType.Inswingers, 160, BallPitchZone.zone2));
            allBowlingConfig.Add(BowlingConfig.RAF_Bouncer, getBallData(BowlerTypes.RAF, BowlingType.Bouncer, 140, BallPitchZone.zone1));
            allBowlingConfig.Add(BowlingConfig.RAF_Outswinger, getBallData(BowlerTypes.RAF, BowlingType.Outswinger, 150, BallPitchZone.zone2));
            allBowlingConfig.Add(BowlingConfig.RAS_LegBreak, getBallData(BowlerTypes.RAS, BowlingType.LegBreak, 90, BallPitchZone.zone2));
            allBowlingConfig.Add(BowlingConfig.RAS_Googly, getBallData(BowlerTypes.RAS, BowlingType.Googly, 90, BallPitchZone.zone1));
            allBowlingConfig.Add(BowlingConfig.RAS_OffBreak, getBallData(BowlerTypes.RAS, BowlingType.OffBreak, 150, BallPitchZone.zone2));

            allBowlingConfig.Add(BowlingConfig.LAF_Inswinger, getBallData(BowlerTypes.RAF, BowlingType.Inswingers, 160, BallPitchZone.zone2));
            allBowlingConfig.Add(BowlingConfig.LAF_Bouncer, getBallData(BowlerTypes.RAF, BowlingType.Bouncer, 140, BallPitchZone.zone1));
            allBowlingConfig.Add(BowlingConfig.LAF_Outswinger, getBallData(BowlerTypes.RAF, BowlingType.Outswinger, 150, BallPitchZone.zone2));
            allBowlingConfig.Add(BowlingConfig.LAS_LegBreak, getBallData(BowlerTypes.RAS, BowlingType.LegBreak, 90, BallPitchZone.zone2));
            allBowlingConfig.Add(BowlingConfig.LAS_Googly, getBallData(BowlerTypes.RAS, BowlingType.Googly, 90, BallPitchZone.zone1));
            allBowlingConfig.Add(BowlingConfig.LAS_OffBreak, getBallData(BowlerTypes.RAS, BowlingType.OffBreak, 150, BallPitchZone.zone2));

            return allBowlingConfig[getRandom()];

        }

        private  BowlingConfig getRandom()
        {
            Array values = Enum.GetValues(typeof(BowlingConfig));
            Random random = new Random();
            return (BowlingConfig)values.GetValue(random.Next(values.Length));
        }

        private BallModel getBallData(BowlerTypes bowlerType, BowlingType bowlingType,int speed, BallPitchZone pitchZone)
        {
            return new BallModel
            {
                bowlerType = bowlerType,
                bowingType = bowlingType,
                bowlerName = "Sachin",              
                speed = speed,
                zone = pitchZone
            };
        }

        public void getLastBallData(MatchProgressModel matchProgressModel)
        {
            _hardRepository.UpdateAnalytics(matchProgressModel);
        }
    }
}
