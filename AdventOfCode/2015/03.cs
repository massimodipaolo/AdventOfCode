﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode._2015
{
    public class _03 : Puzzle
    {
        public _03()
        {
            ReadInputFromFile("2015/03.txt");
        }

        /*
--- Day 3: Perfectly Spherical Houses in a Vacuum ---
Santa is delivering presents to an infinite two-dimensional grid of houses.

He begins by delivering a present to the house at his starting location, and then an elf at the North Pole calls him via radio and tells him where to move next. Moves are always exactly one house to the north (^), south (v), east (>), or west (<). After each move, he delivers another present to the house at his new location.

However, the elf back at the north pole has had a little too much eggnog, and so his directions are a little off, and Santa ends up visiting some houses more than once. How many houses receive at least one present?

For example:

> delivers presents to 2 houses: one at the starting location, and one to the east.
^>v< delivers presents to 4 houses in a square, including twice to the house at his starting/ending location.
^v^v^v^v^v delivers a bunch of presents to some very lucky children at only 2 houses.         
             */
        public override string Output(string input)
        {
            int x = 0; int y = 0;
            var houses = new Dictionary<string, bool>();
            houses[$"{x}-{y}"] = true;
            foreach (char move in input)
            {
                switch (move)
                {
                    case '^':
                        y++;
                        break;
                    case '>':
                        x++;
                        break;
                    case 'v':
                        y--;
                        break;
                    case '<':
                        x--;
                        break;
                }
                houses[$"{x}-{y}"] = true;
            }
            return houses.Count.ToString();
        }

        /*
         --- Part Two ---
        The next year, to speed up the process, Santa creates a robot version of himself, Robo-Santa, to deliver presents with him.

        Santa and Robo-Santa start at the same location (delivering two presents to the same starting house), then take turns moving based on instructions from the elf, who is eggnoggedly reading from the same script as the previous year.

        This year, how many houses receive at least one present?

        For example:

        ^v delivers presents to 3 houses, because Santa goes north, and then Robo-Santa goes south.
        ^>v< now delivers presents to 3 houses, and Santa and Robo-Santa end up back where they started.
        ^v^v^v^v^v now delivers presents to 11 houses, with Santa going one direction and Robo-Santa going the other.
                     */
        public override string Output2(string input)
        {
            int x1 = 0; int y1 = 0;
            int x2 = 0; int y2 = 0;
            var houses = new Dictionary<string, bool>();
            houses[$"{x1}-{y1}"] = true;
            var i = 0;
            foreach (char move in input)
            {
                i++;
                switch (move)
                {
                    case '^':
                        if (i % 2 == 0) y1++; else y2++;
                        break;
                    case '>':
                        if (i % 2 == 0) x1++; else x2++;
                        break;
                    case 'v':
                        if (i % 2 == 0) y1--; else y2--;
                        break;
                    case '<':
                        if (i % 2 == 0) x1--; else x2--;
                        break;
                }
                if (i % 2 == 0) houses[$"{x1}-{y1}"] = true; else houses[$"{x2}-{y2}"] = true;
            }
            return houses.Count.ToString();
        }
    }


}
