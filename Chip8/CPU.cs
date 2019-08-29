using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Chip8
{
    class CPU
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

        //Display
        public byte[] Display = new byte[64 * 32];

        public void opCodesExecution(ushort opcode)
        {
            ushort nibble = (ushort)(opcode & 0xF000); //To get the first byte of the opcode

            switch(nibble)
            {
                case 0x0000:
                    if(opcode == 0x00e0)
                    {
                        for (int i = 0; i < Display.Length; i++)
                        {
                            Display[i] = 0;
                            Debug.Write("\nScreen cleared "+ opcode.ToString("X4"));
                        }
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
                    I= (ushort)(opcode & 0x0FFF);//To get the last 12 bits or 3 bytes
                    break;
                case 0X3000:


                    break;

                default:
                    throw new Exception($"Opcode not supported " + opcode.ToString("X4"));

            }
        }


    }
}
