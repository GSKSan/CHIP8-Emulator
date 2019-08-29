using System;
using System.IO;

namespace Chip8
{
    class Program
    {
        static void Main(string[] args)
        {

            CPU cpu = new CPU();
            using (BinaryReader reader = new BinaryReader(new FileStream(@"C:\Users\santh\source\repos\CHIP8-Emulator\Chip8\IBM Logo.ch8", FileMode.Open)))
            {
                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    var opcode =(ushort) (reader.ReadByte() << 8 | reader.ReadByte());

                    // Console.WriteLine(opcode.ToString("X4"));
                    try
                    {
                        cpu.opCodesExecution(opcode);

                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            Console.ReadKey();
        }
    }
}
