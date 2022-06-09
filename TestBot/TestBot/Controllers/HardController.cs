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

        public static int currentBowlingConfigId;

        public static IDictionary<BowlingType, List<Shots>> _BallToShotMap;

        public HardController(IService bowlingService)
        {
            _bowlingService = (BowlingService)bowlingService;
            _BallToShotMap = LoadBallToShotMap();
        }

        private IDictionary<BowlingType, List<Shots>> LoadBallToShotMap()
        {
            var map =  new Dictionary<BowlingType, List<Shots>>();

            map.Add(BowlingType.Inswingers, new List<Shots> { Shots.Ondrive,Shots.pull,Shots.Straightdrive,Shots.Sweep});
            map.Add(BowlingType.Outswinger, new List<Shots> { Shots.Coverdrive,Shots.Cut,Shots.latecut,Shots.Offdrive });
            map.Add(BowlingType.Bouncer, new List<Shots> {Shots.Uppercut,Shots.pull,Shots.hook });
            map.Add(BowlingType.OffBreak, new List<Shots> {Shots.Sweep,Shots.latecut,Shots.Straightdrive,Shots.Ondrive });
            map.Add(BowlingType.LegBreak, new List<Shots> { Shots.Coverdrive, Shots.Cut, Shots.latecut, Shots.Offdrive });
            map.Add(BowlingType.Googly, new List<Shots> { Shots.Sweep, Shots.latecut, Shots.Straightdrive, Shots.Ondrive });

            return map;
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


        [HttpPost]
        [Route("PostBalldata")]
        public BatsmanModel PostBalldata(BallModel nextball)
        {
            Shots Shot = _bowlingService.getShot(nextball);
            int batSpeed = _bowlingService.getBatSpeed(Shot,nextball);

            return new BatsmanModel
            {
                batSpeed = batSpeed,
                shots = Shot,
                batsman = _bowlingService.getPlayerName()
            };
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
            _bowlingService.postLastBallData(matchProgress, currentBowlingConfigId);

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
    }
}
