using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.y2005
{
    public class _2 : Puzzle
    {
        public _2()
        {
            ReadInputFromFile("2005/02.txt");
        }

        /*
--- Day 2: I Was Told There Would Be No Math ---
The elves are running low on wrapping paper, and so they need to submit an order for more. They have a list of the dimensions (length l, width w, and height h) of each present, and only want to order exactly as much as they need.

Fortunately, every present is a box (a perfect right rectangular prism), which makes calculating the required wrapping paper for each gift a little easier: find the surface area of the box, which is 2*l*w + 2*w*h + 2*h*l. The elves also need a little extra paper for each present: the area of the smallest side.

For example:

A present with dimensions 2x3x4 requires 2*6 + 2*12 + 2*8 = 52 square feet of wrapping paper plus 6 square feet of slack, for a total of 58 square feet.
A present with dimensions 1x1x10 requires 2*1 + 2*10 + 2*10 = 42 square feet of wrapping paper plus 1 square foot of slack, for a total of 43 square feet.
All numbers in the elves' list are in feet. How many total square feet of wrapping paper should they order?         
             */
        public override string Output(string input)
        {
            int paper = 0;
            foreach (var box in input.Split("\n"))
            {
                var dim = box.Replace("\r", "").Trim().Split('x');
                int l = int.Parse(dim[0].ToString());
                int w = int.Parse(dim[1].ToString());
                int h = int.Parse(dim[2].ToString());
                var s1 = l * w;
                var s2 = l * h;
                var s3 = w * h;
                var sMin = Math.Min(Math.Min(s1, s2), s3);
                paper += sMin + 2 * (s1 + s2 + s3);
            }
            return paper.ToString();
        }

        /*
  --- Part Two ---
The elves are also running low on ribbon. Ribbon is all the same width, so they only have to worry about the length they need to order, which they would again like to be exact.

The ribbon required to wrap a present is the shortest distance around its sides, or the smallest perimeter of any one face. 
Each present also requires a bow made out of ribbon as well; the feet of ribbon required for the perfect bow is equal to the cubic feet of volume of the present. 
Don't ask how they tie the bow, though; they'll never tell.

For example:

A present with dimensions 2x3x4 requires 2+2+3+3 = 10 feet of ribbon to wrap the present plus 2*3*4 = 24 feet of ribbon for the bow, for a total of 34 feet.
A present with dimensions 1x1x10 requires 1+1+1+1 = 4 feet of ribbon to wrap the present plus 1*1*10 = 10 feet of ribbon for the bow, for a total of 14 feet.
How many total feet of ribbon should they order?       
             */
        public override string Output2(string input)
        {
            int ribbon = 0;
            foreach (var box in input.Split("\n"))
            {
                var dim = box.Replace("\r", "").Trim().Split('x');
                var dims = new List<int>();
                foreach (var d in dim)
                    dims.Add(int.Parse(d.ToString()));

                ribbon += 2 * dims.OrderBy(_ => _).Take(2).Sum() + dims.Multiply();
            }
            return ribbon.ToString();
        }
    }
}