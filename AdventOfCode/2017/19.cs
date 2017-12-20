using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2017
{
    class _19 : Puzzle
    {
        public _19() {
            ReadInputFromFile("2017/19.txt");
        }

        /*
--- Day 19: A Series of Tubes ---
Somehow, a network packet got lost and ended up here. It's trying to follow a routing diagram (your puzzle input), but it's confused about where to go.

Its starting point is just off the top of the diagram. 
Lines (drawn with |, -, and +) show the path it needs to take, starting by going down onto the only line connected to the top of the diagram. 
It needs to follow this path until it reaches the end (located somewhere within the diagram) and stop there.

Sometimes, the lines cross over each other; in these cases, it needs to continue going the same direction, and only turn left or right when there's no other option. 
In addition, someone has left letters on the line; these also don't change its direction, but it can use them to keep track of where it's been. For example:

     |          
     |  +--+    
     A  |  C    
 F---|----E|--+ 
     |  |  |  D 
     +B-+  +--+ 

Given this diagram, the packet needs to take the following path:

Starting at the only line touching the top of the diagram, it must go down, pass through A, and continue onward to the first +.
Travel right, up, and right, passing through B in the process.
Continue down (collecting C), right, and up (collecting D).
Finally, go all the way left through E and stopping at F.
Following the path to the end, the letters it sees on its path are ABCDEF.

The little packet looks up at you, hoping you can help it find the way. 
What letters will it see (in the order it would see them) if it follows the path? 
(The routing diagram is very wide; make sure you view it without line wrapping.)         
             */
        public override string Output(string input)
        {
            return Solve(input, "path");
        }

        /*
--- Part Two ---
The packet is curious how many steps it needs to go.

For example, using the same routing diagram from the example above...

     |          
     |  +--+    
     A  |  C    
 F---|--|-E---+ 
     |  |  |  D 
     +B-+  +--+ 

...the packet would go:

6 steps down (including the first line at the top of the diagram).
3 steps right.
4 steps up.
3 steps right.
4 steps down.
3 steps right.
2 steps up.
13 steps left (including the F it stops on).
This would result in a total of 38 steps.

How many steps does the packet need to go?         
             */
        public override string Output2(string input)
        {
            return Solve(input, "step");
        }

        public class Point
        {
            public int x { get; set; }
            public int y { get; set; }
            public char c { get; set; }
        }

        public string Solve(string input,string type)
        {
            /*
            input = @"
     |          
     |  +--+    
     A  |  C    
 F---|----E|--+ 
     |  |  |  D 
     +B-+  +--+ 
";
            */

            var rows = input.Split("\n");

            var points = new List<Point>();
            // Map
            for (var y = 0; y < rows.Length; y++)
                for (var x = 0; x < rows[y].Length; x++)
                    if (rows[y][x] != ' ') points.Add(new Point() { x = x, y = y, c = rows[y][x] });

            //Func
            bool topright = true;
            Func<Point, Point> vertical = p => points.FirstOrDefault(_ => _.x == p.x && _.y == p.y + (topright ? 1 : -1));
            Func<Point, Point> horizontal = p => points.FirstOrDefault(_ => _.x == p.x + (topright ? 1 : -1) && _.y == p.y);

            // Move
            var paths = "";
            Point previous = null;
            var current = points[0];
            var next = new Point();

            var steps = 0;
            while (current != null)
            {
                steps++;
                switch (current.c)
                {
                    case '|':
                        if (previous?.y == current.y)
                            next = horizontal(current);
                        else
                            next = vertical(current);
                        break;
                    case '-':
                        if (previous?.x == current.x)
                            next = vertical(current);
                        else
                            next = horizontal(current);
                        break;
                    case '+':
                        if (previous.x == current.x)
                        {
                            next = points.FirstOrDefault(_ => new int[] { current.x + 1, current.x - 1 }.Contains(_.x) && _.y == current.y);
                            topright = next.x > current.x;
                        }
                        else
                        {
                            next = points.FirstOrDefault(_ => _.x == current.x && new int[] { current.y + 1, current.y - 1 }.Contains(_.y));
                            topright = next.y > current.y;
                        }
                        break;
                    default:
                        paths += current.c;
                        current.c = previous.c;
                        if (previous.x == current.x)
                            next = vertical(current);
                        else
                            next = horizontal(current);

                        break;
                }
                previous = current;
                current = next;
            }

            return type == "path" ? paths : steps.ToString();
        }

    }
}
