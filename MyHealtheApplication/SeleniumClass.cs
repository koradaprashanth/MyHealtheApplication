using System;
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
            _webDriver.Navigate().GoToUrl("https://healtheatcernerportal.cerner.com/dt/nutr/PedometerEntry.asp?BID=38715");

        }

        // Optional SetUp method to wait for page to load and/or other setup actions prior to each test method run.
        // This runs after the constructor which means after the browser is started and logged into the home page.
        [SetUp]
        public void Setup()
        {
            _webDriver.WaitForPageLoad();
            _pageHeader = _webDriver.FindElement(By.ClassName("PageHeader"));
            IWebElement headerElement = _pageHeader.FindElement(By.ClassName("oAuth"));
            headerElement.Click();
            Thread.Sleep(TimeSpan.FromSeconds(1));
            _webDriver.FindElement(By.Id("id_login_username")).SendKeys("youremailaddresshere");
            _webDriver.FindElement(By.Id("id_login_password")).SendKeys("yourpasswordhere");
            _webDriver.FindElement(By.Id("signin")).Click();
            Thread.Sleep(TimeSpan.FromSeconds(1));
            var stepsTitle = _webDriver.FindElement(By.ClassName("dtPageTitle")).Text;
            Assert.AreEqual("Steps", stepsTitle);
            _webDriver.FindElement(By.ClassName("ui-datepicker-trigger")).Click();
            if (_webDriver.FindElement(By.Id("ui-datepicker-div")).Displayed)
            {
                _webDriver.FindElement(By.ClassName("ui-datepicker-month")).Click();
                Thread.Sleep(TimeSpan.FromSeconds(1));
                _webDriver.FindElement(By.ClassName("ui-datepicker-month")).FindElements(By.TagName("option"))[0].Click();
                _webDriver.FindElement(By.CssSelector(".ui-datepicker-calendar tbody tr:nth-of-type(1) td:nth-of-type(2)")).Click();

            }


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
            do
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));
                _webDriver.FindElement(By.Id("DateSelectorRight")).Click();
                _webDriver.FindElement(By.Id("steps")).SendKeys("10000");
                IWebElement inputElement1 = _webDriver.FindElement(By.ClassName("data-entry-grid-row")).FindElement(By.TagName("input"));
                inputElement1.Submit();
                Thread.Sleep(TimeSpan.FromSeconds(1));
            } while (_webDriver.FindElement(By.Id("DateSelectorRight")).GetAttribute("title")!= "Cannot select future dates");
          
        }
    }
}
