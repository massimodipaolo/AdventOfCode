using System;
namespace AdventOfCode
{
    public class Puzzle : IPuzzle
    {
        public Puzzle()
        {
        }

        public string Input { get; set; }

        public string Output()
        {
            return Output(Input);
        }

        public virtual string Output(string input)
        {
            return string.Empty;
        }
    }
}
