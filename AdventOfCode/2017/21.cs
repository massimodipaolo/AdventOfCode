using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2017
{
    class _21 : Puzzle
    {
        public _21()
        {
            ReadInputFromFile("2017/21.txt");
        }
        /*
--- Day 21: Fractal Art ---
You find a program trying to generate some art. It uses a strange process that involves repeatedly enhancing the detail of an image through a set of rules.

The image consists of a two-dimensional square grid of pixels that are either on (#) or off (.). The program always begins with this pattern:

.#.
..#
###
Because the pattern is both 3 pixels wide and 3 pixels tall, it is said to have a size of 3.

Then, the program repeats the following process:

If the size is evenly divisible by 2, break the pixels up into 2x2 squares, and convert each 2x2 square into a 3x3 square by following the corresponding enhancement rule.
Otherwise, the size is evenly divisible by 3; break the pixels up into 3x3 squares, and convert each 3x3 square into a 4x4 square by following the corresponding enhancement rule.
Because each square of pixels is replaced by a larger one, the image gains pixels and so its size increases.

The artist's book of enhancement rules is nearby (your puzzle input); however, it seems to be missing rules. The artist explains that sometimes, one must rotate or flip the input pattern to find a match.
(Never rotate or flip the output pattern, though.) 
Each pattern is written concisely: rows are listed as single units, ordered top-down, and separated by slashes. For example, the following rules correspond to the adjacent patterns:

../.#  =  ..
          .#

                .#.
.#./..#/###  =  ..#
                ###

                        #..#
#..#/..../#..#/.##.  =  ....
                        #..#
                        .##.
When searching for a rule to use, rotate and flip the pattern as necessary. For example, all of the following patterns match the same rule:

.#.   .#.   #..   ###
..#   #..   #.#   ..#
###   ###   ##.   .#.
Suppose the book contained the following two rules:

../.# => ##./#../...
.#./..#/### => #..#/..../..../#..#
As before, the program begins with this pattern:

.#.
..#
###
The size of the grid (3) is not divisible by 2, but it is divisible by 3. It divides evenly into a single square; the square matches the second rule, which produces:

#..#
....
....
#..#
The size of this enhanced grid (4) is evenly divisible by 2, so that rule is used. It divides evenly into four squares:

#.|.#
..|..
--+--
..|..
#.|.#
Each of these squares matches the same rule (../.# => ##./#../...), three of which require some flipping and rotation to line up with the rule. The output for the rule is the same in all four cases:

##.|##.
#..|#..
...|...
---+---
##.|##.
#..|#..
...|...
Finally, the squares are joined into a new grid:

##.##.
#..#..
......
##.##.
#..#..
......
Thus, after 2 iterations, the grid contains 12 pixels that are on.

How many pixels stay on after 5 iterations?         
             */
        public override string Output(string input)
        {
            /*
            input = @"../.# => ##./#../...
.#./..#/### => #..#/..../..../#..#";
            */
            IEnumerable<Rule> rules = input.Split("\n").Select(row => { var separator = row.Split(" => "); return new Rule() { inbound = separator[0], outboud = separator[1] }; });

            var f = new Fractal();
            f.points = new List<Point>();
            f.points.AddRange(".#./..#/###".ToPoints());

            for (var i=0;i<5;i++)
            {
                f.Divide();
                foreach (var sq in f.squares)
                {
                    var r = rules.FirstOrDefault(_ => _.inbound == sq.ToString());
                    if (r == null)
                        r = rules.FirstOrDefault(_ => _.inbound == sq.Rotate().ToString());
                    if (r == null)
                        r = rules.FirstOrDefault(_ => _.inbound == sq.Rotate().Rotate().ToString());
                    if (r == null)
                        r = rules.FirstOrDefault(_ => _.inbound == sq.Rotate().Rotate().Rotate().ToString());
                    if (r == null)
                        r = rules.FirstOrDefault(_ => _.inbound == sq.Rotate().Flip().ToString());
                    if (r == null)
                        r = rules.FirstOrDefault(_ => _.inbound == sq.Rotate().Rotate().Flip().ToString());                    
                    if (r == null)
                        r = rules.FirstOrDefault(_ => _.inbound == sq.Rotate().Rotate().Rotate().Flip().ToString());                    
                    if (r != null)
                    {
                        sq.points = r.outboud.ToPoints();
                    } else
                    {
                        throw new Exception();
                    }
                }
                f.Compose();
            }

            return f.ToString().Where(_ => _ == '#').Count().ToString();
        }

        public class Fractal
        {
            public List<Point> points { get; set; }
            public List<Fractal> squares { get; set; }
            public int size => (int)Math.Sqrt(points.Count);

            public void Divide()
            {
                int n = size % 2 == 0 ? 2 : 3;
                var _squares = new List<Fractal>();
                var sqs = points.GroupBy(_ => $"{_.x / n}-{_.y / n}" );                
                for (var j = 0; j < sqs.Count(); j++)
                {
                    var f = new Fractal();
                    f.points = new List<Point>();
                    f.points.AddRange(sqs.Skip(j).First());
                    _squares.Add(f);
                }
                squares = _squares;
            }

            public void Compose()
            {
                points = new List<Point>();                 
                int size = (int)Math.Sqrt(squares.Count());
                int capacity = squares[0].size;                

                for (var i=0;i <= size-1;i++)
                    for (var j = 0; j <= size-1; j++)
                    {
                        points.AddRange(squares[i + j].points.Select(_ => new Point() { x = _.x + (i * capacity), y = _.y + (j * capacity), v = _.v }).ToList());
                    }
                        
            }

            public override string ToString()
            {
                var s = string.Join("", points.OrderBy(_ => _.y).ThenBy(_ => _.x).Select((p, i) => (i + 1) % size == 0 && (i + 1) < size * size ? $"{p.v}/" : $"{p.v}"));
                return s;
            }
        }
        public class Point
        {
            public int x { get; set; }
            public int y { get; set; }
            public char v { get; set; }
        }

        public class Rule
        {
            public string inbound { get; set; }
            public string outboud { get; set; }
        }

        /*
--- Part Two ---
How many pixels stay on after 18 iterations?         
             */
        public override string Output2(string input)
        {
            List<Rule> rules = input.Split("\n").Select(row => { var separator = row.Split(" => "); return new Rule() { inbound = separator[0], outboud = separator[1] }; }).ToList();
            var rulesExt = new List<Rule>();
            foreach (var r in rules)
            {
                var _f = new Fractal();
                _f.points = r.inbound.ToPoints();
                rulesExt.Add(new Rule() { outboud = r.outboud, inbound = _f.Rotate().ToString() });
                rulesExt.Add(new Rule() { outboud = r.outboud, inbound = _f.Rotate().Rotate().ToString() });
                rulesExt.Add(new Rule() { outboud = r.outboud, inbound = _f.Rotate().Rotate().Rotate().ToString() });
                rulesExt.Add(new Rule() { outboud = r.outboud, inbound = _f.Rotate().Flip().ToString() });
                rulesExt.Add(new Rule() { outboud = r.outboud, inbound = _f.Rotate().Rotate().Flip().ToString() });
                rulesExt.Add(new Rule() { outboud = r.outboud, inbound = _f.Rotate().Rotate().Rotate().Flip().ToString() });
            }
            rules = rules.Concat(rulesExt.Distinct()).Distinct().ToList();

            var f = new Fractal();
            f.points = new List<Point>();
            f.points.AddRange(".#./..#/###".ToPoints());

            for (var i = 0; i < 18; i++)
            {                
                f.Divide();
                foreach (var sq in f.squares)
                {
                    var r = rules.FirstOrDefault(_ => _.inbound == sq.ToString());
                    if (r != null)
                    {
                        sq.points = r.outboud.ToPoints();
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                f.Compose();
                Console.WriteLine($"{i}) sq: {f.squares.Count()} p: {f.points.Count} {f.ToString().Where(_ => _ == '#').Count()}"); 
            }

            return f.ToString().Where(_ => _ == '#').Count().ToString();
        }
    }

    static class _extension
    {
        public static List<_21.Point> ToPoints(this string s)
        {
            var points = new List<_21.Point>();
            var rows = s.Split('/');
            for (var y = 0; y < rows.Length; y++)
                for (var x = 0; x < rows[y].Length; x++)
                    points.Add(new _21.Point() { x = x, y = y, v = rows[y][x] });

            return points;
        }

        public static _21.Fractal Rotate(this _21.Fractal f)
        {
            /*
.#.   .#.   #..   ###
..#   #..   #.#   ..#
###   ###   ##.   .#.

.#.   #..   
..#   #.#   
###   ##.   
             */
            var fR = new _21.Fractal();
            fR.points = new List<_21.Point>();
            int size = f.size;
            foreach (var p in f.points)
                fR.points.Add(new _21.Point() { x = (size - p.y - 1), y = p.x, v = p.v });

            return fR;
        }
        public static _21.Fractal Reverse(this _21.Fractal f)
        {
            var fR = new _21.Fractal();
            fR.points = new List<_21.Point>();
            int size = f.size;
            foreach (var p in f.points)
                fR.points.Add(new _21.Point() { x = size - p.x - 1, y = p.y, v = p.v });

            return fR;
        }
        public static _21.Fractal Flip(this _21.Fractal f)
        {
            var fF = new _21.Fractal();
            fF.points = new List<_21.Point>();
            int size = f.size;
            foreach (var p in f.points)
                fF.points.Add(new _21.Point() { x = p.x, y = size - p.y - 1, v = p.v });
            return fF;
        }
    }
}
