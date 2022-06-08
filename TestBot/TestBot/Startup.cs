using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TestBot.Match;
using Microsoft.OpenApi.Models;
using TestBot.Bowling;
using TestBot.Controllers;

namespace TestBot
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDbContext<MatchContext>(option => option.UseInMemoryDatabase(Configuration.GetConnectionString("HARD_Database")));
            services.AddTransient<IRepository, HardRepository>();
            services.AddTransient<IService, BowlingService>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "HARD BOT API",
                    Version = "v10",
                    Description = "Description for the API goes here.",
                    Contact = new OpenApiContact
                    {
                        Name = "HARD Bot",
                        Email = string.Empty,
                        Url = new Uri("https://coderjony.com/"),
                    },
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();

      
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "HARD Bot API V1");

                c.RoutePrefix = string.Empty;
            });

            var scope = app.ApplicationServices.CreateScope();

            
            seedBowlingData(scope.ServiceProvider.GetService<MatchContext>());
        }

        public static void seedBowlingData(MatchContext context)
        {

            //RAF Inswingers
            context.BowlingConfigs.Add(new BowlingConfigs
            {
                id = 1,
                bowlerType = BowlerTypes.RAF,
                bowlingType = BowlingType.Inswingers,
                speed = Speedlimit.RAF_MAX,
                pitchZone = BallPitchZone.zone2,
                isTried = false
            }) ;
            context.BowlingConfigs.Add(new BowlingConfigs
            {
                id = 2,
                bowlerType = BowlerTypes.RAF,
                bowlingType = BowlingType.Inswingers,
                speed = Speedlimit.RAF_MIN,
                pitchZone = BallPitchZone.zone2,
                isTried = false
            });
            context.BowlingConfigs.Add(new BowlingConfigs
            {
                id = 3,
                bowlerType = BowlerTypes.RAF,
                bowlingType = BowlingType.Inswingers,
                speed = (int)(Speedlimit.RAF_MIN + Speedlimit.RAF_MAX) /2,
                pitchZone = BallPitchZone.zone2,
                isTried = false
            });


           // RAF Outswingers
            context.BowlingConfigs.Add(new BowlingConfigs
            {
                id = 4,
                bowlerType = BowlerTypes.RAF,
                bowlingType = BowlingType.Outswinger,
                speed = Speedlimit.RAF_MAX,
                pitchZone = BallPitchZone.zone2,
                isTried = false
            });
            context.BowlingConfigs.Add(new BowlingConfigs
            {
                id = 5,
                bowlerType = BowlerTypes.RAF,
                bowlingType = BowlingType.Outswinger,
                speed = Speedlimit.RAF_MIN,
                pitchZone = BallPitchZone.zone2,
                isTried = false
            });
            context.BowlingConfigs.Add(new BowlingConfigs
            {
                id = 6,
                bowlerType = BowlerTypes.RAF,
                bowlingType = BowlingType.Outswinger,
                speed = (int)(Speedlimit.RAF_MIN + Speedlimit.RAF_MAX) / 2,
                pitchZone = BallPitchZone.zone2,
                isTried = false
            });

            //RAF_BOUNCER

            context.BowlingConfigs.Add(new BowlingConfigs
            {
                id = 7,
                bowlerType = BowlerTypes.RAF,
                bowlingType = BowlingType.Bouncer,
                speed = Speedlimit.RAF_MAX,
                pitchZone = BallPitchZone.zone1,
                isTried = false
            });
            context.BowlingConfigs.Add(new BowlingConfigs
            {
                id = 8,
                bowlerType = BowlerTypes.RAF,
                bowlingType = BowlingType.Bouncer,
                speed = Speedlimit.RAF_MIN,
                pitchZone = BallPitchZone.zone1,
                isTried = false
            });
            context.BowlingConfigs.Add(new BowlingConfigs
            {
                id = 9,
                bowlerType = BowlerTypes.RAF,
                bowlingType = BowlingType.Bouncer,
                speed = (int)(Speedlimit.RAF_MIN + Speedlimit.RAF_MAX) / 2,
                pitchZone = BallPitchZone.zone1,
                isTried = false
            });

            //RAFM Inswingers
            context.BowlingConfigs.Add(new BowlingConfigs
            {
                id = 10,
                bowlerType = BowlerTypes.RAFM,
                bowlingType = BowlingType.Inswingers,
                speed = Speedlimit.RAFM_MAX,
                pitchZone = BallPitchZone.zone2,
                isTried = false
            });
            context.BowlingConfigs.Add(new BowlingConfigs
            {
                id = 11,
                bowlerType = BowlerTypes.RAFM,
                bowlingType = BowlingType.Inswingers,
                speed = Speedlimit.RAFM_MIN,
                pitchZone = BallPitchZone.zone2,
                isTried = false
            });
            context.BowlingConfigs.Add(new BowlingConfigs
            {
                id = 12,
                bowlerType = BowlerTypes.RAFM,
                bowlingType = BowlingType.Inswingers,
                speed = (int)(Speedlimit.RAFM_MIN + Speedlimit.RAFM_MAX) / 2,
                pitchZone = BallPitchZone.zone2,
                isTried = false
            });


            // RAFM Outswingers
            context.BowlingConfigs.Add(new BowlingConfigs
            {
                id = 13,
                bowlerType = BowlerTypes.RAFM,
                bowlingType = BowlingType.Outswinger,
                speed = Speedlimit.RAFM_MAX,
                pitchZone = BallPitchZone.zone2,
                isTried = false
            });
            context.BowlingConfigs.Add(new BowlingConfigs
            {
                id = 14,
                bowlerType = BowlerTypes.RAFM,
                bowlingType = BowlingType.Outswinger,
                speed = Speedlimit.RAFM_MIN,
                pitchZone = BallPitchZone.zone2,
                isTried = false
            });
            context.BowlingConfigs.Add(new BowlingConfigs
            {
                id = 15,
                bowlerType = BowlerTypes.RAFM,
                bowlingType = BowlingType.Outswinger,
                speed = (int)(Speedlimit.RAFM_MIN + Speedlimit.RAFM_MAX) / 2,
                pitchZone = BallPitchZone.zone2,
                isTried = false
            });

            //RAFM_BOUNCER

            context.BowlingConfigs.Add(new BowlingConfigs
            {
                id = 16,
                bowlerType = BowlerTypes.RAFM,
                bowlingType = BowlingType.Bouncer,
                speed = Speedlimit.RAFM_MAX,
                pitchZone = BallPitchZone.zone1,
                isTried = false
            });
            context.BowlingConfigs.Add(new BowlingConfigs
            {
                id = 17,
                bowlerType = BowlerTypes.RAFM,
                bowlingType = BowlingType.Bouncer,
                speed = Speedlimit.RAFM_MIN,
                pitchZone = BallPitchZone.zone1,
                isTried = false
            });
            context.BowlingConfigs.Add(new BowlingConfigs
            {
                id = 18,
                bowlerType = BowlerTypes.RAFM,
                bowlingType = BowlingType.Bouncer,
                speed = (int)(Speedlimit.RAFM_MIN + Speedlimit.RAFM_MAX) / 2,
                pitchZone = BallPitchZone.zone1,
                isTried = false
            });

            context.BowlingConfigs.Add(new BowlingConfigs
            {
                id = 19,
                bowlerType = BowlerTypes.RAS,
                bowlingType = BowlingType.Inswingers,
                speed = Speedlimit.RAS_MAX,
                pitchZone = BallPitchZone.zone2,
                isTried = false
            });
            context.BowlingConfigs.Add(new BowlingConfigs
            {
                id = 20,
                bowlerType = BowlerTypes.RAS,
                bowlingType = BowlingType.Inswingers,
                speed = Speedlimit.RAS_MIN,
                pitchZone = BallPitchZone.zone2,
                isTried = false
            });
            context.BowlingConfigs.Add(new BowlingConfigs
            {
                id = 21,
                bowlerType = BowlerTypes.RAS,
                bowlingType = BowlingType.Inswingers,
                speed = (int)(Speedlimit.RAS_MIN + Speedlimit.RAS_MAX) / 2,
                pitchZone = BallPitchZone.zone2,
                isTried = false
            });


            // RAS Outswingers
            context.BowlingConfigs.Add(new BowlingConfigs
            {
                id = 22,
                bowlerType = BowlerTypes.RAS,
                bowlingType = BowlingType.Outswinger,
                speed = Speedlimit.RAS_MAX,
                pitchZone = BallPitchZone.zone2,
                isTried = false
            });
            context.BowlingConfigs.Add(new BowlingConfigs
            {
                id = 23,
                bowlerType = BowlerTypes.RAS,
                bowlingType = BowlingType.Outswinger,
                speed = Speedlimit.RAS_MIN,
                pitchZone = BallPitchZone.zone2,
                isTried = false
            });
            context.BowlingConfigs.Add(new BowlingConfigs
            {
                id = 24,
                bowlerType = BowlerTypes.RAS,
                bowlingType = BowlingType.Outswinger,
                speed = (int)(Speedlimit.RAS_MIN + Speedlimit.RAS_MAX) / 2,
                pitchZone = BallPitchZone.zone2,
                isTried = false
            });

            //RAS_BOUNCER

            context.BowlingConfigs.Add(new BowlingConfigs
            {
                id = 25,
                bowlerType = BowlerTypes.RAS,
                bowlingType = BowlingType.Bouncer,
                speed = Speedlimit.RAS_MAX,
                pitchZone = BallPitchZone.zone1,
                isTried = false
            });
            context.BowlingConfigs.Add(new BowlingConfigs
            {
                id = 26,
                bowlerType = BowlerTypes.RAS,
                bowlingType = BowlingType.Bouncer,
                speed = Speedlimit.RAS_MIN,
                pitchZone = BallPitchZone.zone1,
                isTried = false
            });
            context.BowlingConfigs.Add(new BowlingConfigs
            {
                id = 27,
                bowlerType = BowlerTypes.RAS,
                bowlingType = BowlingType.Bouncer,
                speed = (int)(Speedlimit.RAS_MIN + Speedlimit.RAS_MAX) / 2,
                pitchZone = BallPitchZone.zone1,
                isTried = false
            });
            //OB OffBreak
            context.BowlingConfigs.Add(new BowlingConfigs
            {
                id = 28,
                bowlerType = BowlerTypes.OB,
                bowlingType = BowlingType.OffBreak,
                speed = Speedlimit.OB_MAX,
                pitchZone = BallPitchZone.zone2,
                isTried = false
            });
            context.BowlingConfigs.Add(new BowlingConfigs
            {
                id = 29,
                bowlerType = BowlerTypes.OB,
                bowlingType = BowlingType.OffBreak,
                speed = Speedlimit.OB_MIN,
                pitchZone = BallPitchZone.zone2,
                isTried = false
            });
            context.BowlingConfigs.Add(new BowlingConfigs
            {
                id = 30,
                bowlerType = BowlerTypes.OB,
                bowlingType = BowlingType.OffBreak,
                speed = (int)(Speedlimit.OB_MIN + Speedlimit.OB_MAX) / 2,
                pitchZone = BallPitchZone.zone2,
                isTried = false
            });


            // OB LegBreaks
            context.BowlingConfigs.Add(new BowlingConfigs
            {
                id = 31,
                bowlerType = BowlerTypes.OB,
                bowlingType = BowlingType.LegBreak,
                speed = Speedlimit.OB_MAX,
                pitchZone = BallPitchZone.zone2,
                isTried = false
            });
            context.BowlingConfigs.Add(new BowlingConfigs
            {
                id = 32,
                bowlerType = BowlerTypes.OB,
                bowlingType = BowlingType.LegBreak,
                speed = Speedlimit.OB_MIN,
                pitchZone = BallPitchZone.zone2,
                isTried = false
            });
            context.BowlingConfigs.Add(new BowlingConfigs
            {
                id = 33,
                bowlerType = BowlerTypes.OB,
                bowlingType = BowlingType.LegBreak,
                speed = (int)(Speedlimit.OB_MIN + Speedlimit.OB_MAX) / 2,
                pitchZone = BallPitchZone.zone2,
                isTried = false
            });

            //OB_Googly

            context.BowlingConfigs.Add(new BowlingConfigs
            {
                id = 34,
                bowlerType = BowlerTypes.OB,
                bowlingType = BowlingType.Googly,
                speed = Speedlimit.OB_MAX,
                pitchZone = BallPitchZone.zone1,
                isTried = false
            });
            context.BowlingConfigs.Add(new BowlingConfigs
            {
                id = 35,
                bowlerType = BowlerTypes.OB,
                bowlingType = BowlingType.Googly,
                speed = Speedlimit.OB_MIN,
                pitchZone = BallPitchZone.zone1,
                isTried = false
            });
            context.BowlingConfigs.Add(new BowlingConfigs
            {
                id = 36,
                bowlerType = BowlerTypes.OB,
                bowlingType = BowlingType.Googly,
                speed = (int)(Speedlimit.OB_MIN + Speedlimit.OB_MAX) / 2,
                pitchZone = BallPitchZone.zone1,
                isTried = false
            });

            //LB OffBreak
            context.BowlingConfigs.Add(new BowlingConfigs
            {
                id = 37,
                bowlerType = BowlerTypes.LB,
                bowlingType = BowlingType.OffBreak,
                speed = Speedlimit.LB_MAX,
                pitchZone = BallPitchZone.zone2,
                isTried = false
            });
            context.BowlingConfigs.Add(new BowlingConfigs
            {
                id = 38,
                bowlerType = BowlerTypes.LB,
                bowlingType = BowlingType.OffBreak,
                speed = Speedlimit.LB_MIN,
                pitchZone = BallPitchZone.zone2,
                isTried = false
            });
            context.BowlingConfigs.Add(new BowlingConfigs
            {
                id = 39,
                bowlerType = BowlerTypes.LB,
                bowlingType = BowlingType.OffBreak,
                speed = (int)(Speedlimit.LB_MIN + Speedlimit.LB_MAX) / 2,
                pitchZone = BallPitchZone.zone2,
                isTried = false
            });


            // LB LegBreaks
            context.BowlingConfigs.Add(new BowlingConfigs
            {
                id = 40,
                bowlerType = BowlerTypes.LB,
                bowlingType = BowlingType.LegBreak,
                speed = Speedlimit.LB_MAX,
                pitchZone = BallPitchZone.zone2,
                isTried = false
            });
            context.BowlingConfigs.Add(new BowlingConfigs
            {
                id = 41,
                bowlerType = BowlerTypes.LB,
                bowlingType = BowlingType.LegBreak,
                speed = Speedlimit.LB_MIN,
                pitchZone = BallPitchZone.zone2,
                isTried = false
            });
            context.BowlingConfigs.Add(new BowlingConfigs
            {
                id = 42,
                bowlerType = BowlerTypes.LB,
                bowlingType = BowlingType.LegBreak,
                speed = (int)(Speedlimit.LB_MIN + Speedlimit.LB_MAX) / 2,
                pitchZone = BallPitchZone.zone2,
                isTried = false
            });

            //LB_Googly

            context.BowlingConfigs.Add(new BowlingConfigs
            {
                id = 43,
                bowlerType = BowlerTypes.LB,
                bowlingType = BowlingType.Googly,
                speed = Speedlimit.LB_MAX,
                pitchZone = BallPitchZone.zone1,
                isTried = false
            });
            context.BowlingConfigs.Add(new BowlingConfigs
            {
                id = 44,
                bowlerType = BowlerTypes.LB,
                bowlingType = BowlingType.Googly,
                speed = Speedlimit.LB_MIN,
                pitchZone = BallPitchZone.zone1,
                isTried = false
            });
            context.BowlingConfigs.Add(new BowlingConfigs
            {
                id = 45,
                bowlerType = BowlerTypes.LB,
                bowlingType = BowlingType.Googly,
                speed = (int)(Speedlimit.LB_MIN + Speedlimit.LB_MAX) / 2,
                pitchZone = BallPitchZone.zone1,
                isTried = false
            });


            context.SaveChanges();
        }
    }
}
