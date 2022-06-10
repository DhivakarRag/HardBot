using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestBot.Batting;
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
                pitchZone = ballToBowl.zone
                
            }) ;

            return (ballToBowl,decideBallToBowl().Item2);
        }

        private (BallModel,int) decideBallToBowl()
        {

            if (_hardRepository.hasWicketBall())
            {
                return (getBallModel(_hardRepository.getWicketBall()), 1);
            }
            else if (_hardRepository.hasDotBall())
            {
                return (getBallModel(_hardRepository.getDotBall()), 1);
            }
            else if (_hardRepository.hasTriedEnough())
            {
                return (getBallModel(_hardRepository.getAnalytics().OrderBy(x => x.runsOnLastBall).First()), 1);
            }

            return getRandomBowling();
        }

        private BallModel getBallModel(BowlingConfigs ballAnalytics)
        {
            return new BallModel
            {
                bowlerType = ballAnalytics.bowlerType,
                bowingType = ballAnalytics.bowlingType,
                bowlerName = getPlayerName(),
                speed = ballAnalytics.speed,
                zone = ballAnalytics.pitchZone
            };
        }

        internal int getIsTriedCount()
        {
            return 45 - _allBalls.Count();

        }

        internal List<BattingRecords> getBattingConfigs()
        {
            return _hardRepository.GetBattingConfigs();
        }

        public string getPlayerName()
        {
            string[] playerList =
            {
                "Sir Whiskey",
                "Sir Absolute",
                "Sir Jameson",
                "Sir Jack Daniels",
                "Sir Black Dog",
                "Sir Johnie Walker",
                "Sir Balentine",
                "Sir Chivas Regal",
                "Sir Jagermeister",
                "Sir Jim beam",
                "Sir Old Monk"
            };

            var random = new Random();

            var index = random.Next(playerList.Count());

           return playerList[index];

        }

        internal void insertBattingConfig(Shots shot, int batSpeed, int ballSpeed)
        {
            _hardRepository.InsertBattingAnaytics(shot, batSpeed, ballSpeed);
        }

        private (BallModel,int) getRandomBowling()
        {
            var random = new Random();

            var index = random.Next(_allBalls.Count());

            var ballToBowl = _allBalls[index];

            return (new BallModel
            {
                bowingType = ballToBowl.bowlingType,
                bowlerType = ballToBowl.bowlerType,
                speed = ballToBowl.speed,
                zone = ballToBowl.pitchZone,
                bowlerName = getPlayerName()
            }, ballToBowl.id);

}

        public void postLastBallData(MatchProgressModel matchProgressModel,int currentBowlingConfigId)
        {
            _hardRepository.UpdateAnalytics(matchProgressModel);
            _hardRepository.UpdateBowlingConfig(currentBowlingConfigId,matchProgressModel.iswicketlost,matchProgressModel.runonlastball);
        }

        public Shots getShot(BallModel ballingData)
        {
            Shots selectShort;
            var random = new Random();

            if (ballingData.bowingType == BowlingType.Bouncer)
            {
                if (ballingData.zone == BallPitchZone.zone1)
                {
                    List<Shots> shortsCanBePlayed = new List<Shots> { Shots.pull, Shots.Uppercut, Shots.hook };

                    selectShort = shortsCanBePlayed[random.Next(shortsCanBePlayed.Count)];
                }
                else if (ballingData.zone == BallPitchZone.zone2 && ballingData.speed < 100)
                    selectShort = Shots.Uppercut;
                else if (ballingData.zone == BallPitchZone.zone2 && ballingData.speed > 100)
                    selectShort = Shots.hook;
                else selectShort = Shots.Uppercut;
            }
            else if (ballingData.bowingType == BowlingType.Outswinger)
            {
                if (ballingData.zone == BallPitchZone.zone1)
                {
                    List<Shots> shortsCanBePlayed = new List<Shots> { Shots.Cut, Shots.latecut };

                    selectShort = shortsCanBePlayed[random.Next(shortsCanBePlayed.Count)];
                }
                else
                {
                    List<Shots> shortsCanBePlayed = new List<Shots> { Shots.Offdrive, Shots.Coverdrive };
                    selectShort = shortsCanBePlayed[random.Next(shortsCanBePlayed.Count)];
                }
            }
            else if (ballingData.bowingType == BowlingType.Inswingers)
            {
                if (ballingData.zone == BallPitchZone.zone1)
                {
                    List<Shots> shortsCanBePlayed = new() { Shots.Straightdrive, Shots.Ondrive, Shots.pull };
                    selectShort = shortsCanBePlayed[random.Next(shortsCanBePlayed.Count)];
                }
                else if (ballingData.zone == BallPitchZone.zone2 && ballingData.speed < 100)
                    selectShort = Shots.Straightdrive;
                else if (ballingData.zone == BallPitchZone.zone2 && ballingData.speed > 100)
                    selectShort = Shots.Ondrive;
                else selectShort = Shots.Straightdrive;
            }
            else if (ballingData.bowingType == BowlingType.LegBreak)
            {

                if (ballingData.zone == BallPitchZone.zone1)
                {
                    List<Shots> shortsCanBePlayed = new List<Shots> { Shots.latecut, Shots.Cut, Shots.hook, Shots.hook };
                    selectShort = shortsCanBePlayed[random.Next(shortsCanBePlayed.Count)];
                }
                else if (ballingData.zone == BallPitchZone.zone2)
                {
                    List<Shots> shortsCanBePlayed = new List<Shots> { Shots.hook, Shots.Coverdrive, Shots.latecut, Shots.Cut };
                    selectShort = shortsCanBePlayed[random.Next(shortsCanBePlayed.Count)];
                }
                else selectShort = Shots.hook;
            }
            else if (ballingData.bowingType == BowlingType.OffBreak)
            {
                if (ballingData.zone == BallPitchZone.zone1)
                {
                    List<Shots> shortsCanBePlayed = new List<Shots> { Shots.latecut, Shots.Straightdrive, Shots.Ondrive };
                    selectShort = shortsCanBePlayed[random.Next(shortsCanBePlayed.Count)];
                }
                else if (ballingData.zone == BallPitchZone.zone2)
                {
                    List<Shots> shortsCanBePlayed = new List<Shots> { Shots.Offdrive, Shots.Coverdrive, Shots.Straightdrive, Shots.squarecut };
                    selectShort = shortsCanBePlayed[random.Next(shortsCanBePlayed.Count)];
                }
                else selectShort = Shots.Offdrive;
            }
            else if (ballingData.bowingType == BowlingType.Googly)
            {
                if (ballingData.zone == BallPitchZone.zone1)
                {

                    List<Shots> shortsCanBePlayed = new List<Shots> { Shots.Straightdrive, Shots.Ondrive, Shots.pull };
                    selectShort = shortsCanBePlayed[random.Next(shortsCanBePlayed.Count)];
                }
                else if (ballingData.zone == BallPitchZone.zone2)
                {
                    List<Shots> shortsCanBePlayed = new List<Shots> { Shots.Straightdrive, Shots.Ondrive, Shots.Sweep };
                    selectShort = shortsCanBePlayed[random.Next(shortsCanBePlayed.Count)];
                }
                else selectShort = Shots.Straightdrive;
            }
            else
            {
                List<Shots> shortsCanBePlayed = new List<Shots> { Shots.Straightdrive, Shots.latecut, Shots.Defensiveshot };
                selectShort = shortsCanBePlayed[random.Next(shortsCanBePlayed.Count)];
            }

            return selectShort;

        }

        public int getBatSpeed(Shots shot, BallModel nextball)
        {
            int angle = 0;

            var random = new Random();

            switch (shot)
            {
                case Shots.Straightdrive:
                    angle = (int)Math.Floor(random.NextDouble() * (-30 - 30 + 1) + 30);
                    break;
                case Shots.Ondrive:
                    angle = (int)Math.Floor(random.NextDouble() * (60 - 30 + 1) + 30);
                    break;
                case Shots.pull:
                    angle = (int)Math.Floor(random.NextDouble() * (90 - 60 + 1) + 60);
                    break;
                case Shots.hook:
                    angle = (int)Math.Floor(random.NextDouble() * (120 - 90 + 1) + 90);
                    break;
                case Shots.Sweep:
                    angle = (int)Math.Floor(random.NextDouble() * (150 - 120 + 1) + 120);
                    break;
                case Shots.latecut:
                    angle = (int)Math.Floor(random.NextDouble() * (210 - 150 + 1) + 150);
                    break;
                case Shots.squarecut:
                    angle = (int)Math.Floor(random.NextDouble() * (240 - 210 + 1) + 210);
                    break;
                case Shots.Coverdrive:
                    angle = (int)Math.Floor(random.NextDouble() * (270 - 240 + 1) + 240);
                    break;
                case Shots.Offdrive:
                    angle = (int)Math.Floor(random.NextDouble() * (330 - 270 + 1) + 270);
                    break;

                default:
                    angle = 0;
                    break;
            }

            return calculateSpeed(angle, nextball);
        }

        private int calculateSpeed(int angle, BallModel ballingData)
        {
            double speedofBat = 100;
            double speedOfBall = ballingData.speed * 0.28; // converting into m/s
            int speedOfBallInTrajectory = 33;
            double massOfBall = 153;
            double massOfBat = 1500;


            double speedOfTrajectoryBall =
              massOfBall *
              speedOfBallInTrajectory *
              speedOfBallInTrajectory;
            

            double speedOfBallInitial = massOfBall * speedOfBall * speedOfBall;

            speedofBat = Math.Sqrt(
              Math.Abs((speedOfBallInitial + speedOfTrajectoryBall) / massOfBat)
            );

            speedofBat *= 3.6; //converting into km/h
            return (int)Math.Round(speedofBat);
        }
    }
}

