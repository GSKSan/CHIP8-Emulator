using System;
using System.Collections.Generic;
using System.Text;

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
            ushort nibble = (ushort)(opcode & 0xF000);

            switch(nibble)
            {
                case 0x0000:
                    if(opcode == 0x00e0)
                    {
                        for (int i = 0; i < Display.Length; i++)
                        {
                            Display[i] = 0;
                        }
                    }
                    else if(opcode == 0x00ee)
                    {
                        I = stack.Pop();
                    }
                    else
                    {
                        throw new Exception("Opcpde note supported "+opcode.ToString("X4"));
                    }
                    break;
                default:
                    throw new Exception($"Opcode not supported " + opcode.ToString("X4"));

            }
        }


    }
}
