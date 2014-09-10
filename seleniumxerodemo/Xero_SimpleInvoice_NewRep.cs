using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using Selenium;

/*
 Purpose of the script:
 This Automation script tests the XERO application's Repeating Invoice functionality.
 This script specifically tests the creation of the repeating invoices using the invoicing form.
 
 Expected Result:
 The Repeating Invoice gets created and the test passes when the invoice is listed in the Invoices page.
*/

namespace SeleniumTests
{
    [TestFixture]
    public class XeroSimple
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
        public void XeroSimple_NewRepInvoice()
        {
            selenium.Open("/");

            //Maximizing the window
            selenium.WindowMaximize();

            //check for "Welcome to XERO" text on the Login Page
            for (int second = 0; ; second++)
            {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if ("Welcome to Xero" == selenium.GetText("css=h2.x-boxed.noBorder")) break;
                }
                catch (Exception)
                { }
                Thread.Sleep(1000);
            }
            try
            {
                Assert.AreEqual("Welcome to Xero", selenium.GetText("css=h2.x-boxed.noBorder"));
            }
            catch (AssertionException e)
            {
                verificationErrors.Append(e.Message);
            }

            //Entering the credentials to login
            selenium.Type("id=email", "sanjanatiw@gmail.com");
            selenium.Type("id=password", "swamiji123");
            selenium.Click("id=submitButton");
            selenium.WaitForPageToLoad("30000");

            //Check for the organization name as"Demo NZ" on the following page
            for (int second = 0; ; second++)
            {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if ("Demo NZ" == selenium.GetText("id=title")) break;
                }
                catch (Exception)
                { }
                Thread.Sleep(1000);
            }
            try
            {
                Assert.AreEqual("Demo NZ", selenium.GetText("id=title"));
            }
            catch (AssertionException e)
            {
                verificationErrors.Append(e.Message);
            }

            // On the DashBoard page, Clicking on the "Go to Sales" Link
            selenium.Click("link=Go to Sales ›");
            selenium.WaitForPageToLoad("30000");

            //Checking whether the Sales page is opened
            for (int second = 0; ; second++)
            {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if ("Sales" == selenium.GetText("css=#page_title > div > h1")) break;
                }
                catch (Exception)
                { }
                Thread.Sleep(1000);
            }
            try
            {
                Assert.AreEqual("Sales", selenium.GetText("css=#page_title > div > h1"));
            }
            catch (AssertionException e)
            {
                verificationErrors.Append(e.Message);
            }

            //Click on the repeating invoice option in the dropdown on the Sales page
            selenium.Click("css=#ext-gen1037 > span");
            selenium.Click("link=Repeating invoice");
            selenium.WaitForPageToLoad("30000");

            //Checking whether the New Repeating Invoice is opened 
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

            //Enter all the valid values for the Invoice page
            selenium.Type("id=StartDate", "12 Sep 2014");
            selenium.Type("id=DueDateDay", "1");
            selenium.Click("id=DueDateType_toggle");
            selenium.Click("//div[@id='DueDateType_suggestions']/div/div[2]");
            selenium.Click("id=saveAsAutoApproved");
            selenium.Type("//div[@id='ext-gen48']/input", "Associated Limited");
            selenium.Type("//div[@id='invoiceForm']/div/div[2]/div/input", "Sanjana");
            Thread.Sleep(300);

            //Enter the Items in the  first row of the Table from the inventory
            selenium.Click("//div[@id='ext-gen19']/div/table/tbody/tr/td[2]/div");
            selenium.Click("//div[2]/div/img");
            selenium.Click("//div[8]/div/div[4]");

            //Enter the Items in the  second row of the Table from the inventory
            selenium.Click("//div[@id='ext-gen19']/div[2]/table/tbody/tr/td[2]/div");
            selenium.Click("//div[2]/div/img");
            selenium.Click("//div[8]/div/div[5]");

            //Deleting all the un- necessary rows from the table
            selenium.Click("//div[@id='ext-gen19']/div[3]/table/tbody/tr/td[11]/div/div/div");
            selenium.Click("//div[@id='ext-gen19']/div[3]/table/tbody/tr/td[11]/div/div/div");
            selenium.Click("//div[@id='ext-gen19']/div[3]/table/tbody/tr/td[11]/div/div/div");

            //Save the invoice
            selenium.Click("xpath=(//button[@type='button'])[3]");

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
