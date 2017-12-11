using System;
namespace AdventOfCode
{
    public interface IPuzzle
    {
        string Input { get; set; }
        string Output();
        string Output(string input);
    }
}
