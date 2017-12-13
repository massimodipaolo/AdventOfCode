using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode._2017
{
    class _03 : Puzzle
    {
        public _03()
        {            
            Input = "361527";
        }

        /*
         --- Day 3: Spiral Memory ---
You come across an experimental new kind of memory stored on an infinite two-dimensional grid.

Each square on the grid is allocated in a spiral pattern starting at a location marked 1 and then counting up while spiraling outward. For example, the first few squares are allocated like this:

17  16  15  14  13
18   5   4   3  12
19   6   1   2  11
20   7   8   9  10
21  22  23---> ...
While this is very space-efficient (no squares are skipped), requested data must be carried back to square 1 (the location of the only access port for this memory system) by programs that can only move up, down, left, or right. They always take the shortest path: the Manhattan Distance between the location of the data and square 1.

For example:

Data from square 1 is carried 0 steps, since it's at the access port.
Data from square 12 is carried 3 steps, such as: down, left, left.
Data from square 23 is carried only 2 steps: up twice.
Data from square 1024 must be carried 31 steps.
How many steps are required to carry the data from the square identified in your puzzle input all the way to the access port?
             */
        public override string Output(string input)
        {
            // intuitivamente = 326                        
            var center = new Tuple<int,int>(0,0);
            var point = getCoords(int.Parse(input.ToString()));
            return (Math.Abs(point.Item1-center.Item1) + Math.Abs(point.Item2 - center.Item2)).ToString();
        }

        private Tuple<int,int> getCoords(int num)
        {
            int size = (int)Math.Sqrt(num);
            int sq = (int)Math.Pow(size,2);
            int sqCoord = ((int)Math.Sqrt(sq)) / 2;
            int x = 0;int y = 0;
            int diff = num - sq;
            if (sq % 2 == 1)
            {
                x = sqCoord;
                y = -sqCoord;
                if (diff > 0)
                {
                    x++;
                    y += (Math.Min(diff,size) - 1);
                    if (diff > size)
                    {
                        y++;
                        x -= diff - size - 1;
                    }
                        
                }
            } else
            {
                x = -sqCoord + 1;
                y = sqCoord;
                if (diff > 0)
                {
                    x--;
                    y -= (Math.Min(diff, size) - 1);
                    if (diff > size)
                    {
                        y--;
                        x += diff - size - 1;
                    }                        
                }
            }            
            return new Tuple<int, int>(x, y);
        }

        public override string Output2(string input)
        {
            return base.Output2(input);
        }

    }
}
