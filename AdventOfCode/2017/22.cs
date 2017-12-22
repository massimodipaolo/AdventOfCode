using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2017
{
    class _22 : Puzzle
    {
        public _22()
        {
            ReadInputFromFile("2017/22.txt");
        }

        /*
--- Day 22: Sporifica Virus ---
Diagnostics indicate that the local grid computing cluster has been contaminated with the Sporifica Virus. 
The grid computing cluster is a seemingly-infinite two-dimensional grid of compute nodes. Each node is either clean or infected by the virus.

To prevent overloading the nodes (which would render them useless to the virus) or detection by system administrators, exactly one virus carrier moves through the network, infecting or cleaning nodes as it moves. 
The virus carrier is always located on a single node in the network (the current node) and keeps track of the direction it is facing.

To avoid detection, the virus carrier works in bursts; in each burst, it wakes up, does some work, and goes back to sleep. The following steps are all executed in order one time each burst:

If the current node is infected, it turns to its right. Otherwise, it turns to its left. (Turning is done in-place; the current node does not change.)
If the current node is clean, it becomes infected. Otherwise, it becomes cleaned. (This is done after the node is considered for the purposes of changing direction.)
The virus carrier moves forward one node in the direction it is facing.
Diagnostics have also provided a map of the node infection status (your puzzle input). 
Clean nodes are shown as .; infected nodes are shown as #. 
This map only shows the center of the grid; there are many more nodes beyond those shown, but none of them are currently infected.

The virus carrier begins in the middle of the map facing up.

For example, suppose you are given a map like this:

..#
#..
...
Then, the middle of the infinite grid looks like this, with the virus carrier's position marked with [ ]:

. . . . . . . . .
. . . . . . . . .
. . . . . . . . .
. . . . . # . . .
. . . #[.]. . . .
. . . . . . . . .
. . . . . . . . .
. . . . . . . . .
The virus carrier is on a clean node, so it turns left, infects the node, and moves left:

. . . . . . . . .
. . . . . . . . .
. . . . . . . . .
. . . . . # . . .
. . .[#]# . . . .
. . . . . . . . .
. . . . . . . . .
. . . . . . . . .
The virus carrier is on an infected node, so it turns right, cleans the node, and moves up:

. . . . . . . . .
. . . . . . . . .
. . . . . . . . .
. . .[.]. # . . .
. . . . # . . . .
. . . . . . . . .
. . . . . . . . .
. . . . . . . . .
Four times in a row, the virus carrier finds a clean, infects it, turns left, and moves forward, ending in the same place and still facing up:

. . . . . . . . .
. . . . . . . . .
. . . . . . . . .
. . #[#]. # . . .
. . # # # . . . .
. . . . . . . . .
. . . . . . . . .
. . . . . . . . .
Now on the same node as before, it sees an infection, which causes it to turn right, clean the node, and move forward:

. . . . . . . . .
. . . . . . . . .
. . . . . . . . .
. . # .[.]# . . .
. . # # # . . . .
. . . . . . . . .
. . . . . . . . .
. . . . . . . . .
After the above actions, a total of 7 bursts of activity had taken place. Of them, 5 bursts of activity caused an infection.

After a total of 70, the grid looks like this, with the virus carrier facing up:

. . . . . # # . .
. . . . # . . # .
. . . # . . . . #
. . # . #[.]. . #
. . # . # . . # .
. . . . . # # . .
. . . . . . . . .
. . . . . . . . .
By this time, 41 bursts of activity caused an infection (though most of those nodes have since been cleaned).

After a total of 10000 bursts of activity, 5587 bursts will have caused an infection.

Given your actual map, after 10000 bursts of activity, how many bursts cause a node to become infected? (Do not count nodes that begin infected.)         
             */
        public override string Output(string input)
        {
            /*
            input = @"..#
#..
...";
*/
            int iterations = 10000;
            var points = new List<Point>();
            var rows = input.Split("\n");
            var bound = (int)Math.Floor((double)rows.Length / 2);
            for (var _y = -bound; _y <= bound; _y++)
                for (var _x = -bound; _x <= bound; _x++)
                    points.Add(new Point() { x = _x, y = _y, v = rows[_y + bound][_x + bound] == '#' ? 'I' : 'C' });

            var x = 0; var y = 0; var vector = "T";
            var bursts = 0;
            for (var i = 0; i < iterations; i++)
            {
                var p = points.SingleOrDefault(_ => _.x == x && _.y == y);
                if (p == null)
                {
                    p = new Point() { x = x, y = y, v = 'C' };
                    points.Add(p);
                }

                if (p.v == 'I')
                    switch (vector)
                    {
                        case "T":
                            vector = "R";
                            x++;
                            break;
                        case "R":
                            vector = "D";
                            y++;
                            break;
                        case "D":
                            vector = "L";
                            x--;
                            break;
                        case "L":
                            vector = "T";
                            y--;
                            break;
                    }
                else
                    switch (vector)
                    {
                        case "T":
                            vector = "L";
                            x--;
                            break;
                        case "L":
                            vector = "D";
                            y++;
                            break;
                        case "D":
                            vector = "R";
                            x++;
                            break;
                        case "R":
                            vector = "T";
                            y--;
                            break;
                    }

                p.v = p.v == 'I' ? 'C' : 'I';
                if (p.v == 'I') bursts++;
            }

            return bursts.ToString();
        }

        /*
--- Part Two ---
As you go to remove the virus from the infected nodes, it evolves to resist your attempt.

Now, before it infects a clean node, it will weaken it to disable your defenses. If it encounters an infected node, it will instead flag the node to be cleaned in the future. So:

Clean nodes become weakened.
Weakened nodes become infected.
Infected nodes become flagged.
Flagged nodes become clean.
Every node is always in exactly one of the above states.

The virus carrier still functions in a similar way, but now uses the following logic during its bursts of action:

Decide which way to turn based on the current node:
If it is clean, it turns left.
If it is weakened, it does not turn, and will continue moving in the same direction.
If it is infected, it turns right.
If it is flagged, it reverses direction, and will go back the way it came.
Modify the state of the current node, as described above.
The virus carrier moves forward one node in the direction it is facing.
Start with the same map (still using . for clean and # for infected) and still with the virus carrier starting in the middle and facing up.

Using the same initial state as the previous example, and drawing weakened as W and flagged as F, the middle of the infinite grid looks like this, with the virus carrier's position again marked with [ ]:

. . . . . . . . .
. . . . . . . . .
. . . . . . . . .
. . . . . # . . .
. . . #[.]. . . .
. . . . . . . . .
. . . . . . . . .
. . . . . . . . .
This is the same as before, since no initial nodes are weakened or flagged. The virus carrier is on a clean node, so it still turns left, instead weakens the node, and moves left:

. . . . . . . . .
. . . . . . . . .
. . . . . . . . .
. . . . . # . . .
. . .[#]W . . . .
. . . . . . . . .
. . . . . . . . .
. . . . . . . . .
The virus carrier is on an infected node, so it still turns right, instead flags the node, and moves up:

. . . . . . . . .
. . . . . . . . .
. . . . . . . . .
. . .[.]. # . . .
. . . F W . . . .
. . . . . . . . .
. . . . . . . . .
. . . . . . . . .
This process repeats three more times, ending on the previously-flagged node and facing right:

. . . . . . . . .
. . . . . . . . .
. . . . . . . . .
. . W W . # . . .
. . W[F]W . . . .
. . . . . . . . .
. . . . . . . . .
. . . . . . . . .
Finding a flagged node, it reverses direction and cleans the node:

. . . . . . . . .
. . . . . . . . .
. . . . . . . . .
. . W W . # . . .
. .[W]. W . . . .
. . . . . . . . .
. . . . . . . . .
. . . . . . . . .
The weakened node becomes infected, and it continues in the same direction:

. . . . . . . . .
. . . . . . . . .
. . . . . . . . .
. . W W . # . . .
.[.]# . W . . . .
. . . . . . . . .
. . . . . . . . .
. . . . . . . . .
Of the first 100 bursts, 26 will result in infection. Unfortunately, another feature of this evolved virus is speed; of the first 10000000 bursts, 2511944 will result in infection.

Given your actual map, after 10000000 bursts of activity, how many bursts cause a node to become infected? (Do not count nodes that begin infected.)         
         */
        public override string Output2(string input)
        {

            input = @"..#
#..
...";
                        
            var points = new List<Point>();            
            var rows = input.Split("\r\n");
            var bound = (int)Math.Floor((double)rows.Length / 2);
            for (var _y = -bound; _y <= bound; _y++)
                for (var _x = -bound; _x <= bound; _x++)
                    points.Add(new Point() { x = _x, y = _y, v = rows[_y + bound][_x + bound] == '#' ? 'I' : 'C' });

            var x = 0; var y = 0; var vector = "T";
            var bursts = 0;
            //while (bursts <= 100 /*10000000*/)
            for (var i = 0;i < 10000000; i++/*10000000*/)
                {
                var p = points.SingleOrDefault(_ => _.x == x && _.y == y);
                if (p == null)
                {
                    p = new Point() { x = x, y = y, v = 'C' };
                    points.Add(p);
                }                

                if (p.v == 'I') // right
                    switch (vector)
                    {
                        case "T":
                            vector = "R";
                            x++;
                            break;
                        case "R":
                            vector = "D";
                            y++;
                            break;
                        case "D":
                            vector = "L";
                            x--;
                            break;
                        case "L":
                            vector = "T";
                            y--;
                            break;
                    }
                else if (p.v == 'C') // left
                    switch (vector)
                    {
                        case "T":
                            vector = "L";
                            x--;
                            break;
                        case "L":
                            vector = "D";
                            y++;
                            break;
                        case "D":
                            vector = "R";
                            x++;
                            break;
                        case "R":
                            vector = "T";
                            y--;
                            break;
                    }
                else if (p.v == 'W') // no turn
                    switch (vector)
                    {
                        case "T":
                            y--;
                            break;
                        case "L":
                            x--;
                            break;
                        case "D":
                            y++;
                            break;
                        case "R":
                            x++;
                            break;
                    }
                else if (p.v == 'F') // reverse
                    switch (vector)
                    {
                        case "T":
                            vector = "D";
                            y++;
                            break;
                        case "L":
                            vector = "R";
                            x++;
                            break;
                        case "D":
                            vector = "T";
                            y--;
                            break;
                        case "R":
                            vector = "L";
                            x--;
                            break;
                    }

                //change state
                switch (p.v)
                {
                    case 'C':
                        p.v = 'W';
                        break;
                    case 'W':
                        p.v = 'I';
                        break;
                    case 'I':
                        p.v = 'F';
                        break;
                    case 'F':
                        p.v = 'C';
                        break;

                }
                if (p.v == 'I') bursts++;

            }

            //return points.Count(_ => _.v == 'I').ToString();
            return bursts.ToString();
        }

        public class Point
        {
            public int x { get; set; }
            public int y { get; set; }
            public char v { get; set; }
        }
    }
}
