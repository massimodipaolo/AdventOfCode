﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2017
{
    class _20 : Puzzle
    {
        public _20()
        {
            ReadInputFromFile("2017/20.txt");
        }

        /*
--- Day 20: Particle Swarm ---
Suddenly, the GPU contacts you, asking for help. Someone has asked it to simulate too many particles, and it won't be able to finish them all in time to render the next frame at this rate.

It transmits to you a buffer (your puzzle input) listing each particle in order (starting with particle 0, then particle 1, particle 2, and so on). 
For each particle, it provides the X, Y, and Z coordinates for the particle's position (p), velocity (v), and acceleration (a), each in the format <X,Y,Z>.

Each tick, all particles are updated simultaneously. A particle's properties are updated in the following order:

Increase the X velocity by the X acceleration.
Increase the Y velocity by the Y acceleration.
Increase the Z velocity by the Z acceleration.
Increase the X position by the X velocity.
Increase the Y position by the Y velocity.
Increase the Z position by the Z velocity.
Because of seemingly tenuous rationale involving z-buffering, the GPU would like to know which particle will stay closest to position <0,0,0> in the long term. 
Measure this using the Manhattan distance, which in this situation is simply the sum of the absolute values of a particle's X, Y, and Z position.

For example, suppose you are only given two particles, both of which stay entirely on the X-axis (for simplicity). 
Drawing the current states of particles 0 and 1 (in that order) with an adjacent a number line and diagram of current X positions (marked in parenthesis), the following would take place:

p=< 3,0,0>, v=< 2,0,0>, a=<-1,0,0>    -4 -3 -2 -1  0  1  2  3  4
p=< 4,0,0>, v=< 0,0,0>, a=<-2,0,0>                         (0)(1)

p=< 4,0,0>, v=< 1,0,0>, a=<-1,0,0>    -4 -3 -2 -1  0  1  2  3  4
p=< 2,0,0>, v=<-2,0,0>, a=<-2,0,0>                      (1)   (0)

p=< 4,0,0>, v=< 0,0,0>, a=<-1,0,0>    -4 -3 -2 -1  0  1  2  3  4
p=<-2,0,0>, v=<-4,0,0>, a=<-2,0,0>          (1)               (0)

p=< 3,0,0>, v=<-1,0,0>, a=<-1,0,0>    -4 -3 -2 -1  0  1  2  3  4
p=<-8,0,0>, v=<-6,0,0>, a=<-2,0,0>                         (0)   
At this point, particle 1 will never be closer to <0,0,0> than particle 0, and so, in the long run, particle 0 will stay closest.

Which particle will stay closest to position <0,0,0> in the long term?         
             */
        public override string Output(string input)
        {
            var particels = GetParticels(input);
            var longTermClosest = particels.OrderBy(_ => Math.Abs(_.Acceleration.X) + Math.Abs(_.Acceleration.Y) + Math.Abs(_.Acceleration.Z)).FirstOrDefault();
            return longTermClosest.Id.ToString();
        }

        /*
--- Part Two ---
To simplify the problem further, the GPU would like to remove any particles that collide. 
Particles collide if their positions ever exactly match. 
Because particles are updated simultaneously, more than two particles can collide at the same time and place. 
Once particles collide, they are removed and cannot collide with anything else after that tick.

For example:

p=<-6,0,0>, v=< 3,0,0>, a=< 0,0,0>    
p=<-4,0,0>, v=< 2,0,0>, a=< 0,0,0>    -6 -5 -4 -3 -2 -1  0  1  2  3
p=<-2,0,0>, v=< 1,0,0>, a=< 0,0,0>    (0)   (1)   (2)            (3)
p=< 3,0,0>, v=<-1,0,0>, a=< 0,0,0>

p=<-3,0,0>, v=< 3,0,0>, a=< 0,0,0>    
p=<-2,0,0>, v=< 2,0,0>, a=< 0,0,0>    -6 -5 -4 -3 -2 -1  0  1  2  3
p=<-1,0,0>, v=< 1,0,0>, a=< 0,0,0>             (0)(1)(2)      (3)   
p=< 2,0,0>, v=<-1,0,0>, a=< 0,0,0>

p=< 0,0,0>, v=< 3,0,0>, a=< 0,0,0>    
p=< 0,0,0>, v=< 2,0,0>, a=< 0,0,0>    -6 -5 -4 -3 -2 -1  0  1  2  3
p=< 0,0,0>, v=< 1,0,0>, a=< 0,0,0>                       X (3)      
p=< 1,0,0>, v=<-1,0,0>, a=< 0,0,0>

------destroyed by collision------    
------destroyed by collision------    -6 -5 -4 -3 -2 -1  0  1  2  3
------destroyed by collision------                      (3)         
p=< 0,0,0>, v=<-1,0,0>, a=< 0,0,0>
In this example, particles 0, 1, and 2 are simultaneously destroyed at the time and place marked X. On the next tick, particle 3 passes through unharmed.

How many particles are left after all collisions are resolved?         
             */
        public override string Output2(string input)
        {
            var particels = GetParticels(input);
            foreach (var p in particels)
                p.MinDistance = p.Distance();

            while (true)
            {
                var list = particels.Where(_ => !_.Destroyed);

                if (!list.Any()) break;

                var someAreCollapsing = false;
                foreach (var p in list)
                {
                    p.Velocity.X += p.Acceleration.X;
                    p.Velocity.Y += p.Acceleration.Y;
                    p.Velocity.Z += p.Acceleration.Z;
                    p.Location.X += p.Velocity.X;
                    p.Location.Y += p.Velocity.Y;
                    p.Location.Z += p.Velocity.Z;

                    var distance = p.Distance();
                    if (distance < p.MinDistance)
                    {
                        p.MinDistance = distance;
                        someAreCollapsing = true;
                    }

                }

                var samePositionGroup = particels.GroupBy(p => p.Location.X.ToString() + p.Location.Y.ToString() + p.Location.Z.ToString()).Where(g => g.Count() > 1);
                if (samePositionGroup.Any())
                {
                    foreach (var g in samePositionGroup)
                        foreach (var p in g)
                            p.Destroyed = true;
                }

                if (!someAreCollapsing) break;
            }

            return particels.Where(_ => !_.Destroyed).Count().ToString();
        }

        public class Particel
        {
            public int Id { get; set; }
            public _3D Location { get; set; }
            public _3D Velocity { get; set; }
            public _3D Acceleration { get; set; }
            public bool Destroyed { get; set; } = false;
            public long MinDistance { get; set; }
            public long Distance() => Math.Abs(Location.X) + Math.Abs(Location.Y) + Math.Abs(Location.Z);
        }

        public List<Particel> GetParticels(string input)
        {
            var particels = new List<Particel>();
            particels.AddRange(
                input
                .Split("\n")
                .Select((row, i) =>
                {
                    var sep = row.Split(" ");
                    var loc = sep[0].Replace(">,", "").Split("p=<")[1].Split(",");
                    var vel = sep[1].Replace(">,", "").Split("v=<")[1].Split(",");
                    var acc = sep[2].Replace(">", "").Split("a=<")[1].Split(",");
                    return new Particel()
                    {
                        Id = i,
                        Location = new _3D { X = int.Parse(loc[0]), Y = int.Parse(loc[1]), Z = int.Parse(loc[2]) },
                        Velocity = new _3D { X = int.Parse(vel[0]), Y = int.Parse(vel[1]), Z = int.Parse(vel[2]) },
                        Acceleration = new _3D { X = int.Parse(acc[0]), Y = int.Parse(acc[1]), Z = int.Parse(acc[2]) }
                    };
                }
                    )
                );
            return particels;
        }

        public class _3D
        {
            public long X { get; set; }
            public long Y { get; set; }
            public long Z { get; set; }
        }
    }
}
