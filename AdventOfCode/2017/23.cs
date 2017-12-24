using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections;

namespace AdventOfCode._2017
{
    public class _23 : Puzzle
    {
        public _23()
        {
            ReadInputFromFile("2017/23.txt");
        }

        /*
--- Day 23: Coprocessor Conflagration ---
You decide to head directly to the CPU and fix the printer from there. As you get close, you find an experimental coprocessor doing so much work that the local programs are afraid it will halt and catch fire. This would cause serious issues for the rest of the computer, so you head in and see what you can do.

The code it's running seems to be a variant of the kind you saw recently on that tablet. The general functionality seems very similar, but some of the instructions are different:

set X Y sets register X to the value of Y.
sub X Y decreases register X by the value of Y.
mul X Y sets register X to the result of multiplying the value contained in register X by the value of Y.
jnz X Y jumps with an offset of the value of Y, but only if the value of X is not zero. (An offset of 2 skips the next instruction, an offset of -1 jumps to the previous instruction, and so on.)
Only the instructions listed above are used. The eight registers here, named a through h, all start at 0.

The coprocessor is currently set to some kind of debug mode, which allows for testing, but prevents it from doing any meaningful work.

If you run the program (your puzzle input), how many times is the mul instruction invoked?
        */
        public override string Output(string input)
        {

            var rows = input.Split("\n");

            var instructions = new List<string>(rows.Length);
            instructions.AddRange(rows);
            int position = 0;
            var registers = new Dictionary<string, long>();
            long mul = 0;
            Regex number = new Regex(@"^(-*)[0-9]+$");
            while (position < instructions.Capacity)
            {
                var cmd = instructions[position].Split(" ");

                if (!registers.ContainsKey(cmd[1]))
                    registers.Add(cmd[1], number.IsMatch(cmd[1]) ? int.Parse(cmd[1]) : 0);

                long value = 0;
                if (cmd.Length == 3)
                    if (number.IsMatch(cmd[2]))
                        value = int.Parse(cmd[2]);
                    else
                        value = registers[cmd[2]];


                switch (cmd[0])
                {
                    case "set":
                        registers[cmd[1]] = value;
                        break;
                    case "sub":
                        registers[cmd[1]] -= value;
                        break;
                    case "mul":
                        registers[cmd[1]] *= value;
                        mul++;
                        break;
                    case "jnz":
                        if (registers[cmd[1]] != 0)
                            position += (int)value - 1;
                        break;
                    default:
                        position--;
                        break;
                }

                position++;
            }

            return mul.ToString();
        }

        /*
--- Part Two ---
Now, it's time to fix the problem.

The debug mode switch is wired directly to register a. You flip the switch, which makes register a now start at 1 when the program is executed.

Immediately, the coprocessor begins to overheat. Whoever wrote this program obviously didn't choose a very efficient implementation. 
You'll need to optimize the program if it has any hope of completing before Santa needs that printer working.

The coprocessor's ultimate goal is to determine the final value left in register h once the program completes. 
Technically, if it had that... it wouldn't even need to run the program.

After setting register a to 1, if the program were to run to completion, what value would be left in register h?
        */
        public override string Output2(string input)
        {
            input = @"set b 84
set c b
jnz a 2
jnz 1 5
mul b 100
sub b -100000
set c b                         
sub c -17000                    
set f 1                  
set a b
div a 2
set d 2                  
set j b 
div j d 
set e j              
set g d
mul g e
sub g b
jnz g 3             
set f 0
jnz 1 5                           
sub d -1                
set g d                 
sub g a
jnz g -12              
jnz f 2           
sub h -1
set g b            
sub g c
jnz g 2           
jnz 1 3 
sub b -17   
jnz 1 -24";
            var rows = input.Split("\n");

            var instructions = new List<string>(rows.Length);
            instructions.AddRange(rows);
            int position = 0;
            var registers = new Dictionary<string, long>();
            Regex number = new Regex(@"^(-*)[0-9]+$");
            while (position < instructions.Capacity)
            {
                var cmd = instructions[position].Trim().Split(" ");

                if (!registers.ContainsKey(cmd[1]))
                    registers.Add(cmd[1], number.IsMatch(cmd[1]) ? int.Parse(cmd[1]) : (cmd[1] == "a" ? 1 : 0));

                long value = 0;
                if (cmd.Length == 3)
                    if (number.IsMatch(cmd[2]))
                        value = int.Parse(cmd[2]);
                    else
                        value = registers[cmd[2]];


                switch (cmd[0])
                {
                    case "set":
                        registers[cmd[1]] = value;
                        break;
                    case "sub":
                        registers[cmd[1]] -= value;
                        break;
                    case "mul":
                        registers[cmd[1]] *= value;
                        break;
                    case "div":
                        registers[cmd[1]] /= value;
                        break;
                    case "jnz":
                        if (registers[cmd[1]] != 0)
                            position += (int)value - 1;
                        break;
                }

                //Console.WriteLine($"[{position}] {instructions[position]}");
                position++;
            }

            return registers["h"].ToString();
        }

        public string Output2b(string input)
        {
            int h = 0;
            var primes = GetPrimeArray(108400, 125400);
            for (int i = 108400; i <= 125400; i += 17)
            {
                if (!primes.Any(_ => _ == i))
                    h++;
            }
            return h.ToString();
        }

        private static int[] GetPrimeArray(int floor, int ceiling)
        {


            // Variables:
            int stoppingPoint = (int)Math.Sqrt(ceiling);
            double rosserBound = (1.25506 * (ceiling + 1)) / Math.Log(ceiling + 1, Math.E);
            int[] primeArray = new int[(int)rosserBound];
            int primeIndex = 0;
            int bitIndex = 4;
            int innerIndex = 3;

            // Handle single digit prime ranges.
            if (ceiling < 11)
            {
                if (floor <= 2 && ceiling >= 2) // Range includes 2.
                    primeArray[primeIndex++] = 2;

                if (floor <= 3 && ceiling >= 3) // Range includes 3.
                    primeArray[primeIndex++] = 3;

                if (floor <= 5 && ceiling >= 5) // Range includes 5.
                    primeArray[primeIndex++] = 5;

                return primeArray;
            }

            // Begin Sieve of Eratosthenes. All values initialized as true.
            BitArray primeBits = new BitArray(ceiling + 1, true);
            primeBits.Set(0, false); // Zero is not prime.
            primeBits.Set(1, false); // One is not prime.

            checked // Check overflow.
            {
                try
                {
                    // Set even numbers, excluding 2, to false.
                    for (bitIndex = 4; bitIndex < ceiling; bitIndex += 2)
                        primeBits[bitIndex] = false;
                }
                catch { } // Break for() if overflow occurs.
            }

            // Iterate by steps of two in order to skip even values.
            for (bitIndex = 3; bitIndex <= stoppingPoint; bitIndex += 2)
            {
                if (primeBits[bitIndex] == true) // Is prime.
                {
                    // First position to unset is always the squared value.
                    innerIndex = bitIndex * bitIndex;
                    primeBits[innerIndex] = false;

                    checked // Check overflow.
                    {
                        try
                        {
                            // Set multiples of i, which are odd, to false.
                            innerIndex += bitIndex + bitIndex;
                            while (innerIndex <= ceiling)
                            {
                                primeBits[innerIndex] = false;
                                innerIndex += bitIndex + bitIndex;
                            }
                        }
                        catch { continue; } // Break while() if overflow occurs.
                    }
                }
            }

            // Set initial array values.
            if (floor <= 2)
            {
                // Range includes 2 - 5.
                primeArray[primeIndex++] = 2;
                primeArray[primeIndex++] = 3;
                primeArray[primeIndex++] = 5;
            }

            else if (floor <= 3)
            {
                // Range includes 3 - 5.
                primeArray[primeIndex++] = 3;
                primeArray[primeIndex++] = 5;
            }
            else if (floor <= 5)
            {
                // Range includes 5.
                primeArray[primeIndex++] = 5;
            }

            // Increment values that skip multiples of 2, 3, and 5.
            int[] increment = { 6, 4, 2, 4, 2, 4, 6, 2 };
            int indexModulus = -1;
            int moduloSkipAmount = (int)Math.Floor((double)(floor / 30));

            // Set bit index to increment range which includes the floor.
            bitIndex = moduloSkipAmount * 30 + 1;

            // Increase bit and increment indicies until the floor is reached.
            for (int i = 0; i < increment.Length; i++)
            {
                if (bitIndex >= floor)
                    break; // Floor reached.

                // Increment, skipping multiples of 2, 3, and 5.
                bitIndex += increment[++indexModulus];
            }

            // Initialize values of return array.
            while (bitIndex <= ceiling)
            {
                // Add bit index to prime array, if true.
                if (primeBits[bitIndex])
                    primeArray[primeIndex++] = bitIndex;

                checked // Check overflow.
                {
                    try
                    {
                        // Increment. Skip multiples of 2, 3, and 5.
                        indexModulus = ++indexModulus % 8;
                        bitIndex += increment[indexModulus];
                    }
                    catch { break; } // Break if overflow occurs.
                }
            }

            // Resize array. Rosser-Schoenfeld upper bound of π(x) is not an equality.
            Array.Resize(ref primeArray, primeIndex);


            return primeArray;
        }



    }
}
