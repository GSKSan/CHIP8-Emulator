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

        //Address register
        public ushort I = 0;

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

        public void opCodesExecution(ushort opcode)
        {
            ushort nibble = (ushort)(opcode & 0xF000); //To get the first byte of the opcode
            Console.WriteLine(nibble.ToString("X4"));
            Console.WriteLine(((ushort)(opcode & 0x00FF)).ToString("X4"));

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
                        I = stack.Pop();
                    }
                    else
                    {
                        throw new Exception("Opcode not supported "+opcode.ToString("X4"));
                    }
                    break;
                case 0x1000:
                    var address = (ushort)(opcode & 0x0FFF);//To get the last 12 bits or 3 bytes
                    I = address;
                    Debug.Write("Jumps to address " + address);
                    break;
                case 0x2000:
                    stack.Push(I);
                    Debug.Write("Calls subroutine at " + I);

                    I = (ushort)(opcode & 0x0FFF);//To get the last 12 bits or 3 bytes
                    break;
                case 0x3000:

                    var x = (opcode & 0x0F00)>>8;
                    var xx = (opcode & 0x00FF);
                    if (registers[x] == xx)
                    {
                        I += 2;
                    }
                    Debug.Write("Compare Vx-" + registers[x] + " and nn-" + xx);

                    break;
                case 0x4000:
                    var x1 = (opcode & 0x0F00)>>8;
                    var xx1 = (opcode & 0x00FF);
                    if (registers[x1] != xx1)
                    {
                        I += 2;
                    }
                    Debug.Write("Compare Vx-" + registers[x1] + " and nn-" + xx1);

                    break;
                case 0x5000:
                    x= (opcode & 0x0F00)>>8;
                    var y = (opcode & 0x00F0)>>4;
                    if (registers[x] == registers[y])
                    {
                        I += 2;
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



                    }
                    break;  
                default:
                    throw new Exception($"Opcode not supported " + opcode.ToString("X4"));

            }
        }


    }
}
