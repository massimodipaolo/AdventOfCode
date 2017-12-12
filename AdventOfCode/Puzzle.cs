using System;
using System.IO;

namespace AdventOfCode
{
    public class Puzzle : IPuzzle
    {
        public Puzzle()
        {
        }

        public string Input { get; set; }

        public void ReadInputFromFile(string path) {
            Input= System.IO.File.ReadAllText(path);          
        }

        public string Output()
        {
            return Output(Input);
        }

        public virtual string Output(string input)
        {
            return string.Empty;
        }

        public string Output2()
        {
            return Output2(Input);
        }

        public virtual string Output2(string input)
        {
            return string.Empty;
        }
    }
}
