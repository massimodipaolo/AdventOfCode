using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode._2017
{
    public class _24 : Puzzle
    {
        public _24()
        {
            ReadInputFromFile("2017/24.txt");
        }

        /*
--- Day 24: Electromagnetic Moat ---
The CPU itself is a large, black building surrounded by a bottomless pit. Enormous metal tubes extend outward from the side of the building at regular intervals and descend down into the void. There's no way to cross, but you need to get inside.

No way, of course, other than building a bridge out of the magnetic components strewn about nearby.

Each component has two ports, one on each end. The ports come in all different types, and only matching types can be connected. You take an inventory of the components by their port types (your puzzle input). Each port is identified by the number of pins it uses; more pins mean a stronger connection for your bridge. A 3/7 component, for example, has a type-3 port on one side, and a type-7 port on the other.

Your side of the pit is metallic; a perfect surface to connect a magnetic, zero-pin port. Because of this, the first port you use must be of type 0. It doesn't matter what type of port you end with; your goal is just to make the bridge as strong as possible.

The strength of a bridge is the sum of the port types in each component. For example, if your bridge is made of components 0/3, 3/7, and 7/4, your bridge has a strength of 0+3 + 3+7 + 7+4 = 24.

For example, suppose you had the following components:

0/2
2/2
2/3
3/4
3/5
0/1
10/1
9/10
With them, you could make the following valid bridges:

0/1
0/1--10/1
0/1--10/1--9/10
0/2
0/2--2/3
0/2--2/3--3/4
0/2--2/3--3/5
0/2--2/2
0/2--2/2--2/3
0/2--2/2--2/3--3/4
0/2--2/2--2/3--3/5
(Note how, as shown by 10/1, order of ports within a component doesn't matter. However, you may only use each port on a component once.)

Of these bridges, the strongest one is 0/1--10/1--9/10; it has a strength of 0+1 + 1+10 + 10+9 = 31.

What is the strength of the strongest bridge you can make with the components you have available?
        */

        public static List<Pin> pins { get; set; }
        public static List<Path> paths { get; set; }

        public override string Output(string input)
        {
            /*
            input = @"0/2
2/2
2/3
3/4
3/5
0/1
10/1
9/10";
*/
            pins = new List<Pin>();
            pins.AddRange(input.Split("\n")
                          .Select(row => { var slash = row.Split('/'); return new Pin() { p1 = int.Parse(slash[0]), p2 = int.Parse(slash[1]) }; }));

            paths = new List<Path>();

            foreach (var pin in pins.Where(_ => _.p1 == 0))
            {
                var _list = new List<Pin>(); _list.Add(pin);
                paths.Add(new Path(_list));
            }

            Find();

            return paths.Max(_ => _.Strength).ToString();
        }

        /*
--- Part Two ---
The bridge you've built isn't long enough; you can't jump the rest of the way.

In the example above, there are two longest bridges:

0/2--2/2--2/3--3/4
0/2--2/2--2/3--3/5
Of them, the one which uses the 3/5 component is stronger; its strength is 0+2 + 2+2 + 2+3 + 3+5 = 19.

What is the strength of the longest bridge you can make? If you can make multiple bridges of the longest length, pick the strongest one.
        */
        public override string Output2(string input)
        {
            pins = new List<Pin>();
            pins.AddRange(input.Split("\n")
                          .Select(row => { var slash = row.Split('/'); return new Pin() { p1 = int.Parse(slash[0]), p2 = int.Parse(slash[1]) }; }));

            paths = new List<Path>();

            foreach (var pin in pins.Where(_ => _.p1 == 0))
            {
                var _list = new List<Pin>(); _list.Add(pin);
                paths.Add(new Path(_list));
            }

            Find();

            var longest = paths.OrderByDescending(_ => _.Pins.Count()).First().Pins.Count;
            return paths.Where(_ => longest == _.Pins.Count()).Max(_ => _.Strength).ToString();
        }


        public void Find()
        {
            while (true)
            {
                var _todo = paths.Where(_ => !_.Worked).ToList();
                for (var i = 0; i < _todo.Count(); i++)
                {
                    var path = _todo[i];
                    path.Worked = true;
                    var nexts = pins.Except(path.Pins).Where(_ => path.Connector == _.p1 || path.Connector == _.p2);
                    foreach (var pin in nexts)
                    {
                        var newPins = new List<Pin>();
                        newPins.AddRange(path.Pins);
                        newPins.Add(pin);
                        paths.Add(new Path(newPins));
                    }
                }
                if (!_todo.Any()) break;
            }
        }

        public class Path
        {
            public List<Pin> Pins { get; set; }
            public int Connector { get; set; }
            public bool Worked { get; set; } = false;
            public int Strength => Pins.Sum(_ => _.p1 + _.p2);

            public Path(IEnumerable<Pin> pins)
            {
                Pins = pins.ToList();
                var last = pins.Last();
                var prevPin = pins.SkipLast(1).LastOrDefault();
                if (prevPin != null)
                    Connector = (last.p1 == prevPin.p1 || last.p1 == prevPin.p2) ? last.p2 : last.p1;
                else
                    Connector = last.p2;
            }

            public override string ToString()
            {
                return string.Format($"{string.Join("--", Pins.Select(p => $"{p.p1}/{p.p2}"))} :{Strength}");
            }


        }

        public class Pin
        {
            public int p1 { get; set; }
            public int p2 { get; set; }
        }

    }
}
