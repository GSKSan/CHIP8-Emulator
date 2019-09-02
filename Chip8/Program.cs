using System;
using System.Collections.Generic;
using System.IO;

namespace Chip8
{
    class Program
    {
        static void Main(string[] args)
        {
            var nib = (byte)(229 & 0x80);
            //Console.WriteLine(nib.ToString("X2"));
            CPU cpu = new CPU();
            List<byte> program = new List<byte>();
            using (BinaryReader reader = new BinaryReader(new FileStream(@"C:\Users\OEM\Desktop\Ch8\heart.ch8", FileMode.Open)))
            {
                while (reader.BaseStream.Position < reader.BaseStream.Length-1)
                {
                    program.Add(reader.ReadByte());

                    // Console.WriteLine(opcode.ToString("X4"));
                   
                }
                
            }
            try
            {
                cpu.LoadProgram(program.ToArray());

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            while (true)
            {
                try
                {
                    cpu.opCodesExecution();
                    cpu.DisplayConsole();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            Console.ReadKey();
        }
    }
}
