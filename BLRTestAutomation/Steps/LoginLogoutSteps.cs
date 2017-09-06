using System;
using System.Threading;
using NUnit.Framework;
using TechTalk.SpecFlow;
using BLRTestAutomation.Pages;

namespace BLRTestAutomation.Steps
{
    [Binding]
    public class LoginLogoutSteps : TestBase
    {
        public static AdminPage landingpage;
        public static ClientPage clientpage;
        public static SupplierPage supplierpage;
        public static LoginPage loginpage;
        public static HomePage homepage;

        [Given(@"I go to BLR Admin portal")]
        public void GivenIGoToBLRAdminPortal()
        {
            driver.IgnoreSynchronization = true;

            if (FeatureContext.Current.ContainsKey("error"))
            {
                return;
            }

            UIHelper.GoTo<AdminPage>(HostUrl, false);
            Thread.Sleep(1000);
        }

        [Given(@"I go to BLR Client portal")]
        public void GivenIGoToBLRClientPortal()
        {
            driver.IgnoreSynchronization = true;

            if (FeatureContext.Current.ContainsKey("error"))
            {
                return;
            }

            UIHelper.GoTo<ClientPage>(HostUrl, false);
        }

        [Given(@"I go to BLR Supplier portal")]
        public void GivenIGoToBLRSupplierPortal()
        {
            driver.IgnoreSynchronization = true;

            if (FeatureContext.Current.ContainsKey("error"))
            {
                return;
            }

            UIHelper.GoTo<SupplierPage>(HostUrl, false);
        }
        
        [Then(@"I provide my (.*) and (.*) and (.*) and press Sign in button")]
        public void ThenIProvideMySpiderAndItmagnetAndFalseAndPressSignInButton(string username, string password, bool staysigned)
        {
            loginpage = UIHelper.PageInit<LoginPage>(driver);
            loginpage.ProvideCredentials(username, password, staysigned);
        }

        [Then(@"I verify I see (.*) and I am logged (.*)")]
        public void ThenIVerifyISeeSpiderProjectManagerAndIAmLoggedTrue(string myusername, bool logged)
        {
            if(logged)
            {
                homepage = UIHelper.PageInit<HomePage>(driver);
                Assert.That(homepage.GetLoggedinUsername(driver), Does.Contain(myusername));
            }
        }

        [Then(@"I click logout link and verify I am logged out")]
        public void ThenIClickLogoutLinkAndVerifyIAmLoggedOut()
        {
            homepage = UIHelper.PageInit<HomePage>(driver);
            homepage.Logout(driver);
            loginpage.VerifyLoggedOut(driver);
        }

    }
}
