using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestBot.Batting;
using TestBot.Bowling;
using TestBot.Fielding;
using TestBot.Match;


namespace TestBot.Controllers
{
    [Route("api/CricketBot")]
    [ApiController]
    public class HardController : ControllerBase
    {

        private  BowlingService _bowlingService;

        public static BallModel CurrentBall;

        public static BallModel currentBallFromOpponent;

        public static BatsmanModel CurrentShot;

        public static int currentBowlingConfigId;

        public static Dictionary<BowlingType, List<Shots>> _BallToShotMap = LoadBallToShotMap();
        public static Dictionary<BowlingType, List<Shots>> _BallToShotMap_Batting = LoadBallToShotMap();
        
        private static Dictionary<int, int> _perfectBatSpeedMap = LoadBatSpeedToBallSpeed();

        private static IDictionary<BowlingType, Shots> _perfectShotsForBallType = new Dictionary<BowlingType, Shots>();

        public HardController(IService bowlingService)
        {
            _bowlingService = (BowlingService)bowlingService;

        }

       

        [HttpGet]
        [Route("GetNextBall")]
        public BallModel GetNextBall()
        {
            return CurrentBall ?? _bowlingService.getBowlingData().Item1;
        }

        [HttpGet]
        [Route("oldData")]
        public int OldData()
        {
            return  _bowlingService.getIsTriedCount();
        }


        [HttpGet]
        [Route("GetBattingConfig")]
        public List<BattingRecords> GetBattingConfig()
        {
            return _bowlingService.getBattingConfigs();
        }


        [HttpPost]
        [Route("PostBalldata")]
        public BatsmanModel PostBalldata(BallModel nextball)
        {
            currentBallFromOpponent = nextball;

            Shots Shot = _perfectShotsForBallType.ContainsKey(nextball.bowingType) ? _perfectShotsForBallType[nextball.bowingType] : _bowlingService.getShot(nextball);

            int batSpeed = GetBatSpeed(nextball, Shot);

            _bowlingService.insertBattingConfig(Shot, batSpeed, nextball.speed);

            var playesShot = new BatsmanModel
            {
                batSpeed = batSpeed,
                shots = Shot,
                batsman = _bowlingService.getPlayerName()
            };

            CurrentShot = playesShot;

            return playesShot;
        }

        private int GetBatSpeed(BallModel nextball, Shots Shot)
        {
            if(_perfectBatSpeedMap.ContainsKey(nextball.speed))
            {
                return _perfectBatSpeedMap[nextball.speed];
            }

            for (int i = nextball.speed; i <= nextball.speed+5; i++)
            {
                if (_perfectBatSpeedMap.ContainsKey(i))
                {
                    return (int)(_perfectBatSpeedMap[i] - (i - nextball.speed) * 0.75);
                }
            }

            for (int i = nextball.speed; i >= nextball.speed - 5; i--)
            {
                if (_perfectBatSpeedMap.ContainsKey(i))
                {
                    return (int)(_perfectBatSpeedMap[i] + (i - nextball.speed) * 0.75);
                }
            }

            return (int)(_bowlingService.getBatSpeed(Shot, nextball) * 1.3);
        }

        [HttpPost]
        [Route("Postfieldsetting")]
        public HttpStatusCode Postfieldsetting(List<FieldingModel> fieldingModels)
        {
            return HttpStatusCode.OK;
        }


        [HttpPost]
        [Route("PostLastballStatus")]
        public HttpStatusCode PostLastballStatus(MatchProgressModel matchProgress)
        {
            _bowlingService.postLastBallData(matchProgress, currentBowlingConfigId, CurrentShot);

            TuneBatting(matchProgress);

            return HttpStatusCode.OK;
        }


        [HttpGet]
        [Route("Getfieldsetting")]
        public List<FieldingModel> Getfieldsetting()
        {
            var result = _bowlingService.getBowlingData();

            CurrentBall = result.Item1;
            currentBowlingConfigId = result.Item2;

            return getFieldForBall(CurrentBall);         
        }

        [HttpGet]
        [Route("Toss")]
        public Toss GetTossCall()
        {
            return Toss.Tail;
        }

        private List<FieldingModel> getFieldForBall(BallModel currentBall)
        {
            List<FieldingModel> fieldingModels = new List<FieldingModel>();

            List<Shots> shots = _BallToShotMap[currentBall.bowingType];


            foreach (var shot in shots)
            {
                fieldingModels.Add(new FieldingModel
                {
                    fp = fieldPosition.z4,
                    Prvshot = shot
                });
            }

            foreach (var shot in shots)
            {             
                    fieldingModels.Add(new FieldingModel
                    {
                        fp = fieldPosition.z3,
                        Prvshot = shot
                    });                             
            }

            foreach (var shot in shots)
            {             
                    fieldingModels.Add(new FieldingModel
                    {
                        fp = fieldPosition.z2,
                        Prvshot = shot
                    });
            }

            foreach (var shot in shots)
            {
                fieldingModels.Add(new FieldingModel
                {
                    fp = fieldPosition.z1,
                    Prvshot = shot
                });
            }

            return fieldingModels.Take(9).ToList();
        }


        private void TuneBatting(MatchProgressModel matchProgress)
        {
            if(matchProgress.runonlastball == 6)
            {
                setShotForBowlingType();
                setBatSpeedForBallSpeed(CurrentShot.batSpeed);
            }
            // check isShotValid 's reason
            if(matchProgress.isMissed && _BallToShotMap_Batting[currentBallFromOpponent.bowingType].Count > 1)
            {
                _BallToShotMap_Batting[currentBallFromOpponent.bowingType].Remove(CurrentShot.shots);
            }

            if (matchProgress.runonlastball == 1)
            {
                int newComputedSpeed = (int)(CurrentShot.batSpeed * 1.2);
                setBatSpeedForBallSpeed(newComputedSpeed > 120 ? 120 : newComputedSpeed);
            }
            if (matchProgress.runonlastball == 2 || (matchProgress.iswicketlost && matchProgress.OutReason == "Caught Out"))
            {
                int newComputedSpeed = (int)(CurrentShot.batSpeed * 1.15);
                setBatSpeedForBallSpeed(newComputedSpeed > 120 ? 120 : newComputedSpeed);
            }
            if (matchProgress.runonlastball == 3)
            {
                int newComputedSpeed = (int)(CurrentShot.batSpeed * 1.1);
                setBatSpeedForBallSpeed(newComputedSpeed > 120 ? 120 : newComputedSpeed);
            }
            if (matchProgress.runonlastball == 4)
            {
                int newComputedSpeed = (int)(CurrentShot.batSpeed * 1.05);
                setBatSpeedForBallSpeed(newComputedSpeed > 120 ? 120 : newComputedSpeed);
            }

        }

        private void setBatSpeedForBallSpeed(int batSpeed)
        {
            if (_perfectBatSpeedMap.ContainsKey(currentBallFromOpponent.speed))
            {
                _perfectBatSpeedMap[currentBallFromOpponent.speed] = batSpeed;
            }
            else
            {
                _perfectBatSpeedMap.Add(currentBallFromOpponent.speed, batSpeed);
            }
        }

        private static void setShotForBowlingType()
        {
            if (_perfectShotsForBallType.ContainsKey(currentBallFromOpponent.bowingType))
            {
                _perfectShotsForBallType[currentBallFromOpponent.bowingType] = CurrentShot.shots;
            }
            else
            {
                _perfectShotsForBallType.Add(currentBallFromOpponent.bowingType, CurrentShot.shots);
            }
        }

        private static Dictionary<int, int> LoadBatSpeedToBallSpeed()
        {
            var map = new Dictionary<int, int>();

            map.Add(160, 83);
            map.Add(130, 72);

            return map;
        }

        private static Dictionary<BowlingType, List<Shots>> LoadBallToShotMap()
        {
            var map = new Dictionary<BowlingType, List<Shots>>();

            map.Add(BowlingType.Inswingers, new List<Shots> { Shots.Ondrive, Shots.pull, Shots.Straightdrive, Shots.Sweep });
            map.Add(BowlingType.Outswinger, new List<Shots> { Shots.Coverdrive, Shots.Cut, Shots.latecut, Shots.Offdrive });
            map.Add(BowlingType.Bouncer, new List<Shots> { Shots.Uppercut, Shots.pull, Shots.hook });
            map.Add(BowlingType.OffBreak, new List<Shots> { Shots.Sweep, Shots.latecut, Shots.Straightdrive, Shots.Ondrive });
            map.Add(BowlingType.LegBreak, new List<Shots> { Shots.Coverdrive, Shots.Cut, Shots.latecut, Shots.Offdrive });
            map.Add(BowlingType.Googly, new List<Shots> { Shots.Sweep, Shots.latecut, Shots.Straightdrive, Shots.Ondrive });

            return map;
        }
    }
}
