using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using Selenium;

/*
 Purpose of the script:
 * This Automation script tests the XERO application's Repeating Invoice functionality.
 * This script specifically tests the additional flow of adding new items in the Inventory 
   along with the creation of the repeating invoices using the invoicing form. The item code has been randomly created by using javascript method.
 
 Expected Result:
   The Repeating Invoice gets created with the new item added to the inventory.
*/
namespace SeleniumTests
{
    [TestFixture]
    public class Xero_NewInventory_NewRepInvoice
    {
        private ISelenium selenium;
        private StringBuilder verificationErrors;

        [SetUp]
        public void SetupTest()
        {
            selenium = new DefaultSelenium("localhost", 4444, "*chrome", "https://login.xero.com/");
            selenium.Start();
            verificationErrors = new StringBuilder();
        }

        [TearDown]
        public void TeardownTest()
        {
            try
            {
                selenium.Stop();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
            Assert.AreEqual("", verificationErrors.ToString());
        }

        [Test]
        public void TheXero_NewInventory_NewRepInvoiceTest()
        {
            selenium.Open("/");

            //Maximizing the window
            selenium.WindowMaximize();

            //check for "Welcome to XERO" text on the Login Page
            try
            {
                Assert.AreEqual("Welcome to Xero", selenium.GetText("css=h2.x-boxed.noBorder"));
            }
            catch (AssertionException e)
            {
                verificationErrors.Append(e.Message);
            }

            //Entering the credentials for Login
            selenium.Type("id=email", "sanjanatiw@gmail.com");
            selenium.Type("id=password", "swamiji123");
            selenium.Click("id=submitButton");
            selenium.WaitForPageToLoad("30000");

            // Checking for the organization name "Demo NZ" on the DashBoard
            try
            {
                Assert.AreEqual("Demo NZ", selenium.GetText("id=title"));
            }
            catch (AssertionException e)
            {
                verificationErrors.Append(e.Message);
            }

            // Navigate to the Sales page through the Accounts Tab
            selenium.Click("id=Accounts");
            selenium.Click("link=Sales");
            selenium.WaitForPageToLoad("30000");

            // Check whether the Sales page is opened
            try
            {
                Assert.AreEqual("Sales", selenium.GetText("css=#page_title > div > h1"));
            }
            catch (AssertionException e)
            {
                verificationErrors.Append(e.Message);
            }

            //Navigate to the New Repeating Invoice Page by clicking the repeating invoice option in the dropdown
            selenium.Click("css=#ext-gen1037 > span");
            selenium.Click("css=li.ico-repeating-invoice > a > b");
            selenium.WaitForPageToLoad("30000");

            //Checking if the New Repeating Invoice page is opened
            for (int second = 0; ; second++)
            {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if ("Demo NZ - New Repeating Invoice" == selenium.GetText("//div[@id='bodyContainer']/div/div/h1")) break;
                }
                catch (Exception)
                { }
                Thread.Sleep(1000);
            }
            try
            {
                Assert.AreEqual("Demo NZ - New Repeating Invoice", selenium.GetText("//div[@id='bodyContainer']/div/div/h1"));
            }
            catch (AssertionException e)
            {
                verificationErrors.Append(e.Message);
            }

            // Entering all the valid inputs on the page. Please change the start date(write the next day's date on which you are executing) in the script before executing it.
            selenium.Type("id=StartDate", "12 Sep 2014");
            selenium.Click("id=DueDateDay");
            selenium.Type("id=DueDateDay", "1");
            selenium.Click("id=DueDateType_toggle");
            selenium.Click("//div[@id='DueDateType_suggestions']/div/div[2]");
            selenium.Click("id=saveAsAutoApproved");
            selenium.Type("//div[@id='ext-gen48']/input", "associates");
            selenium.Click("//div[@id='invoiceForm']/div/div[2]/div/input");
            selenium.Type("//div[@id='invoiceForm']/div/div[2]/div/input", "sanjana");

            // Enter data in the Item's table by adding a new item in the Inventory.
            selenium.Click("//tbody/tr/td[2]/div");
            selenium.Click("//div[2]/div/img");
            selenium.Click("//div[8]/div/div/span");
            for (int second = 0; ; second++)
            {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if ("Item Code" == selenium.GetText("css=div.field.code > label")) break;
                }
                catch (Exception)
                { }
                Thread.Sleep(1000);
            }
            try
            {
                Assert.AreEqual("Item Code", selenium.GetText("css=div.field.code > label"));
            }
            catch (AssertionException e)
            {
                verificationErrors.Append(e.Message);
            }

            //Entering the item code,Item name,unit Price and Description of your chioce before executing.
            // Using javascript method to randomly create the item code.
            selenium.Type("//div[@id='lineItem']/div/div/div/input", "javascript{Math.floor(Math.random()*9000)+1000}");
            selenium.Type("//div[@id='lineItem']/div/div[2]/div/input", "apples");
            selenium.Type("//div[@id='lineItem']/div[3]/div[2]/div/div/input", "40");
            selenium.Click("id=Account_toggle");
            selenium.Click("//div[@id='Account_suggestions']/div/div[12]");
            selenium.Type("//div[@id='lineItem']/div[3]/div[2]/div[4]/div/textarea", "board");
            selenium.Click("link=Save");

            Thread.Sleep(5000);

            //Save the invoice
            selenium.Click("xpath=(//button[@type='button'])[3]");
            selenium.WaitForPageToLoad("30000");


            //Checking whether the invoice is created
            for (int second = 0; ; second++)
            {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if ("Repeating Template Saved. Click to view." == selenium.GetText("//div[@id='notify01']/div/p")) break;
                }
                catch (Exception)
                { }
                Thread.Sleep(1000);
            }
            try
            {
                Assert.AreEqual("Repeating Template Saved. Click to view.", selenium.GetText("//div[@id='notify01']/div/p"));
            }
            catch (AssertionException e)
            {
                verificationErrors.Append(e.Message);
            }
        }
    }
}
