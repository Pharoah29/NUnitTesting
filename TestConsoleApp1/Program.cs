using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTestProject1;

namespace TestConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            UnitTest1 tests = new UnitTest1();

            tests.IsInSilentMode = true;
            tests.NumberOfRuns = 2;

            string logFile = Path.GetTempPath() + "test.logger.txt";


            Parallel.For(0, 3, (i) =>
            {
                tests.BeforeEach();

                tests.TestMethod1();

                tests.AfterEach();
                tests.AfterAll();

            });



        }
    }
}
