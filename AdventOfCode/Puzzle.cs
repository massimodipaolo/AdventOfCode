using System;
namespace AdventOfCode
{
    public class Puzzle : IPuzzle
    {
        public Puzzle()
        {
        }

        public int Year => 2005;
        public int Day => 1;
        public string Text => "";
        public string Input => Console.ReadLine();
        public string Output()
        {
            return Output(Input);
        }

        public string Output(string input)
        {
            return "";
        }
    }
}
