using Microsoft.VisualStudio.TestTools.UnitTesting;
using Chip8;
namespace Chip8.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
        }

        [TestMethod]
        public void TestClearScreen()
        {
            var chip = new CPU();
            ushort clrScrOpcode = 0x00e0;
           // chip.opCodesExecution(clrScrOpcode);
            Assert.Fail();
            


        }
    }
}
