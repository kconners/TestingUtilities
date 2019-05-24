using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using FluentAssertions;


namespace TestingUtilities
{
    class Clicker
    {
        int countr;
        private IWebDriver _driver;
        private ClickSetting _clickSetting;
        private bool WasSuccessful;
        private IWebElement HH;
        private System.Timers.Timer aTimer;
        private WebElem we;

        public Clicker(IWebDriver driv, ClickSetting clickSetting)
        {
            countr = 0;
            _driver = driv;
            _clickSetting = clickSetting;
            WasSuccessful = false;
            aTimer = new System.Timers.Timer();
            we = new WebElem(_driver, _clickSetting);
        }
        WebElem.Direction _secondScroll;
        WebElem.Direction _firstScroll;
        public void Click(IWebElement item, string elementDescription, WebElem.Direction ScrollFirst)
        {
            HH = item;
            int countr = 0;

            _firstScroll = ScrollFirst;
            if (ScrollFirst == WebElem.Direction.Down) { _secondScroll = WebElem.Direction.Up; }
            else { _secondScroll = WebElem.Direction.Down; }

            bool didItWork = false;

            Console.WriteLine("Here");
            Console.WriteLine(string.Format("THis is my interval {0}", _clickSetting.PollInterval.TotalMilliseconds));
            aTimer = new System.Timers.Timer();
            aTimer.Interval = _clickSetting.PollInterval.TotalMilliseconds;
            aTimer.Elapsed += TimedClickOnWebElement;
            aTimer.Enabled = true;
            while (aTimer.Enabled == true)
            { }
            if (countr > _clickSetting.MaxPollCount && WasSuccessful == false)
            {
                didItWork = false;
            }
            else if (countr <= _clickSetting.MaxPollCount || WasSuccessful == true)
            {
                didItWork = true;
            }
            aTimer.Dispose();
            didItWork.Should().BeTrue();

        }
        private void TimedClickOnWebElement(Object source, System.Timers.ElapsedEventArgs e)
        {
           
            if (countr <= _clickSetting.MaxPollCount)
            {
       
                try
                {
                    
                    if (countr >= 5 && countr <= 10)
                    {
                        we.scroll(HH, _firstScroll);
                    }
                    else if (countr >= 10)
                    {
                        we.scroll(HH, _secondScroll);
                    }
                    
                    HH.Click();
                    WasSuccessful = true;
                    aTimer.Enabled = false;
                }
                catch
                {
                    if (countr < 5)
                    {
                        we.scrollintoView(HH);
                    }

                    we.HighLight(HH, 1);
                    countr++;
                    we.Log(WebElem.loglevel.debug, string.Format("Can't click {0} on attempt number {1}", Convert.ToString(HH), Convert.ToString(countr)));
                }

            }
            else if (countr > _clickSetting.MaxPollCount)
            {
                WasSuccessful = false;
                aTimer.Enabled = false;
            }
        }
        private void TimedRightClickOnWebElement(Object source, System.Timers.ElapsedEventArgs e)
        {
            //if (countr <= TimeOutSeconds)
            //{
            //    try
            //    {
            //        Actions action = new Actions(driv);
            //        action.ContextClick(HH);
            //        action.Perform();

            //        int TimeoutSeconds = 1;

            //        var wait = new WebDriverWait(driv, TimeSpan.FromSeconds(TimeoutSeconds));

            //        wait = new WebDriverWait(driv, TimeSpan.FromSeconds(TimeoutSeconds));
            //        wait.Until(driver => driv.FindElement(By.LinkText(OptionToClick)));

            //        IWebElement Op = driv.FindElement(By.LinkText(OptionToClick));
            //        Op.Click();


            //        WasSuccessful = true;
            //        aTimer.Enabled = false;
            //        OptionToClick = string.Empty;
            //    }
            //    catch
            //    {
            //        scrollintoView(HH);
            //        HighLight(HH, 1);
            //        countr++;
            //        Log(string.Format("Can't right click {0} on attempt number {1}", Convert.ToString(HH), Convert.ToString(countr)));
            //    }

            //}
            //else if (countr > TimeOutSeconds)
            //{
            //    WasSuccessful = false;
            //    aTimer.Enabled = false;
            //}
        }
    }
}
