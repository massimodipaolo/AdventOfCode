using System;
using System.Collections.Generic;
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
    }
}
