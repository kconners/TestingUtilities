using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestingUtilities;
using NUnit.Framework;

namespace TestingUtilities
{
    public static class Testing_Objects
    {
        static public WebElem we;
        static public Browser browser;

        static public DataHelper dh;
        static public TestContext T;

        static public void Start(String FullPathToURLJson = "")
        {
            T = TestContext.CurrentContext;
            browser = new TestingUtilities.Browser(T);
            browser.StartBrowser(FullPathToURLJson, 100);
            we = new WebElem(browser);

        }
    }
}
