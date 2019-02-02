using System;
using OpenQA.Selenium.Chrome;
using System.Threading;
using NUnit.Framework;
using System.Diagnostics;
using System.IO;

namespace UnitTestProject1
{
    [TestFixture]
    //[Parallelizable(ParallelScope.Self)]
    public class UnitTest1
    {
        public bool IsInSilentMode { get; set; }
        public int NumberOfRuns { get; set; }

        ChromeOptions options;
        ChromeDriverService service;

        string logFile = Path.GetTempPath() + "test.logger.txt";

        [SetUp]
        public void BeforeEach()
        {
            TestContext.AddTestAttachment(logFile, "log");

            service = ChromeDriverService.CreateDefaultService();

            options = new ChromeOptions
            {
                PageLoadStrategy = OpenQA.Selenium.PageLoadStrategy.Default,

            };

            //silent mode
            if (IsInSilentMode)
            {
                service.HideCommandPromptWindow = true;
                options.AddArgument("headless");
                
               
            }
        }


        [Category("LongRunning")]
        [Parallelizable(ParallelScope.Self)]
        [Test,Order(1)]
        public void TestMethod1()
        {

            OpenBrowser((browser) => {

                browser.FindElementByName("q").SendKeys("ynet");
                browser.FindElementByName("btnK").Submit();

                Thread.Sleep(1000);

                browser.FindElementByPartialLinkText("ynet").Click();

                Assert.Pass("TestMethod1 Success");

                //Assert.That(browser.Title.Contains("ccc"), Is.True, "TestMethod1: The title is wrong");

            });


        }


        [Category("LongRunning")]
        [Parallelizable(ParallelScope.Self)]
        [Test,Order(2),Repeat(1)]
        public void TestMethod2()
        {
            OpenBrowser((browser) => {

                browser.FindElementByName("q").SendKeys("facebook");
                browser.FindElementByName("btnK").Submit();

                Thread.Sleep(1000);

                browser.FindElementByPartialLinkText("facebook").Click();

                Assert.Pass("We've found Facebook");

            });


        }

        [Category("LongRunning")]
        [Parallelizable(ParallelScope.Self)]
        [Test, Order(3), Repeat(1)]
        public void TestMethod3()
        {
            OpenBrowser((browser) => {

                browser.FindElementByName("q").SendKeys("facebook");
                browser.FindElementByName("btnK").Submit();

                Thread.Sleep(1000);

                browser.FindElementByPartialLinkText("facebook").Click();

                Assert.That(browser.Title.Contains("ccc"), Is.True, "The title is wrong");
            });
        }

        private void OpenBrowser(Action<ChromeDriver> browserAction)
        {
            //ChromeOptions options = new ChromeOptions
            //{
            //    PageLoadStrategy = OpenQA.Selenium.PageLoadStrategy.Normal
            //};

            using (ChromeDriver driver = new ChromeDriver(service,options))
            {
                driver.Navigate().GoToUrl("http://www.google.com");

                driver.Manage().Window.Maximize();

                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);

                try
                {
                    browserAction(driver);
                }

                catch (SuccessException ex)
                {
                    WriteToLog(ex.ResultState.Status.ToString(), ex.Message);
                }

                catch (AssertionException ex)
                {
                    WriteToLog(ex.ResultState.Status.ToString(), ex.Message);
                }

                catch(Exception ex)
                {

                }


                finally
                {
                    driver.Quit();
                }

            }
        }


        public void CheckConsoleOutput()
        {
            Console.WriteLine("Write to log");
           
            Debug.WriteLine("Debug to log");
            
        }

        private void WriteContextResultToLog()
        {
            File.AppendAllText(logFile, string.Format("{0} - {1}: {2}: {3}\r\n",
              DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"),
              TestContext.CurrentContext.Test.MethodName,
              TestContext.CurrentContext.Result.Outcome, 
              TestContext.CurrentContext.Result.Message));
        }

        private void WriteToLog(string status,string message)
        {
            File.AppendAllText(logFile, string.Format("{0} - {1}: {2}\r\n",
              DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"),
              status,message));
        }

        [TearDown]
        public void AfterEach()
        {
           // WriteResultToLog();

            CheckConsoleOutput();
        }

        [OneTimeTearDown]
        public void AfterAll()
        {
           // WriteResultToLog();
        }
    }

   

}
