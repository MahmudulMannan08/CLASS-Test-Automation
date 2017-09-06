namespace BLRTestAutomation
{
    using System;
    using System.Text;
    using System.Configuration;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium.Firefox;
    using OpenQA.Selenium.Support.UI;
    using Protractor;
    using TechTalk.SpecFlow;
    using OpenQA.Selenium.Remote;

    [Binding]
    public class TestBase
    {
        public static string HostUrl = ConfigurationManager.AppSettings["HOST_URL"];
        public static string SeleniumDriver = ConfigurationManager.AppSettings["SELENIUM_DRIVER"];
        //public static readonly WebApiServiceHelper api = new WebApiServiceHelper();
        //public static string accessToken = ConfigurationManager.AppSettings["apiKey"];
        private static readonly Random Random = new Random((int)DateTime.Now.Ticks);
        public static DefaultWait<bool> apiWait = new DefaultWait<bool>(new bool())
        {
            Timeout = TimeSpan.FromMinutes(2),
            PollingInterval = TimeSpan.FromSeconds(10)
        };
        public static WebDriverWait Wait;
        public static NgWebDriver driver
        {
            get
            {
                if (!FeatureContext.Current.ContainsKey("browser"))
                {
                    FeatureContext.Current["browser"] = StartBrowser(SeleniumDriver);
                }

                return (NgWebDriver)FeatureContext.Current["browser"];
            }
        }

        public static string RandomString(int size)
        {
            var builder = new StringBuilder();
            for (var i = 0; i < size; i++)
            {
                var ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * Random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }

        public static NgWebDriver StartBrowser(string browser)
        {
            IWebDriver driver;

            if (browser.Equals("Firefox"))
            {
                driver = new FirefoxDriver();
            }
            else
            {
                var options = new ChromeOptions();
                options.AddArguments("test-type");
                driver = new ChromeDriver(options);                
            }

            var ngdriver = new NgWebDriver(driver);
            ngdriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(100));
            ngdriver.Manage().Timeouts().SetScriptTimeout(TimeSpan.FromSeconds(20));
            ngdriver.Manage().Window.Maximize();
            Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            ngdriver.IgnoreSynchronization = true;
            return ngdriver;
        }

        [BeforeScenario]
        public void RecordLog()
        {
            //api.SetApiKey(accessToken);

            if (!ScenarioContext.Current.ContainsKey("report"))
            {
                var listenerId = RandomString(20);
                //var path = Directory.GetCurrentDirectory();
                //var path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                //var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                //var fileName = path + "\\" + FeatureContext.Current.FeatureInfo.Title + "-"
                //+ ScenarioContext.Current.ScenarioInfo.Title + "_" + listenerId + "_.txt";
                var fileName = "Test.txt";
                var textListener = new TextWriterTraceListener(fileName, listenerId);
                ScenarioContext.Current.Add("report", textListener);
                ScenarioContext.Current.Add("report_file_name", fileName);
                ScenarioContext.Current.Add("stepcounter", 1);
                ScenarioContext.Current.Add("timeStarted", DateTime.Now);
                Trace.Listeners.Add(textListener);
                Trace.AutoFlush = true;
                Trace.Indent();
                textListener.WriteLine("-----------------------------------------------------------");
                textListener.WriteLine("START OF SCENARIO: " + ScenarioContext.Current.ScenarioInfo.Title);
                textListener.WriteLine("-----------------------------------------------------------");
            }
        }

        [AfterScenario]
        public void CloseBrowser()
        {
            var textListener = ScenarioContext.Current.Get<TextWriterTraceListener>("report");

            if (!FeatureContext.Current.ContainsKey("browser"))
            {
                return;
            }

            var dateTime1 = ScenarioContext.Current.Get<DateTime>("timeStarted");
            var dateTime2 = DateTime.Now;
            var diff = dateTime2 - dateTime1;

            textListener.WriteLine("END OF SCENARIO: " + ScenarioContext.Current.ScenarioInfo.Title);
            textListener.WriteLine("-----------------------------------------------------------");
            textListener.WriteLine("STARTED AT: " + ScenarioContext.Current["timeStarted"] + ": COMPLETED AT: " + dateTime2 + ": EXECUTION TIME: " + diff);
            textListener.WriteLine("-----------------------------------------------------------");

            if (ScenarioContext.Current.TestError != null)
            {
                textListener.WriteLine(
                    "ERROR OCCURED: " + ScenarioContext.Current.TestError.Message + " OF TYPE: "
                    + ScenarioContext.Current.TestError.GetType().Name);
                textListener.Flush();
                textListener.Close();
                UIHelper.SendEmailOnError(UIHelper.TakeScreenshot());
            }
            else
            {
                textListener.Flush();
                textListener.Close();
            }

            if (ConfigurationManager.AppSettings["VERBOSE_MODE"].Equals("OFF"))
            {
                //UIHelper.SendEmailOnError(UIHelper.TakeScreenshot());
                driver.Quit();
                driver.WrappedDriver.Quit();
            }

            FeatureContext.Current.Remove("browser");
            FeatureContext.Current.Remove("report");
        }
    }
}
