using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2017
{
    class _14 : Puzzle
    {
        public _14()
        {
            Input = "ffayrhll";
        }

        /*
--- Day 14: Disk Defragmentation ---
Suddenly, a scheduled job activates the system's disk defragmenter. Were the situation different, you might sit and watch it for a while, but today, you just don't have that kind of time. It's soaking up valuable system resources that are needed elsewhere, and so the only option is to help it finish its task as soon as possible.

The disk in question consists of a 128x128 grid; each square of the grid is either free or used. On this disk, the state of the grid is tracked by the bits in a sequence of knot hashes.

A total of 128 knot hashes are calculated, each corresponding to a single row in the grid; each hash contains 128 bits which correspond to individual grid squares. Each bit of a hash indicates whether that square is free (0) or used (1).

The hash inputs are a key string (your puzzle input), a dash, and a number from 0 to 127 corresponding to the row. 
For example, if your key string were flqrgnkx, then the first row would be given by the bits of the knot hash of flqrgnkx-0, the second row from the bits of the knot hash of flqrgnkx-1, and so on until the last row, flqrgnkx-127.

The output of a knot hash is traditionally represented by 32 hexadecimal digits; 
each of these digits correspond to 4 bits, for a total of 4 * 32 = 128 bits. 
To convert to bits, turn each hexadecimal digit to its equivalent binary value, high-bit first: 0 becomes 0000, 1 becomes 0001, e becomes 1110, f becomes 1111, and so on; 
a hash that begins with a0c2017... in hexadecimal would begin with 10100000110000100000000101110000... in binary.

Continuing this process, the first 8 rows and columns for key flqrgnkx appear as follows, using # to denote used squares, and . to denote free ones:

##.#.#..-->
.#.#.#.#   
....#.#.   
#.#.##.#   
.##.#...   
##..#..#   
.#...#..   
##.#.##.-->
|      |   
V      V   
In this example, 8108 squares are used across the entire 128x128 grid.

Given your actual key string, how many squares are used?         
             */
        public override string Output(string input)
        {
            //input = "flqrgnkx";                      
            return Enumerable.Range(0, 128)
                .Sum(i => HexToBinary($"{input}-{i}".KnotHash()).Count(_ => _ == '1'))  //KnotHash in Extensions/KnotHash
                .ToString();
        }

        /*
--- Part Two ---
Now, all the defragmenter needs to know is the number of regions. A region is a group of used squares that are all adjacent, not including diagonals. Every used square is in exactly one region: lone used squares form their own isolated regions, while several adjacent squares all count as a single region.

In the example above, the following nine regions are visible, each marked with a distinct digit:

11.2.3..-->
.1.2.3.4   
....5.6.   
7.8.55.9   
.88.5...   
88..5..8   
.8...8..   
88.8.88.-->
|      |   
V      V   
Of particular interest is the region marked 8; while it does not appear contiguous in this small view, all of the squares marked 8 are connected when considering the whole 128x128 grid. In total, in this example, 1242 regions are present.

How many regions are present given your key string?         
             */

        static IList<Fragment> grid;

        public override string Output2(string input)
        {
            //input = "flqrgnkx";        
            int size = 128;
            grid = Enumerable.Range(0, size)
                .SelectMany(y => HexToBinary($"{input}-{y}".KnotHash()).Take(size).Select((c, i) => new Fragment() { X = i, Y = y, Free = c == '1' }))
                .ToList();

            int group = 0;
            for (var i = 0; i < size * size; i++)
            {                
                Fragment f = grid[i];                                
                if (f.Free && !f.Skip)
                {
                    ++group;
                    var _contiguous = contiguous(f);
                    if (_contiguous.Any())
                    {
                        //Console.WriteLine($"{i}) {f.X},{f.Y} ; c:{_contiguous.Count}");                        
                        foreach (var c in _contiguous)
                        {
                            var g = grid.Single(_ => _.X == c.X && _.Y == c.Y);
                            g.Group = group;                            
                        }   
                    }                       
                    f.Group = group;
                }
            }                
            
            return group.ToString(); 
        }

        
        private IList<Fragment> contiguous(Fragment f)
        {
            f.Skip = true;
            var _c = grid.Where(_ => ((_.X == f.X - 1 && _.Y == f.Y) || (_.X == f.X + 1 && _.Y == f.Y) || (_.X == f.X && _.Y == f.Y - 1) || (_.X == f.X && _.Y == f.Y + 1)) && _.Free && !_.Skip).ToList();
            for (var i = 0; i < _c.Count; i++)
                _c.AddRange(contiguous(_c[i]).Where(_ => !_c.Contains(_)).Distinct());         
            return _c.Distinct().ToList();
        }
        

        public class Fragment
        {
            public int X { get; set; }
            public int Y { get; set; }
            public bool Free { get; set; }
            public int Group { get; set; }
            public bool Skip { get; set; } = false;
        }

        private string HexToBinary(string hex)
        {
            return hex.Aggregate("", (res, s) => res += Convert.ToString(Convert.ToUInt32(s.ToString(), 16), 2).PadLeft(4, '0'));
        }
    }
}
