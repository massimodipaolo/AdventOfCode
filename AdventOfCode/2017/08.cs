using System;
using System.Linq;
using System.Collections.Generic;
namespace AdventOfCode._2017
{
    public class _08 : Puzzle
    {
        public _08()
        {
            ReadInputFromFile("2017/08.txt");
        }

        /*
--- Day 8: I Heard You Like Registers ---
You receive a signal directly from the CPU. Because of your recent assistance with jump instructions, it would like you to compute the result of a series of unusual register instructions.

Each instruction consists of several parts: the register to modify, whether to increase or decrease that register's value, the amount by which to increase or decrease it, and a condition. If the condition fails, skip the instruction without modifying the register. The registers all start at 0. The instructions look like this:

b inc 5 if a > 1
a inc 1 if b < 5
c dec -10 if a >= 1
c inc -20 if c == 10
These instructions would be processed as follows:

Because a starts at 0, it is not greater than 1, and so b is not modified.
a is increased by 1 (to 1) because b is less than 5 (it is 0).
c is decreased by -10 (to 10) because a is now greater than or equal to 1 (it is 1).
c is increased by -20 (to -10) because c is equal to 10.
After this process, the largest value in any register is 1.

You might also encounter <= (less than or equal to) or != (not equal to). However, the CPU doesn't have the bandwidth to tell you what all the registers are named, and leaves that to you to determine.

What is the largest value in any register after completing the instructions in your puzzle input?
        */
        public override string Output(string input)
        {
            /*
            input = @"b inc 5 if a > 1
a inc 1 if b < 5
c dec -10 if a >= 1
c inc -20 if c == 10";
            */
            var instructions = input.Split("\n");
            var variables = new Dictionary<string, int>();
            foreach (var instruction in instructions)
            {
                var elements = instruction.Split(" ");
                //b inc 5 if a > 1
                if (!variables.ContainsKey(elements[0])) variables[elements[0]] = 0; //b
                if (!variables.ContainsKey(elements[4])) variables[elements[4]] = 0; //a
                if (evaluate(variables[elements[4]], int.Parse(elements[6]), elements[5])) //a,>,1
                    variables[elements[0]] = sum(variables[elements[0]], int.Parse(elements[2]), elements[1]); //b = sum(b,5,inc)
            }

            return variables.Select(kv => kv.Value).Max().ToString();
        }

        /*
--- Part Two ---
To be safe, the CPU also needs to know the highest value held in any register during this process so that it can decide how much memory to allocate to these operations. 
For example, in the above instructions, the highest value ever held was 10 (in register c after the third instruction was evaluated).
        */
        public override string Output2(string input)
        {
            var instructions = input.Split("\n");
            var variables = new Dictionary<string, int>();
            int highest = 0;
            foreach (var instruction in instructions)
            {
                var elements = instruction.Split(" ");
                //b inc 5 if a > 1
                if (!variables.ContainsKey(elements[0])) variables[elements[0]] = 0; //b
                if (!variables.ContainsKey(elements[4])) variables[elements[4]] = 0; //a
                if (evaluate(variables[elements[4]], int.Parse(elements[6]), elements[5])) //a,>,1
                {
                    variables[elements[0]] = sum(variables[elements[0]], int.Parse(elements[2]), elements[1]); //b = sum(b,5,inc)
                    if (variables[elements[0]] > highest) highest = variables[elements[0]];
                }
            }

            return highest.ToString();
        }

        private bool evaluate(int v1, int v2, string comparer)
        {
            bool result = true;
            switch (comparer)
            {
                case ">":
                    result = v1 > v2;
                    break;
                case ">=":
                    result = v1 >= v2;
                    break;
                case "==":
                    result = v1 == v2;
                    break;
                case "<":
                    result = v1 < v2;
                    break;
                case "<=":
                    result = v1 <= v2;
                    break;
                case "!=":
                    result = v1 != v2;
                    break;
            }
            return result;
        }

        private int sum(int v1, int v2, string operation)
        {
            return operation == "inc" ? v1 + v2 : v1 - v2;
        }
    }
}
