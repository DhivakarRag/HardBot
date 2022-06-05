using System;
using System.Collections.Generic;
using System.Text;

namespace TestBot.Batting
{
    //ref: https://en.wikipedia.org/wiki/File:Cricket_shots.svg
    public enum Shots
    {
        leave, 
        Defensiveshot,
        Cut,// OutSwinger,LegBreak,OffBreak
        squarecut,//OutSwinger,LegBreak,OffBreak
        latecut,//OutSwinger,LegBreak,OffBreak
        Squaredrive,//OutSwinger
        pull, // Inswinger,Bouncer,LegBreak,OffBreak,googly
        hook, // Bouncer
        Coverdrive, //Inswinger,OutSwinger,OffBreak
        Offdrive, // InSwinger,OutSwinger,OffBreak
        Straightdrive, //Inswinger,OffBreak,googly
        Ondrive, // Inswinger,LegBreak,OffBreak,googly
        Sweep, // Inswinger,LegBreak,OffBreak,googly
        Uppercut //Bouncer
    }
}
