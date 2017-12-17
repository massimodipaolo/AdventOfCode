using System;
namespace AdventOfCode._2017
{
    public class _11 : Puzzle
    {
        public _11()
        {
            ReadInputFromFile("2017/11.txt");
        }

        /*
--- Day 11: Hex Ed ---
Crossing the bridge, you've barely reached the other side of the stream when a program comes up to you, clearly in distress. "It's my child process," she says, "he's gotten lost in an infinite grid!"

Fortunately for her, you have plenty of experience with infinite grids.

Unfortunately for you, it's a hex grid.

The hexagons ("hexes") in this grid are aligned such that adjacent hexes can be found to the north, northeast, southeast, south, southwest, and northwest:

  \ n  /
nw +--+ ne
  /    \
-+      +-
  \    /
sw +--+ se
  / s  \
You have the path the child process took. 
Starting where he started, you need to determine the fewest number of steps required to reach him. 
(A "step" means to move from the hex you are in to any adjacent hex.)

For example:

ne,ne,ne is 3 steps away.
ne,ne,sw,sw is 0 steps away (back where you started).
ne,ne,s,s is 2 steps away (se,se).
se,sw,se,sw,sw is 3 steps away (s,s,sw).
        */
        public override string Output(string input)
        {
            //input = "ne,ne,sw,sw";
            Console.WriteLine("--------------");
            Console.WriteLine(input);
            var point = new Point() { x = 0, y = 0 };
            foreach (var s in input.Split(','))
            {
                switch (s)
                {
                    case "n":
                        point.y++;
                        break;
                    case "ne":
                        point.x++;
                        point.y += .5;
                        break;
                    case "se":
                        point.x++;
                        point.y -= .5;
                        break;
                    case "s":
                        point.y--;
                        break;
                    case "sw":
                        point.x--;
                        point.y -= .5;
                        break;
                    case "nw":
                        point.x--;
                        point.y += .5;
                        break;
                }
            }

            Console.WriteLine($"{point.x},{point.y}");
            return calc(point).ToString();
        }

        private double calc(Point point)
        {
            return Math.Abs(point.y) >= Math.Abs(point.x) ?
                       Math.Abs(point.x) + Math.Abs(point.y) - Math.Abs(point.x * .5) // ok
                           :
                       Math.Abs(point.x) + Math.Floor(Math.Abs(point.y - 1) % 2); // fix (overstimate) 
        }

        public override string Output2(string input)
        {
            double distance = 0;
            int step = 0;
            var point = new Point() { x = 0, y = 0 };
            foreach (var s in input.Split(','))
            {
                switch (s)
                {
                    case "n":
                        point.y++;
                        break;
                    case "ne":
                        point.x++;
                        point.y += .5;
                        break;
                    case "se":
                        point.x++;
                        point.y -= .5;
                        break;
                    case "s":
                        point.y--;
                        break;
                    case "sw":
                        point.x--;
                        point.y -= .5;
                        break;
                    case "nw":
                        point.x--;
                        point.y += .5;
                        break;
                }
                var _distance = calc(point);
                if (_distance > distance) distance = _distance;
                Console.WriteLine($"{step++}) {point.x},{point.y}:{_distance}");
            }
            return distance.ToString();
        }

        public struct Point
        {
            public double x { get; set; }
            public double y { get; set; }
        }
    }
}