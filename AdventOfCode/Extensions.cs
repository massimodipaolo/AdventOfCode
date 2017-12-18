using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
    public static class Extensions
    {
        public static int Multiply(this IEnumerable<int> list)
        {
            var res = 1;
            foreach (int n in list)
                res *= n;
            return res;
        }
        public static int XOR(this IEnumerable<int> list)
        {
            var res = 0;
            foreach (int n in list)
                res = res ^ n;
            return res;
        }

        public static string KnotHash(this string input,int numbers = 256)
        {
            var ascii = input.Aggregate("", (res, c) => res + ((byte)c) + ",") + "17,31,73,47,23";
            IEnumerable<int> lengths = ascii.Split(',').Select(_ => int.Parse(_));

            var list = Enumerable.Range(0, numbers).ToArray();
            int position = 0;
            int skip = 0;

            for (var round = 1; round <= 64; round++)
                foreach (var length in lengths)
                {
                    var selected = list.Select((n, i) => new { n, i = (i - position + numbers) % numbers })
                                       .Where(_ => _.i < length)
                                       .OrderBy(_ => _.i)
                                       .Select(_ => _.n).ToList();
                    var reverse = selected.ToArray().Reverse().ToArray();
                    for (var j = 0; j < list.Length; j++)
                    {
                        var jj = selected.IndexOf(list[j]);
                        if (jj >= 0)
                        {
                            list[j] = reverse[jj];
                        }
                    }

                    position = (position + length + skip) % numbers;
                    skip++;
                }

            //Console.WriteLine(String.Join(",", list));

            var hash = list
                .Select((n, i) => new { n, i })
                .GroupBy(_ => _.i / 16) //dense
                .Select(_ => _.Select(__ => __.n).XOR()) //xor
                .Select(_ => Convert.ToString(_, 16).PadLeft(2, '0')) //hex
                .Aggregate("", (res, s) => res += s); //32 digits

            return hash;
        }
    }
}
