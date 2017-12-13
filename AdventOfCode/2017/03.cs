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
            var center = new Tuple<int, int>(0, 0);
            var point = getCoords(int.Parse(input.ToString()));
            return (Math.Abs(point.Item1 - center.Item1) + Math.Abs(point.Item2 - center.Item2)).ToString();
        }

        private Tuple<int, int> getCoords(int num)
        {
            int size = (int)Math.Sqrt(num);
            int sq = (int)Math.Pow(size, 2);
            int sqCoord = ((int)Math.Sqrt(sq)) / 2;
            int x = 0; int y = 0;
            int diff = num - sq;
            if (sq % 2 == 1)
            {
                x = sqCoord;
                y = -sqCoord;
                if (diff > 0)
                {
                    x++;
                    y += (Math.Min(diff, size) - 1);
                    if (diff > size)
                    {
                        y++;
                        x -= diff - size - 1;
                    }

                }
            }
            else
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
        /*
--- Part Two ---
As a stress test on the system, the programs here clear the grid and then store the value 1 in square 1. 
Then, in the same allocation order as shown above, they store the sum of the values in all adjacent squares, including diagonals.

So, the first few squares' values are chosen as follows:

Square 1 starts with the value 1.
Square 2 has only one adjacent filled square (with value 1), so it also stores 1.
Square 3 has both of the above squares as neighbors and stores the sum of their values, 2.
Square 4 has all three of the aforementioned squares as neighbors and stores the sum of their values, 4.
Square 5 only has the first and fourth squares as neighbors, so it gets the value 5.
Once a square is written, its value does not change. Therefore, the first few squares would receive the following values:

147  142  133  122   59
304    5    4    2   57
330   10    1    1   54
351   11   23   25   26
362  747  806--->   ...
What is the first value written that is larger than your puzzle input?
        */
        public override string Output2(string input)
        {
            var value = 0;

            var position = new Point() { x = 0, y = 0 };
            var squares = new Dictionary<string, int>();
            squares["0,0"] = 1;
            Func<Point, Point> R = _ => { _.x++; return _; };
            Func<Point, Point> U = _ => { _.y++; return _; };
            Func<Point, Point> L = _ => { _.x--; return _; };
            Func<Point, Point> D = _ => { _.y--; return _; };
            Func<Point, Point>[] move = { R, U, L, D };
            int moveFunc = 0;

            while (value < int.Parse(input))
            {
                value = 0;
                Point newP = move[moveFunc](position);
                if (squares.ContainsKey($"{newP.x},{newP.y}"))
                {
                    moveFunc = (moveFunc + 3) % 4;
                    newP = move[moveFunc](position);
                }
                position = newP;
                for (var _x = position.x - 1; _x <= position.x + 1; _x++)
                    for (var _y = position.y - 1; _y <= position.y + 1; _y++)
                        if (squares.ContainsKey($"{_x},{_y}"))
                            value += squares[$"{_x},{_y}"];

                squares[$"{position.x},{position.y}"] = value;
                //Console.WriteLine($"{value}: move_{moveFunc} => {position.x},{position.y}");
                moveFunc = (moveFunc + 1) % 4;
            }

            return value.ToString();
        }

        public struct Point
        {
            public int x { get; set; }
            public int y { get; set; }
        }

    }
}
