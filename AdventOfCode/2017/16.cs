using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode._2017
{
    class _16 : Puzzle
    {
        public _16()
        {
            ReadInputFromFile("2017/16.txt");
        }

        /*
--- Day 16: Permutation Promenade ---
You come upon a very unusual sight; a group of programs here appear to be dancing.

There are sixteen programs in total, named a through p. They start by standing in a line: a stands in position 0, b stands in position 1, and so on until p, which stands in position 15.

The programs' dance consists of a sequence of dance moves:

Spin, written sX, makes X programs move from the end to the front, but maintain their order otherwise. (For example, s3 on abcde produces cdeab).
Exchange, written xA/B, makes the programs at positions A and B swap places.
Partner, written pA/B, makes the programs named A and B swap places.
For example, with only five programs standing in a line (abcde), they could do the following dance:

s1, a spin of size 1: eabcd.
x3/4, swapping the last two programs: eabdc.
pe/b, swapping programs e and b: baedc.
After finishing their dance, the programs end up in order baedc.

You watch the dance for a while and record their dance moves (your puzzle input). In what order are the programs standing after their dance?         
             */
        static string sequence = "abcdefghijklmnop";
        static int length => sequence.Length;
        public override string Output(string input)
        {
            //input = "s1,x3/4,pe/b";
            var moves = input.Split(',');

            for (var i = 0; i < moves.Length; i++)
                Dance(moves[i]);

            return sequence;
        }

        /// <summary>
        /// Parser
        /// </summary>
        /// <param name="move"></param>
        private void Dance(string move)
        {
            switch (move.Substring(0, 1))
            {
                case "s":
                    Spin(int.Parse(move.Substring(1)));
                    break;
                case "x":
                    var positions = move.Split("/");
                    Exchange(int.Parse(positions[0].Replace("x", "")), int.Parse(positions[1]));
                    break;
                case "p":
                    var chars = move.Split("/");
                    Partner(chars[0][1], chars[1][0]);
                    break;
            }
        }

        /// <summary>
        /// Spin, written sX, makes X programs move from the end to the front, but maintain their order otherwise. (For example, s3 on abcde produces cdeab).
        /// </summary>
        /// <param name="x"></param>
        private void Spin(int x)
        {
            sequence = new string(sequence.Reverse().Take(x).Reverse().Concat(sequence.Take(length - x)).ToArray());
        }

        /// <summary>
        /// Exchange, written xA/B, makes the programs at positions A and B swap places.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        private void Exchange(int a, int b)
        {
            List<char> _sequence = new List<char>(length);
            for (var i = 0; i < length; i++)
                _sequence.Add(i == a ? sequence[b] : (i == b ? sequence[a] : sequence[i]));
            sequence = new string(_sequence.ToArray());
        }

        /// <summary>
        /// Partner, written pA/B, makes the programs named A and B swap places.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        private void Partner(char a, char b)
        {
            List<char> _sequence = new List<char>(length);
            for (var i = 0; i < length; i++)
                _sequence.Add(sequence[i] == a ? b : (sequence[i] == b ? a : sequence[i]));
            sequence = new string(_sequence.ToArray());
        }

        /*
--- Part Two ---
Now that you're starting to get a feel for the dance moves, you turn your attention to the dance as a whole.

Keeping the positions they ended up in from their previous dance, the programs perform it again and again: including the first dance, a total of one billion (1000000000) times.

In the example above, their second dance would begin with the order baedc, and use the same dance moves:

s1, a spin of size 1: cbaed.
x3/4, swapping the last two programs: cbade.
pe/b, swapping programs e and b: ceadb.
In what order are the programs standing after their billion dances?         
             */
        public override string Output2(string input)
        {
            var moves = input.Split(',');
            var repeat = 1000000000;
            var results = new string[repeat];
            
            for (var i = 0; i < repeat; i++)
            {
                results[i] = sequence;

                for (var j = 0; j < moves.Length; j++)
                    Dance(moves[j]);

                if (sequence == results[0])
                {                    
                    sequence = results[repeat % (i + 1)];
                    break;
                }
            }

            return sequence;
        }
    }
}
