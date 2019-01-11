using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace TestingUtilities
{
    public class by
    {
        public string Description { get; private set;}
        public By Locator
        {
            get; private set;
        }
        public by(By _loc, string _Description)
        {
            Description = _Description;
            Locator = _loc;
        }

        public static implicit operator By(by d)
        {
            return d.Locator;
        }
    }
}
