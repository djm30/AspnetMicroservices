using Microsoft.VisualStudio.TestTools.UnitTesting;
using FileProject;
using System.Text.RegularExpressions;

namespace FileClassTest
{
    [TestClass]
    public class FileProcessTest : TestBase
    {

        private const string BadFileName = @"C:\Windows\bluenebula.exe";

        [ClassInitialize()]
        public static void ClassIntitialise(TestContext tc)
        {
            tc.WriteLine("Initialising test");
        }

        [ClassCleanup()]
        public static void ClassCleanup() { }

        [TestInitialize]
        public void TestInitialise()
        {
            WriteDescription(this.GetType());
            TestContext.WriteLine("In TestInitialise() method");
            SetGoodFileName();

            if (TestContext.TestName.StartsWith("FileNameDoesExist"))
            {
                if (!string.IsNullOrEmpty(_GoodFileName))
                {
                    // Create the file
                    File.AppendAllText(_GoodFileName, "Some text");
                }
            }
        }

        [TestCleanup]
        public void TestCleanup() {

            if (TestContext.TestName.StartsWith("FileNameDoesExist"))
            {
                // Delete file
                if (File.Exists(_GoodFileName)) File.Delete(_GoodFileName);
            }
        }

        [TestMethod]
        [Description("Check to see if a file exists")]
        [Owner("Dylan Morrison")]
        [Priority(1)]
        public void FileNameDoesExist()
        {
            FileProcess fp = new FileProcess();
            bool fromCall;

            TestContext.WriteLine($"Checking File {_GoodFileName}");

            fromCall = fp.FileExists(_GoodFileName);

            Assert.IsTrue(fromCall);
        }

        [TestMethod]
        [Description("Check to see if a file doesn't exist")]
        [Owner("Dylan Morrison")]
        public void FileNameDoesNotExist()
        {
            FileProcess fp = new FileProcess();
            bool fromCall;

            fromCall = fp.FileExists(BadFileName);

            Assert.IsFalse(fromCall);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        [Owner("Philip Cox")]
        [Description("Check for a thrown ArgumentNullException")]
        [Priority(3)]
        public void FillNameNullOrEmpty_UsingAttribute()
        {
            FileProcess fp = new FileProcess();

            fp.FileExists("");
        }

        [TestMethod]
        [Description("Check to see if a ArgumentNullException is thrown and caught by  try catch")]
        [Owner("Philip Cox")]
        [TestCategory("Exception")]
        [Priority(5)]
        [Timeout(1000)]
        [Ignore]
        public void FillNameNullOrEmpty_UsingTryCatch()
        {
            FileProcess fp = new FileProcess();

            try
            {
                fp.FileExists("");
            }
            catch (ArgumentNullException)
            {
                return;
            }
            Assert.Fail("Call to .FileExists did NOT throw an ArgumentNullException like it should have.");
        }

        [TestMethod]
        [DataRow(1, 1, DisplayName = "First Test (1,1)")]
        [DataRow(42, 42, DisplayName = "Second Test (42, 42)")]
        public void AreNumbersEqual(int num1, int num2)
        {
            Assert.AreEqual(num1, num2);
        }

        [TestMethod]
        [DeploymentItem("FileToDeploy.txt")]
        [DataRow(@"C:\Windows\Regedit.exe", DisplayName = "Regedit.exe")]
        [DataRow("FileToDeploy.txt", DisplayName = "Deployment Item: FileToDeploy.txt")]
        public void FileNameUsingDataRow(string fileName)
        {
            FileProcess fp = new FileProcess();
            bool fromCall;

            if (!fileName.Contains(@"\"))
            {
                fileName = TestContext.DeploymentDirectory + @"\" + fileName;
            }
            TestContext.WriteLine("Checking File" + fileName);
            fromCall = fp.FileExists(fileName);
            Assert.IsTrue(fromCall);
        }

        [TestMethod]
        public void FileNameDoesExistSimpleMessage()
        {
            FileProcess fp = new FileProcess();
            bool fromCall;

            fromCall = fp.FileExists(_GoodFileName);
            Assert.IsFalse(fromCall, "{0} does not exist", _GoodFileName);
        }

        [TestMethod]
        public void AreEqualTest()
        {
            string str1 = "Paul";
            string str2 = "paul";

            Assert.AreEqual(str1, str2, true);
        }

        [TestMethod]
        public void AreNotEqualTest()
        {
            string str1 = "Paul";
            string str2 = "Jaul";

            Assert.AreNotEqual(str1, str2);
        }

        [TestMethod]
        public void AreSame()
        {
            FileProcess x = new FileProcess();
            FileProcess y = new FileProcess();
            Assert.AreNotSame(x, y);

            y = x;
            Assert.AreSame(x, y);
        }

        [TestMethod]
        public void TypeOf()
        {
            FileProcess fp = new FileProcess();
            Assert.IsInstanceOfType(fp, typeof(FileProcess));


            Assert.IsInstanceOfType(new ArgumentException(), typeof(Exception));

            Assert.IsNull(null);
        }

        [TestMethod]
        public void Contains()
        {
            string str1 = "Northern Ireland";
            string str2 = "Ireland";

            StringAssert.Contains(str1, str2);
        }

        [TestMethod]
        public void StartsWith()
        {
            string str1 = "Northern Ireland";
            string str2 = "Northern";

            StringAssert.StartsWith(str1, str2);
        }

        [TestMethod]
        public void IsAllLowerCase()
        {
            Regex r = new Regex(@"^([A-z])+$");
            StringAssert.DoesNotMatch("all lower case", r);
        }

        [TestMethod]
        public void CollectionEqual()
        {
            var one = new int[] { 1, 2 };
            var two = new int[] { 1, 2 };

            CollectionAssert.Equals(one, two);
        }

        [TestMethod]
        public void CollectionEquivalent()
        {
            var one = new int[] { 1, 2 };
            var two = new int[] { 2, 1 };

            CollectionAssert.AreNotEqual(one, two);
            CollectionAssert.AreEquivalent(one, two);
        }

        [TestMethod]
        public void CollectionContainsInstanceOf()
        {
            var one = new int[] { 1, 2 };
            CollectionAssert.AllItemsAreInstancesOfType(one, typeof(int));
            CollectionAssert.Contains(one, 1);
        }


    }
}