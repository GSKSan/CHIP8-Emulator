using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Chip8
{
   public class CPU
    {
        //Memory
        public byte[] memory = new byte[4096];

        //Registers
        public byte[] registers = new byte[16];

        public bool WaitForKeyPress = false;

        //Address register
        public ushort I = 0;

        public ushort PC = 0;

        //stack
        public Stack<ushort> stack = new Stack<ushort>();

        //Timers
        public byte DelayTimer;
        public byte SoundTimer;

        //Keyboard
        public byte Keyboard;

        public string testMsg;

        //Display
        public byte[] Display = new byte[64 * 32];

        //Random gen
        public Random generator = new Random(Environment.TickCount);

        //Program
        public ushort[] Program;
        public void LoadProgram(ushort[] program)
        {
            for (int i = 0; i < program.Length; i++)
            {
                memory[512 + i * 2] = (byte)((program[i] & 0xff00)>>8);
                memory[513 + i * 2] = (byte)(program[i] & 0x00ff);
            }
            PC = 512;
            //opCodesExecution();
        }
        public void opCodesExecution()
        {
            var opcode = (ushort)((memory[PC] << 8) | (memory[PC + 1]));
            ushort nibble = (ushort)(opcode & 0xF000); //To get the first byte of the opcode
           // Console.WriteLine(nibble.ToString("X4"));
            //Console.WriteLine(((ushort)(opcode & 0x00FF)).ToString("X4"));

            if (WaitForKeyPress)
            {
                registers[(opcode & 0x0f00) >> 8] = Keyboard;
                return;
            }

            PC += 2;
            switch(nibble)
            {
                case 0x0000:
                    if(opcode == 0x00e0)
                    {
                        for (int i = 0; i < Display.Length; i++)
                        {
                            Display[i] = 0;
                        }
                        Debug.Write("\nScreen cleared " + opcode.ToString("X4"));

                    }
                    else if(opcode == 0x00ee)
                    {
                        PC = stack.Pop();
                    }
                    else
                    {
                        throw new Exception("Opcode not supported "+opcode.ToString("X4"));
                    }
                    break;
                case 0x1000:
                    var address = (ushort)(opcode & 0x0FFF);//To get the last 12 bits or 3 bytes
                    PC = address;
                    Debug.Write("Jumps to address " + address);
                    break;
                case 0x2000:
                    stack.Push(PC);
                    Debug.Write("Calls subroutine at " + I);

                    PC = (ushort)(opcode & 0x0FFF);//To get the last 12 bits or 3 bytes
                    break;
                case 0x3000:

                    var x = (opcode & 0x0F00)>>8;
                    var xx = (opcode & 0x00FF);
                    if (registers[x] == xx)
                    {
                        PC += 2;
                    }
                    Debug.Write("Compare Vx-" + registers[x] + " and nn-" + xx);

                    break;
                case 0x4000:
                    var x1 = (opcode & 0x0F00)>>8;
                    var xx1 = (opcode & 0x00FF);
                    if (registers[x1] != xx1)
                    {
                        PC += 2;
                    }
                    Debug.Write("Compare Vx-" + registers[x1] + " and nn-" + xx1);

                    break;
                case 0x5000:
                    x= (opcode & 0x0F00)>>8;
                    var y = (opcode & 0x00F0)>>4;
                    if (registers[x] == registers[y])
                    {
                        PC += 2;
                    }
                    break;
                case 0x6000:
                    x = (opcode & 0x0F00) >> 8;
                     var kk = (byte)(opcode & 0x00FF);
                    registers[x] = kk;
                    break;
                case 0x7000:
                    x = (opcode & 0x0F00) >> 8;
                    kk = (byte)(opcode & 0x00FF);
                    registers[x] += kk;
                    break;
                case 0x8000:
                    x = (opcode & 0x0F00) >> 8;
                    y = (opcode & 0x00F0) >> 4;
                    switch (opcode & 0x000F)
                    {
                        
                         case 0:
                            
                            registers[x] = registers[y];
                            break;
                        case 1:
                            registers[x] =(byte) (registers[x] | registers[y]);
                            break;
                        case 2:
                            registers[x] = (byte)(registers[x] & registers[y]);
                            break;
                        case 3:
                            registers[x] = (byte)(registers[x] ^ registers[y]);
                            break;
                        case 4:
                            registers[15] = (byte)(registers[x] + registers[y] > 255 ? 1 : 0);
                            registers[x] =(byte)(( registers[x] + registers[y])&0x00FF);
                            break;
                        case 5:
                            registers[15] = (byte)(registers[x] > registers[y] ? 1 : 0);
                            registers[x] = (byte)(registers[x] - registers[y]);
                            break;
                        case 6:
                            registers[15] = (byte)(registers[x] & 0x0001);
                            registers[x] =(byte)( registers[x] >> 1);
                            break;
                        case 7:
                            registers[15] = (byte)(registers[y] > registers[x] ? 1 : 0);
                            registers[x] = (byte)(registers[x] - registers[y]);
                            break;
                        case 14:
                            registers[15]=(byte)((registers[x]& 0x80) == 0x80 ? 1:0);
                            registers[x] = (byte)(registers[x] << 1);
                            break;




                    }

                    break;
                case 0x9000:
                    x = (opcode & 0x0F00) >> 8;
                    y = (opcode & 0x00F0) >> 4;
                    if (registers[x] != registers[y])
                    {

                        PC += 2;
                    }
                    break;
                case 0xA000:
                    var nnn = (byte)(opcode & 0x0fff);
                    I = nnn;
                    break;
                case 0xB000:
                    nnn = (byte)(opcode & 0x0fff);
                    PC = (byte)(nnn + registers[0]);
                    break;
                case 0xC000:
                    x = (opcode & 0x0F00) >> 8;
                    var rand_byte = (byte)(generator.Next() & (opcode & 0x00ff));
                    registers[x] = rand_byte;
                    break;
                case 0xD000:
                    x = registers[(opcode & 0x0F00) >> 8];
                    y = registers[(opcode & 0x00F0) >> 4];
                    int n = opcode & 0x000F;

                    registers[15] = 0;

                    for (int i = 0; i < n; i++)
                    {
                        byte mem = memory[I + i];

                        for (int j = 0; j < 8; j++)
                        {
                            byte pixel = (byte)((mem >> (7 - j)) & 0x01);
                            int index = x + j + (y + i) * 64;

                            if (index > 2047) continue;

                            if (pixel == 1 && Display[index] != 0) registers[15] = 1;

                            Display[index] =(byte)( (Display[index] != 0 && pixel == 0) || (Display[index] == 0 && pixel == 1) ? 0xffffffff : 0);//(byte)(Display[index] ^ pixel);
                        }
                    }
                    break;
                case 0xE000:
                    switch(opcode & 0x00FF)
                    {
                        case 0x009E:
                            if((registers[(opcode & 0x0f00) >> 8] == Keyboard))
                            {
                                PC += 2;
                            }
                            break;
                        case 0x00A1:
                            if((registers[(opcode & 0x0f00) >> 8] != Keyboard))
                            {
                                PC += 2;
                            }
                            break;
                        default:
                            throw new Exception($"Unsupported opcode {opcode.ToString("X4")}");
                    }
                    break;
                case 0xF000:
                    int fx = (opcode & 0x0F00) >> 8;
                    switch (opcode & 0x00FF)
                    {
                        case 0x07:
                            registers[fx] = DelayTimer;

                            break;
                        case 0x0a:
                            WaitForKeyPress = true;
                            PC -= 2;
                            break;
                        case 0x15:

                            DelayTimer = registers[fx];
                            break;
                        case 0x18:

                            SoundTimer = registers[fx];
                            break;
                        case 0x1E:
                            I = (ushort)(I + registers[fx]);
                            break;
                        case 0x29:
                            I = (ushort)(registers[fx] * 5);
                            break;
                        case 0x33:
                            memory[I] = (byte)(registers[fx] / 100);
                            memory[I + 1] = (byte)((registers[fx] / 100) % 10);
                            memory[I + 2] = (byte)(registers[fx] % 10);
                            break;
                        case 0x55:
                            for (int i = 0; i < fx; i++)
                            {
                                memory[I + i] = registers[i];
                            }
                            break;
                        case 0x65:
                            for (int i = 0; i < fx; i++)
                            {
                                registers[i] = memory[I + i];
                            }
                            break;
                        default:
                            throw new Exception($"Opcode not supported " + opcode.ToString("X4"));



                    }
                    break;

                default:
                    throw new Exception($"Opcode not supported " + opcode.ToString("X4"));

            }
        }


    }
}
