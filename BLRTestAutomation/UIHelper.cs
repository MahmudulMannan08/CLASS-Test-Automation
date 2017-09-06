namespace BLRTestAutomation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Configuration;
    using System.Diagnostics;
    using System.Drawing.Imaging;
    using System.Net;
    using System.Net.Mail;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.Events;
    using OpenQA.Selenium.Support.Extensions;
    using OpenQA.Selenium.Support.PageObjects;
    using Protractor;
    using TechTalk.SpecFlow;
    using BLRTestAutomation.Pages;

    class UIHelper : TestBase
    {
        public static string emailOnError = ConfigurationManager.AppSettings["ERROR_EMAIL"];

        public static T PageInit<T>(NgWebDriver driver) where T : class, new()
        {
            var i = Int32.Parse(ScenarioContext.Current["stepcounter"].ToString());
            var log = ScenarioContext.Current.Get<TextWriterTraceListener>("report");
            var page = new T();
            PageFactory.InitElements(driver, page);
            log.WriteLine("-----------------------------------------------------------");
            ScenarioContext.Current["stepcounter"] = i;

            return page;
        }

        public static void GoTo<T>(string host, bool isAngular) where T : IPage, new()
        {
            var i = int.Parse(ScenarioContext.Current["stepcounter"].ToString());
            var log = ScenarioContext.Current.Get<TextWriterTraceListener>("report");
            var page = new T();
            var url = host + page.Url;
            if (isAngular)
            {
                driver.IgnoreSynchronization = false;
                driver.Navigate().GoToUrl(url);
            }
            else
            {
                driver.WrappedDriver.Navigate().GoToUrl(url);
            }

            log.WriteLine(i++ + ". Then I go to " + driver.Title + " page: " + driver.Url);
            //log.WriteLine(". Then I go to " + driver.Title + " page: " + driver.Url);
            log.WriteLine("-----------------------------------------------------------");
            ScenarioContext.Current["stepcounter"] = i;
        }

        public static IWebElement ElementIsRedy(IWebElement webElement)
        {
            var isReady = webElement.Displayed && webElement.Enabled.Equals(true);
            while (isReady.Equals(false))
            {
                isReady = webElement.Displayed;
            }

            return webElement;
        }

        public static Func<IWebDriver, IWebElement> ElementIsClickable(IWebElement webElement)
        {
            return dr => (webElement.Displayed && webElement.Enabled) ? webElement : null;
        }

        public static IWebDriver ClickOnLink(IWebElement webElement)
        {
            var i = Int32.Parse(ScenarioContext.Current["stepcounter"].ToString());
            var log = ScenarioContext.Current.Get<TextWriterTraceListener>("report");
            log.WriteLine(i++ + ". And I clicked on '" + webElement.Text + "' link");
            //log.WriteLine(". And I clicked on '" + webElement.Text + "' link");
            Wait.Until(ElementIsClickable(webElement)).Click();
            log.WriteLine("-----------------------------------------------------------");
            log.Flush();
            log.Close();
            ScenarioContext.Current["stepcounter"] = i;
            return driver;
        }

        public static IWebDriver ClickOnButton(IWebElement webElement)
        {
            var i = Int32.Parse(ScenarioContext.Current["stepcounter"].ToString());
            var log = ScenarioContext.Current.Get<TextWriterTraceListener>("report");
            log.WriteLine(i++ + ". And I clicked on '" + webElement.Text + "' button");
            //log.WriteLine(". And I clicked on '" + webElement.Text + "' button");
            webElement.Click();
            log.WriteLine("-----------------------------------------------------------");
            log.Flush();
            log.Close();
            ScenarioContext.Current["stepcounter"] = i;
            return driver;
        }

        public static void SetCheckbox(IWebElement webElement, string status)
        {
            var log = ScenarioContext.Current.Get<TextWriterTraceListener>("report");
            var i = Int32.Parse(ScenarioContext.Current["stepcounter"].ToString());
            if (webElement.Selected)
            {
                if (!status.Equals("OFF"))
                {
                    return;
                }

                webElement.Click();
                log.WriteLine(i++ + ".  Then I set checkbox '" + webElement.GetAttribute("id") + "' to *OFF*");
                //log.WriteLine(".  Then I set checkbox '" + webElement.GetAttribute("id") + "' to *OFF*");
                log.WriteLine(".  Then I set checkbox '" + webElement.GetAttribute("id") + "' to *OFF*");
                log.WriteLine("-----------------------------------------------------------");
            }
            else
            {
                if (!status.Equals("ON"))
                {
                    return;
                }

                webElement.Click();
                //log.WriteLine(i++ + ".  Then I set checkbox '" + webElement.GetAttribute("id") + "' to *ON*");
                //log.WriteLine(".  Then I set checkbox '" + webElement.GetAttribute("id") + "' to *ON*");
                log.WriteLine("-----------------------------------------------------------");
            }
        }

        public static void EnterText(IWebElement webElement, string text)
        {
            webElement = new NgWebElement(driver, webElement);
            var log = ScenarioContext.Current.Get<TextWriterTraceListener>("report");
            var i = Int32.Parse(ScenarioContext.Current["stepcounter"].ToString());
            var placeholder = webElement.GetAttribute("placeholder");

            if (placeholder.Length == 0)
            {
                placeholder = webElement.GetAttribute("id");
            }

            if (text.Length > 0)
            {
                if (text[text.Length - 1] > (char)57349)
                {
                    log.WriteLine(
                        i++ + ". And I entered '" + text.TrimEnd().Substring(0, text.Length - 1) + "' in '"
                        + placeholder + "' field");
                    log.WriteLine("-----------------------------------------------------------");
                    log.WriteLine(i++ + ". Then I pressed 'ENTER'");
                }
                else
                {
                    log.WriteLine(i++ + ". And I entered '" + text + "' in '" + placeholder + "' field");
                }
            }
            else
            {
                log.WriteLine(i++ + ". And I left field '" + placeholder + "' blank");
            }
            webElement.Click();
            //webElement.Clear();
            webElement.SendKeys(text);
            log.WriteLine("-----------------------------------------------------------");
            log.Close();
            ScenarioContext.Current["stepcounter"] = i;
        }

        public static void ScrollToElement(IWebDriver Driver, IWebElement element)
        {
            var js = Driver as IJavaScriptExecutor;
            var y = element.Location.Y;
            js.ExecuteScript("javascript:window.scrollBy(0," + y + ")");
        }

        //public static bool EmailRecieved()
        //{
        //    var api = new WebApiServiceHelper();
        //    var requestString = string.Format("https://api.mailinator.com/api/inbox?to={0}&token={1}", ScenarioContext.Current["userName"], "7c5b21b2160b42269075e44ff3b7987f");
        //    return api.Get<MailinatorInbox>(requestString).messages.Count > 0;
        //}

        public static string TakeScreenshot()
        {
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd-hhmm-ss");
            var fileName = "Exception-" + timestamp + ".png";
            var firingDriver = new EventFiringWebDriver(driver.WrappedDriver);
            firingDriver.TakeScreenshot().SaveAsFile(fileName, ImageFormat.Png);
            return fileName;
        }

        public static void SendEmailOnError(string fileName)
        {
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd-hhmm-ss");
            var mail = new MailMessage();
            var SmtpServer = new SmtpClient("smtp.gmail.com");
            mail.From = new MailAddress("itmagnettest1@gmail.com");
            mail.To.Add(emailOnError);
            mail.Subject = timestamp + ": " + ScenarioContext.Current.ScenarioInfo.Title + " scenario has failed.";
            mail.Body = ScenarioContext.Current.ScenarioInfo.Title
                        + " scenario has failed. Please view attachment and attachment name for time when this occured. ";
            var attachment = new Attachment(fileName);
            mail.Attachments.Add(attachment);
            attachment = new Attachment(ScenarioContext.Current["report_file_name"].ToString());
            mail.Attachments.Add(attachment);
            SmtpServer.Port = 587; //TLS
            //SmtpServer.Port = 465; //SSL
            SmtpServer.Credentials = new NetworkCredential("itmagnettest1@gmail.com", "itmagnet03");
            SmtpServer.EnableSsl = true;
            SmtpServer.Send(mail);
        }
    }
}
