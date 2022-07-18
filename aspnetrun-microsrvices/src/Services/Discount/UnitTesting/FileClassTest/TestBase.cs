using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FileClassTest
{
    public class TestBase
    {
        public TestContext TestContext { get; set; }
        protected string _GoodFileName;


        protected void SetGoodFileName()
        {
            _GoodFileName = TestContext.Properties["GoodFileName"].ToString();
            if (_GoodFileName.Contains("[AppPath]"))
            {
                _GoodFileName = _GoodFileName.Replace("[AppPath]", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
            }
        }

        protected void WriteDescription(Type typ)
        {
            string testName = TestContext.TestName;

            MemberInfo method = typ.GetMethod(testName);
            if(method != null)
            {
                Attribute attr = method.GetCustomAttribute(typeof(DescriptionAttribute));
                if(attr != null)
                {
                    DescriptionAttribute dattr = (DescriptionAttribute)attr;
                    TestContext.WriteLine("Test Description: " + dattr.Description);
                }
            }
        }
    }
}
