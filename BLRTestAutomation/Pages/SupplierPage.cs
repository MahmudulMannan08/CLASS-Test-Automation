using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System.Configuration;

namespace BLRTestAutomation.Pages
{
    public class SupplierPage : IPage
    {
        public string Url
        {
            get
            {
                return ConfigurationManager.AppSettings["SUPPLIER"];
            }
        }
    }
}
