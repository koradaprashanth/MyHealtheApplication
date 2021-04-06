using System;
using System.Collections.Generic;
using System.Threading;
using CTSSeleniumFramework;
using CTSSeleniumFramework.Enumerations;
using CTSSeleniumFramework.Extensions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace MyHealtheApplication
{
    // Nunit Example
    // This will look at your configuration (seleniumConfig.json or env variables) 
    // and loop each fixture based on each enabled
    [TestFixtureSource(typeof(SeleniumEnabledBrowsersFixtureArgs))]
    public class SeleniumClass:SeleniumTestBase
    {
        private static IWebDriver _webDriver;
        private static IWebElement _pageHeader;
        public SeleniumClass(BrowserType browser)
        {
            // Get the specified webdriver browser. 
            // This will create the local or remote browser session and open it up.
            _webDriver = CreateWebDriver(browser);

            // Login to specified application using Cerner Assocate IDP
            _webDriver.Navigate().GoToUrl("https://healtheatcerner.hac.wellness.us-1.healtheintent.com/pages/wellness/trackers/steps");

        }

        // Optional SetUp method to wait for page to load and/or other setup actions prior to each test method run.
        // This runs after the constructor which means after the browser is started and logged into the home page.
        [SetUp]
        public void Setup()
        {
            _webDriver.WaitForPageLoad();
            _webDriver.FindElement(By.ClassName("main-content")).FindElement(By.TagName("button")).Click();



            //IWebElement headerElement = _pageHeader.FindElement(By.ClassName("oAuth"));
            //headerElement.Click();
            Thread.Sleep(TimeSpan.FromSeconds(1));
            _webDriver.FindElement(By.Id("id_login_username")).SendKeys("korada.prashanth@gmail.com");
            _webDriver.FindElement(By.Id("id_login_password")).SendKeys("******");
            _webDriver.FindElement(By.Id("signin")).Click();
            Thread.Sleep(TimeSpan.FromSeconds(1));
            //var stepsTitle = _webDriver.FindElement(By.ClassName("dtPageTitle")).Text;
            //Assert.AreEqual("Steps", stepsTitle);
            //_webDriver.FindElement(By.ClassName("ui-datepicker-trigger")).Click();
            //if (_webDriver.FindElement(By.Id("ui-datepicker-div")).Displayed)
            //{
            //    _webDriver.FindElement(By.ClassName("ui-datepicker-month")).Click();
            //    Thread.Sleep(TimeSpan.FromSeconds(1));
            //    _webDriver.FindElement(By.ClassName("ui-datepicker-month")).FindElements(By.TagName("option"))[0].Click();
            //    _webDriver.FindElement(By.CssSelector(".ui-datepicker-calendar tbody tr:nth-of-type(1) td:nth-of-type(2)")).Click();

            //}


        }

        [TearDown]
        public void TearDown()
        {
            // Optional deal with tests that fail
            if (TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Failed) return;

            // Print out that the test failed and take a screenshot.
            TestContext.WriteLine($"Test Failed: {TestContext.CurrentContext.Test.Name}");

            // Using the seleniumConfig TakeFailureScreenshots (ScreenshotConfig:TakeFailureScreenshots)
            // Variable is exposed on the SeleniumTestBase class for your use.
            if (TakeFailureScreenshots)
            {
                // Call TakeScreenshot method on SeleniumTestBase
                TakeScreenshot(screenshotName: TestContext.CurrentContext.Test.Name);
            }
        }

        // Making sure to close the browser after all the tests in the class are ran.
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            CloseWebDriver();
        }

        // Basic Header Test
        [Test]
        public void MyWebsiteHomepage_Header_IsVisible()
        {
            _webDriver.WaitForPageLoad();
            var datesList = GetAllDatesAndInitializeTickets(DateTime.Parse("01/01/2021"), DateTime.Now);

            for (int i = 0; i < datesList.Count; i++)
            {
                
                Thread.Sleep(TimeSpan.FromSeconds(1));
                _webDriver.FindElement(By.XPath("//span[.='Add Entry']")).Click();
                var dateIpunt= _webDriver.FindElement(By.ClassName("react-datepicker__input-container")).FindElements(By.TagName("input"))[1];
                dateIpunt.Clear();
                dateIpunt.SendKeys(datesList[i]);


                var inputs = _webDriver.FindElement(By.TagName("form")).FindElements(By.TagName("input"));

                inputs[2].SendKeys("10000");
                _webDriver.FindElement(By.XPath("//span[.='Save']")).Click();
                Thread.Sleep(TimeSpan.FromSeconds(3));

            }
            
          
        }


        public List<string> GetAllDatesAndInitializeTickets(DateTime startingDate, DateTime endingDate)
        {
            List<string> allDates = new List<string>();


            for (DateTime i = startingDate; i <= endingDate; i = i.AddDays(1))
            {
                allDates.Add(i.ToString("MM/dd/yyyy"));
            }
            return allDates;
        }
    }
}
