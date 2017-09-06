using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using NUnit.Framework;
using System.Threading;
using OpenQA.Selenium.Interactions;
using Protractor;
using System.Diagnostics;
using System.IO;
using System.Configuration;
using System.Windows.Forms;

namespace BLRTestAutomation.Pages
{
    public class LoginPage : IPage
    {
        [FindsBy(How = How.Id, Using = "roundPanelLogin_itmLoginControl_txtUsername_I")]
        public IWebElement UserNamefield;

        [FindsBy(How = How.Id, Using = "roundPanelLogin_itmLoginControl_txtPassword_I_CLND")]
        public IWebElement Passwordfield;

        [FindsBy(How = How.Id, Using = "roundPanelLogin_itmLoginControl_btnLogin")]
        public IWebElement LoginBtn;

        [FindsBy(How = How.Id, Using = "roundPanelLogin_itmLoginControl_chkRememberMe_S_D")]
        public IWebElement StaysignedCheckbox;

        public string Url
        {
            get
            {
                return "";
            }
        }

        public static void AutoITProc(string ProcName)
        {
            var CurrentPath = Directory.GetCurrentDirectory();
            var AutoIT = CurrentPath.Remove(CurrentPath.LastIndexOf("\\") + 1) + "AutoIT Files\\" + ProcName;

            try
            {
                var upload = new ProcessStartInfo(@AutoIT)
                {
                    UseShellExecute = false,
                };
                Process.Start(upload);
            }
            catch (IOException)
            {
                MessageBox.Show("There was an error and AutoIT process could not run!");
            }
        }

        public void ProvideCredentials(string username, string password, bool staysigned)
        {
            UIHelper.EnterText(this.UserNamefield, username);

            if(staysigned)
            {
                UIHelper.SetCheckbox(this.StaysignedCheckbox, "ON");
            }

            //UIHelper.EnterText(this.Passwordfield, password + Keys.Enter);
            UIHelper.ClickOnLink(this.Passwordfield);

            Thread.Sleep(1000);           
            AutoITProc(ConfigurationManager.AppSettings["PROC1"]);

            //TestBase.driver.IgnoreSynchronization = false;
        }
        

        public void VerifyLoggedOut(IWebDriver driver)
        {
            Assert.IsTrue(this.LoginBtn.Displayed);
        }
    }
}
