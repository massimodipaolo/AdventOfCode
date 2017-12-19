using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode._2017
{
    class _18 : Puzzle
    {
        public _18() {
            ReadInputFromFile("2017/18.txt");
        }

        /*
--- Day 18: Duet ---
You discover a tablet containing some strange assembly code labeled simply "Duet". Rather than bother the sound card with it, you decide to run the code yourself. Unfortunately, you don't see any documentation, so you're left to figure out what the instructions mean on your own.

It seems like the assembly is meant to operate on a set of registers that are each named with a single letter and that can each hold a single integer. 
You suppose each register should start with a value of 0.

There aren't that many instructions, so it shouldn't be hard to figure out what they do. Here's what you determine:

snd X plays a sound with a frequency equal to the value of X.
set X Y sets register X to the value of Y.
add X Y increases register X by the value of Y.
mul X Y sets register X to the result of multiplying the value contained in register X by the value of Y.
mod X Y sets register X to the remainder of dividing the value contained in register X by the value of Y (that is, it sets X to the result of X modulo Y).
rcv X recovers the frequency of the last sound played, but only when the value of X is not zero. (If it is zero, the command does nothing.)
jgz X Y jumps with an offset of the value of Y, but only if the value of X is greater than zero. (An offset of 2 skips the next instruction, an offset of -1 jumps to the previous instruction, and so on.)
Many of the instructions can take either a register (a single letter) or a number. The value of a register is the integer it contains; the value of a number is that number.

After each jump instruction, the program continues with the instruction to which the jump jumped. After any other instruction, the program continues with the next instruction. 
Continuing (or jumping) off either end of the program terminates it.

For example:

set a 1
add a 2
mul a a
mod a 5
snd a
set a 0
rcv a
jgz a -1
set a 1
jgz a -2
The first four instructions set a to 1, add 2 to it, square it, and then set it to itself modulo 5, resulting in a value of 4.
Then, a sound with frequency 4 (the value of a) is played.
After that, a is set to 0, causing the subsequent rcv and jgz instructions to both be skipped (rcv because a is 0, and jgz because a is not greater than 0).
Finally, a is set to 1, causing the next jgz instruction to activate, jumping back two instructions to another jump, which jumps again to the rcv, which ultimately triggers the recover operation.
At the time the recover operation is executed, the frequency of the last sound played is 4.

What is the value of the recovered frequency (the value of the most recently played sound) the first time a rcv instruction is executed with a non-zero value?         
             */
        public override string Output(string input)
        {
            /*
            input = @"set a 1
add a 2
mul a a
mod a 5
snd a
set a 0
rcv a
jgz a -1
set a 1
jgz a -2";
            */
            var rows = input.Split("\n");

            var instructions = new List<string>(rows.Length);
            instructions.AddRange(rows);
            int position = 0;
            var registers = new Dictionary<string, long>();
            var lasts = new Dictionary<string, long>();
            long last = 0;
            Regex number = new Regex(@"^(-*)[0-9]+$");
            while (position <= instructions.Capacity)
            {
                var cmd = instructions[position].Split(" ");
                
                if (!registers.ContainsKey(cmd[1]))                
                    registers.Add(cmd[1], 0);

                long value = 0;
                if (cmd.Length == 3)
                    if (number.IsMatch(cmd[2]))
                        value = int.Parse(cmd[2]);
                    else
                        value = registers[cmd[2]];


                switch (cmd[0])
                {
                    case "snd":                        
                        if (!lasts.ContainsKey(cmd[1]))
                            lasts.Add(cmd[1], 0);
                        lasts[cmd[1]] = registers[cmd[1]];
                        break;
                    case "set":
                        registers[cmd[1]] = value;
                        break;
                    case "add":
                        registers[cmd[1]] += value;
                        break;
                    case "mul":
                        registers[cmd[1]] *= value;
                        break;
                    case "mod":
                        registers[cmd[1]] %= value;
                        break;
                    case "rcv":
                        if (registers[cmd[1]] > 0 && lasts.ContainsKey(cmd[1]))
                        {
                            last = lasts[cmd[1]];
                            position = instructions.Capacity;
                            //registers[cmd[1]] = lasts[cmd[1]];                            
                        }
                        break;
                    case "jgz":
                        if (registers[cmd[1]] > 0)
                            position += (int)value - 1;
                        break;                    
                }

                position++;
            }
            
            return last.ToString();
        }

        /*
--- Part Two ---
As you congratulate yourself for a job well done, you notice that the documentation has been on the back of the tablet this entire time. 
While you actually got most of the instructions correct, there are a few key differences. This assembly code isn't about sound at all - it's meant to be run twice at the same time.

Each running copy of the program has its own set of registers and follows the code independently - in fact, the programs don't even necessarily run at the same speed. 
To coordinate, they use the send (snd) and receive (rcv) instructions:

snd X sends the value of X to the other program. These values wait in a queue until that program is ready to receive them. Each program has its own message queue, so a program can never receive a message it sent.
rcv X receives the next value and stores it in register X. If no values are in the queue, the program waits for a value to be sent to it. 
Programs do not continue to the next instruction until they have received a value. Values are received in the order they are sent.
Each program also has its own program ID (one 0 and the other 1); the register p should begin with this value.

For example:

snd 1
snd 2
snd p
rcv a
rcv b
rcv c
rcv d
Both programs begin by sending three values to the other. Program 0 sends 1, 2, 0; program 1 sends 1, 2, 1. 
Then, each program receives a value (both 1) and stores it in a, receives another value (both 2) and stores it in b, and then each receives the program ID of the other program (program 0 receives 1; program 1 receives 0) and stores it in c. 
Each program now sees a different value in its own copy of register c.

Finally, both programs try to rcv a fourth time, but no data is waiting for either of them, and they reach a deadlock. When this happens, both programs terminate.

It should be noted that it would be equally valid for the programs to run at different speeds; for example, program 0 might have sent all three values and then stopped at the first rcv before program 1 executed even its first instruction.

Once both of your programs have terminated (regardless of what caused them to do so), how many times did program 1 send a value?         
             */

        public static List<Message> messages { get; set; }
        public static List<string> instructions { get; set; }

        public async Task<int> Output2Async()
        {
            Input = @"snd 1
snd 2
snd p
rcv a
rcv b
rcv c
rcv d
";
            var rows = Input.Split("\r\n");
            instructions = new List<string>(rows.Length);
            instructions.AddRange(rows);

            var p0 = new Program(0);
            var p1 = new Program(1);

            await p0.Process(instructions);
            await p1.Process(instructions);

            while (p0.waiting && p1.waiting)
            {
                await Task.Delay(2000);
            }

            return p0.messageSent;
        }

        public class Program
        {
            private int _id { get; set; }
            public Dictionary<string, long> registers { get; set; }
            public int messageSent { get; set; }
            public bool waiting { get; set; } = false;
            
            public Program(int id)
            {
                registers = new Dictionary<string, long>();                
                registers["p"] = _id = id;
            }
            public async Task Process(List<string> instructions)
            {
                int position = 0;                
                Regex number = new Regex(@"^(-*)[0-9]+$");
                while (position <= instructions.Capacity)
                {
                    var cmd = instructions[position].Split(" ");

                    if (!registers.ContainsKey(cmd[1]))
                        registers.Add(cmd[1], 0);

                    long value = 0;
                    if (cmd.Length == 3)
                        if (number.IsMatch(cmd[2]))
                            value = int.Parse(cmd[2]);
                        else
                            value = registers[cmd[2]];


                    switch (cmd[0])
                    {
                        case "snd":                            
                            Message.Send(new Message() { senderId = _id, value = number.IsMatch(cmd[1]) ? long.Parse(cmd[1]) : registers[cmd[1]] });
                            Console.WriteLine($"Message sent: {_id} - {cmd[1]}");
                            messageSent++;
                            break;
                        case "set":
                            registers[cmd[1]] = value;
                            break;
                        case "add":
                            registers[cmd[1]] += value;
                            break;
                        case "mul":
                            registers[cmd[1]] *= value;
                            break;
                        case "mod":
                            registers[cmd[1]] %= value;
                            break;
                        case "rcv":    
                            while(true) {                                
                                var msg = Message.Receive(_id);
                                if (msg != null)
                                {
                                    registers[cmd[1]] = msg.value;
                                    Console.WriteLine($"Message received: {_id} - {msg.value}");
                                    waiting = false;
                                    break;
                                } else
                                {
                                    waiting = true;
                                    await Task.Delay(2000);
                                }                                    
                            }
                            break;
                        case "jgz":
                            if (registers[cmd[1]] > 0)
                                position += (int)value - 1;
                            break;
                    }

                    position++;
                }
            }
        }

        public class Message
        {
            public int senderId { get; set; }
            public long value { get; set; }

            public static void Send(Message message)
            {
                _18.messages.Add(message);
            }

            public static Message Receive(int receiverId)
            {
                var message = _18.messages.FirstOrDefault(_ => _.senderId != receiverId);
                if (message != null)
                {
                    messages.Remove(message);
                    return message;
                }                
                return null;
            }
        }
    }
}
