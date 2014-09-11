using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using Selenium;

/*
 Purpose of the script:
 * This Automation script tests the XERO application's Repeating Invoice functionality.
 * This script specifically tests the additional flow of drafting an Email for the customer and it also tests the deletion of all invoices created 
   along with the creation of the repeating invoices using the invoicing form. 
 * This script also deletes the unused rows in the Items table on the New Repeating Invoice page.
 
 Expected Result:
   The Repeating Invoice gets created along with the option of Approve for sending option and the test passes when it meets all the specified criterias.
*/

namespace SeleniumTests
{
    [TestFixture]
    public class Xero_DraftEmail_NewRepInvoice
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
        public void TheXero_DraftEmail_NewRepInvoiceTest()
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
            selenium.Click("link=Repeating invoice");
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

            // Entering all the valid inputs on the page. 
            string InvoiceDate = DateTime.Now.AddDays(1).ToString("dd MMM yyyy");
            selenium.Type("id=StartDate", InvoiceDate);
            selenium.Type("id=DueDateDay", "1");
            selenium.Click("id=DueDateType_toggle");
            selenium.Click("//div[@id='DueDateType_suggestions']/div/div[2]");

            //Checking the Drafting of Email facility of the Invoice page when we choose the Approve for sending option on the New Repeating Invoice page.
            selenium.Click("id=saveAsAutoApprovedAndEmail");
            for (int second = 0; ; second++)
            {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if ("" == selenium.GetText("//form[@id='frmEmailStatements']/div/div/div/div/div[3]/input")) break;
                }
                catch (Exception)
                { }
                Thread.Sleep(1000);
            }

            //Entering the Email Id and all the other details of the customer
            selenium.Type("//form[@id='frmEmailStatements']/div/div/div/div/div[3]/input", "prabodhtiwari@gmail.com");
            selenium.Type("//form[@id='frmEmailStatements']/div/div/div/div/div[9]/div[6]/textarea", "Hi Prabodh,\n\nHere's your [Current Month] invoice [Invoice Number] for [Invoice Total].\n\nThe amount outstanding of [Amount Due] is due on [Due Date].\n\nView and pay your bill online: [Online Invoice Link]\n\nFrom your online bill you can print a PDF, export a CSV, or create a free login and view your outstanding bills.\n\nIf you have any questions, please let us know.\n\nThanks,\n[Trading Name]");
            Thread.Sleep(1000);
            selenium.Click("//div/button");
            selenium.Type("//div[@id='ext-gen48']/input", "associated limited");
            selenium.Type("//div[@id='invoiceForm']/div/div[2]/div/input", "sanjana");
            Thread.Sleep(1000);

            //Enter data in the first row of the Item's table from the inventory
            selenium.Click("//div[@id='ext-gen19']/div/table/tbody/tr/td[2]/div");
            selenium.Click("//div[2]/div/img");
            selenium.Click("//div[@id='ext-gen84']/div[5]");

            //Deleting the unused rows
            selenium.Click("//div[@id='ext-gen19']/div[2]/table/tbody/tr/td[11]/div/div/div");
            selenium.Click("//div[@id='ext-gen19']/div[2]/table/tbody/tr/td[11]/div/div/div");
            selenium.Click("//div[@id='ext-gen19']/div[2]/table/tbody/tr/td[11]/div/div/div");
            selenium.Click("//div[@id='ext-gen19']/div[2]/table/tbody/tr/td[11]/div/div/div");

            //Saving the data
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

            // Deleting all the invoices made
            selenium.Click("//table[@id='ext-gen48']/thead/tr/td/input");
            selenium.Click("css=#ext-gen45 > span.text");
            selenium.Click("//div[2]/div[2]/a/span");
            selenium.WaitForPageToLoad("30000");

            //Checking if the repeated transactions are deleted
            for (int second = 0; ; second++)
            {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if ("There are no items to display." == selenium.GetText("//div[7]")) break;
                }
                catch (Exception)
                { }
                Thread.Sleep(1000);
            }
            try
            {
                Assert.AreEqual("There are no items to display.", selenium.GetText("//div[7]"));
            }
            catch (AssertionException e)
            {
                verificationErrors.Append(e.Message);
            }
        }
    }
}
