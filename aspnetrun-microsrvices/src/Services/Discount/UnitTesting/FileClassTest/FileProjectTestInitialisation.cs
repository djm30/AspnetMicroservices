using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FileClassTest
{
    [TestClass]
    public class FileProjectTestInitialisation
    {



        [AssemblyInitialize()]
        public static void AssemblyInitialise(TestContext tc)
        {
            tc.WriteLine("In AssemblyInitialise Method");
        }

        [AssemblyCleanup()]
        public static void AssemblyCleanup() {
        }
    }
}
