namespace BLRTestAutomation.Pages
{
    using System.Threading;
    using NUnit.Framework;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.PageObjects;
    using Protractor;

    public class HomePage : IPage
    {
        public string Url
        {
            get
            {
                return "/";
            }
        }

        //[FindsBy(How = How.Id, Using = "LayoutSplitter_HomeLinksMenu_DXI4_T")]
        //[FindsBy(How = How.XPath, Using = "//a[@id='LayoutSplitter_HomeLinksMenu_DXI4_T']/span")]
        //public IWebElement HomemenuBtn;

        public string GetLoggedinUsername(IWebDriver driver)
        {
            var elapsed = 0;
            var timeSpan = 4000;
            while (driver.PageSource.Contains("//a[@id='LayoutSplitter_HomeLinksMenu_DXI4_T']/span") == false && (elapsed < timeSpan))
            {
                Thread.Sleep(1000);
                elapsed += 1000;
            }

            //return driver.PageSource.Contains("//a[@id='LayoutSplitter_HomeLinksMenu_DXI4_T']/span") ? this.HomemenuBtn.Text : "Username not found, there user is not logged in";
            //return HomemenuBtn.Text.Length > 0 ? this.HomemenuBtn.Text : "Username not found, there user is not logged in";
            IWebElement HomemenuBtn = driver.FindElement(By.XPath("//a[@id='LayoutSplitter_HomeLinksMenu_DXI4_T']/span"));
            return HomemenuBtn.Text.Length > 0 ? HomemenuBtn.Text : "Username not found, therefore user is not logged in";
        }

        public bool UserIsLogged(IWebDriver driver)
        {
            var elapsed = 0;
            var timeSpan = 6000;
            while (driver.PageSource.Contains("//a[@id='LayoutSplitter_HomeLinksMenu_DXI4_T']/span") == false && (elapsed < timeSpan))
            {
                Thread.Sleep(1000);
                elapsed += 1000;
            }

            return driver.PageSource.Contains("//a[@id='LayoutSplitter_HomeLinksMenu_DXI4_T']/span") ? true : false;
        }

        public void Logout(NgWebDriver driver)
        {
            var js = driver.WrappedDriver as IJavaScriptExecutor;
            js.ExecuteScript("javascript:SignoutUser();");
        }
    }
}
