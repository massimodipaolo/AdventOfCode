using System;
namespace AdventOfCode
{
    public interface IPuzzle
    {
        string Input { get; set; }
        string Output();
        string Output(string input);
        string Output2();
        string Output2(string input);
    }
}
