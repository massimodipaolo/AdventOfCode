using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode._2017
{
    class _07 : Puzzle
    {
        public _07()
        {
            ReadInputFromFile("2017/07.txt");
        }

        /*
--- Day 7: Recursive Circus ---
Wandering further through the circuits of the computer, you come upon a tower of programs that have gotten themselves into a bit of trouble. A recursive algorithm has gotten out of hand, and now they're balanced precariously in a large tower.

One program at the bottom supports the entire tower. It's holding a large disc, and on the disc are balanced several more sub-towers. At the bottom of these sub-towers, standing on the bottom disc, are other programs, each holding their own disc, and so on. At the very tops of these sub-sub-sub-...-towers, many programs stand simply keeping the disc below them balanced but with no disc of their own.

You offer to help, but first you need to understand the structure of these towers. You ask each program to yell out their name, their weight, and (if they're holding a disc) the names of the programs immediately above them balancing on that disc. You write this information down (your puzzle input). Unfortunately, in their panic, they don't do this in an orderly fashion; by the time you're done, you're not sure which program gave which information.

For example, if your list is the following:

pbga (66)
xhth (57)
ebii (61)
havc (66)
ktlj (57)
fwft (72) -> ktlj, cntj, xhth
qoyq (66)
padx (45) -> pbga, havc, qoyq
tknk (41) -> ugml, padx, fwft
jptl (61)
ugml (68) -> gyxo, ebii, jptl
gyxo (61)
cntj (57)
...then you would be able to recreate the structure of the towers that looks like this:

                gyxo
              /     
         ugml - ebii
       /      \     
      |         jptl
      |        
      |         pbga
     /        /
tknk --- padx - havc
     \        \
      |         qoyq
      |             
      |         ktlj
       \      /     
         fwft - cntj
              \     
                xhth
In this example, tknk is at the bottom of the tower (the bottom program), and is holding up ugml, padx, and fwft. Those programs are, in turn, holding up other programs; in this example, none of those programs are holding up any other programs, and are all the tops of their own towers. (The actual tower balancing in front of you is much larger.)

Before you're ready to help them, you need to make sure your information is correct. What is the name of the bottom program?         
             */
        public override string Output(string input)
        {
            var towers = input.Split("\n").Where(_ => _.Contains("->")).Select(_ =>
            {
                var split = _.Split("(");
                var node = split[0].Trim();
                var children = split[1].Split("->")[1].Replace(" ","").Replace(",","|");
                return new { node, children };
            }
            );
            var childrens = "|" + string.Join("|", towers.Select(_ => _.children)) + "|";
            var root = towers.FirstOrDefault(_ => !childrens.Contains($"|{_.node}|")).node;
            return root;
        }

        /*
--- Part Two ---
The programs explain the situation: they can't get down. Rather, they could get down, if they weren't expending all of their energy trying to keep the tower balanced. 
Apparently, one program has the wrong weight, and until it's fixed, they're stuck here.

For any program holding a disc, each program standing on that disc forms a sub-tower. Each of those sub-towers are supposed to be the same weight, or the disc itself isn't balanced. 
The weight of a tower is the sum of the weights of the programs in that tower.

In the example above, this means that for ugml's disc to be balanced, gyxo, ebii, and jptl must all have the same weight, and they do: 61.

However, for tknk to be balanced, each of the programs standing on its disc and all programs above it must each match. This means that the following sums must all be the same:

ugml + (gyxo + ebii + jptl) = 68 + (61 + 61 + 61) = 251
padx + (pbga + havc + qoyq) = 45 + (66 + 66 + 66) = 243
fwft + (ktlj + cntj + xhth) = 72 + (57 + 57 + 57) = 243
As you can see, tknk's disc is unbalanced: ugml's stack is heavier than the other two. 
Even though the nodes above ugml are balanced, ugml itself is too heavy: it needs to be 8 units lighter for its stack to weigh 243 and keep the towers balanced. 
If this change were made, its weight would be 60.

Given that exactly one program is the wrong weight, what would its weight need to be to balance the entire tower?         
             */
        public override string Output2(string input)
        {   
            var rows = input.Split("\n");
            var towers = rows.Select(_ =>
            {
                var separator = _.Split("->");
                var left = separator[0].Trim().Split("(");
                string right = separator.Length>1 ? separator[1] : null;
                return new Tower() {
                    Name = left[0].Trim(),
                    Weight = int.Parse(left[1].Replace(")", "").Trim()),
                    Childrens = !string.IsNullOrEmpty(right) ? right.Split(",").Select(__ => __.Trim()) : null
                };                
            }).ToArray();

            string rootName = Output(input);

            calcWeight(rootName, ref towers);

            var tree = towers.Single(_ => _.Name == rootName)
                .Childrens
                .Select(node => towers.Single(_ => _.Name == node));
            var heaviest = tree.OrderByDescending(_ => _.Weight + _.ChildrensWeight).First();
            var diff = (heaviest.Weight + heaviest.ChildrensWeight) - tree.Min(_ => _.Weight + _.ChildrensWeight);

            while (true)
            {
                if (heaviest.Childrens == null) break;
                var _tmp = heaviest.Childrens.Select(node => towers.Single(_ => _.Name == node));
                heaviest = _tmp.OrderByDescending(_ => _.Weight + _.ChildrensWeight).First();
            }            
            
            return (heaviest.Weight - diff).ToString();
        }

        private int calcWeight(string nodeName, ref Tower[] towers)
        {
            Tower tower = towers.Single(_ => _.Name == nodeName);
            if (tower.Childrens != null)            
                foreach (var node in tower.Childrens)               
                    tower.ChildrensWeight += calcWeight(node, ref towers);                            
            return tower.Weight + tower.ChildrensWeight;
        }


    }    

    public class Tower
    {
        public string Name { get; set; }
        public int Weight { get; set; }        
        public IEnumerable<string> Childrens { get; set; }
        public int ChildrensWeight { get; set; }

        public int TotalWeight => Weight + ChildrensWeight;

    }
}
