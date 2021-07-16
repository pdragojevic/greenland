using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Greenland.Tests
{
    class SeleniumTest
    {
        [Test]
        public void Login_CorrectUsernameAndPassword()
        {
            IWebDriver webDriver = new ChromeDriver();
            //Navigate to site
            webDriver.Navigate().GoToUrl("http://51.103.112.37/");

            //username
            webDriver.FindElement(By.XPath("/html/body/app-root/app-login/html/body/div/form/div[1]/input")).SendKeys("basia.le");
            //password
            webDriver.FindElement(By.XPath("/html/body/app-root/app-login/html/body/div/form/div/input[2]")).SendKeys("basia.le");
            //login button
            webDriver.FindElement(By.XPath("/html/body/app-root/app-login/html/body/div/form/button")).Submit();

            //wait 3 seconds until page loads
            Thread.Sleep(3000);

            var lnkHome = webDriver.Url;

            Assert.AreEqual("http://51.103.112.37/home", lnkHome);

            webDriver.Quit();
        }

        [Test]
        public void Login_WrongPassword()
        {
            IWebDriver webDriver = new ChromeDriver();
            //Navigate to site
            webDriver.Navigate().GoToUrl("http://51.103.112.37/");

            //username
            webDriver.FindElement(By.XPath("/html/body/app-root/app-login/html/body/div/form/div[1]/input")).SendKeys("wrong_username");
            //password
            webDriver.FindElement(By.XPath("/html/body/app-root/app-login/html/body/div/form/div/input[2]")).SendKeys("_");
            //login button
            webDriver.FindElement(By.XPath("/html/body/app-root/app-login/html/body/div/form/button")).Submit();

            //wait 3 seconds until page loads
            Thread.Sleep(3000);

            var lnkHome = webDriver.Url;
            var errorMessage = webDriver.FindElement(By.XPath("/html/body/app-root/app-login/html/body/div/div/span"));

            Assert.AreNotEqual("http://51.103.112.37/home", lnkHome);
            Assert.That(errorMessage.Displayed, Is.True);

            webDriver.Quit();
        }

        [Test]
        public void Login_WrongUsername()
        {
            IWebDriver webDriver = new ChromeDriver();
            //Navigate to site
            webDriver.Navigate().GoToUrl("http://51.103.112.37/");

            //username
            webDriver.FindElement(By.XPath("/html/body/app-root/app-login/html/body/div/form/div[1]/input")).SendKeys("basia.le");
            //password
            webDriver.FindElement(By.XPath("/html/body/app-root/app-login/html/body/div/form/div/input[2]")).SendKeys("wrongPassword");
            //login button
            webDriver.FindElement(By.XPath("/html/body/app-root/app-login/html/body/div/form/button")).Submit();

            //wait 3 seconds until page loads
            Thread.Sleep(3000);

            var lnkHome = webDriver.Url;
            var errorMessage = webDriver.FindElement(By.XPath("/html/body/app-root/app-login/html/body/div/div/span"));

            Assert.AreNotEqual("http://51.103.112.37/home", lnkHome);
            Assert.That(errorMessage.Displayed, Is.True);

            webDriver.Quit();
        }

        [Test]
        public void ChangePassword()
        {
            IWebDriver webDriver = new ChromeDriver();
            //Navigate to site
            webDriver.Navigate().GoToUrl("http://51.103.112.37/");

            //username
            webDriver.FindElement(By.XPath("/html/body/app-root/app-login/html/body/div/form/div[1]/input")).SendKeys("basia.le");
            //password
            webDriver.FindElement(By.XPath("/html/body/app-root/app-login/html/body/div/form/div/input[2]")).SendKeys("basia.le");
            //login button
            webDriver.FindElement(By.XPath("/html/body/app-root/app-login/html/body/div/form/button")).Submit();

            Thread.Sleep(2000);
            //navigate to profile page
            webDriver.FindElement(By.XPath("/html/body/app-root/div/div/div/nav/div/ul[2]/li/a")).Click();
            webDriver.FindElement(By.XPath("/html/body/app-root/div/div/div/nav/div/ul[2]/li/div/a[1]")).Click();

            Thread.Sleep(2000);
            //navigate to change password form
            webDriver.FindElement(By.XPath("/html/body/app-root/app-profile/html/body/div/div[10]/div/div[2]/button")).Click();

            Thread.Sleep(2000);
            //enter old and new password
            var oldPassword = "basia.le";
            var newPassword = "newPassword";
            webDriver.FindElement(By.XPath("/html/body/app-root/app-change-password/html/body/div/form/div[1]/input")).SendKeys(oldPassword);
            webDriver.FindElement(By.XPath("/html/body/app-root/app-change-password/html/body/div/form/div[2]/input")).SendKeys(newPassword);
            webDriver.FindElement(By.XPath("/html/body/app-root/app-change-password/html/body/div/form/div[3]/input")).SendKeys(newPassword);

            webDriver.FindElement(By.XPath("/html/body/app-root/app-change-password/html/body/div/form/div[4]/button")).Submit();

            Thread.Sleep(2000);
            var lnkProfile1 = webDriver.Url;

            Assert.AreEqual("http://51.103.112.37/profile", lnkProfile1);

            //change password to previous value
            webDriver.FindElement(By.XPath("/html/body/app-root/app-profile/html/body/div/div[10]/div/div[2]/button")).Click();

            Thread.Sleep(2000);
            webDriver.FindElement(By.XPath("/html/body/app-root/app-change-password/html/body/div/form/div[1]/input")).SendKeys(newPassword);
            webDriver.FindElement(By.XPath("/html/body/app-root/app-change-password/html/body/div/form/div[2]/input")).SendKeys(oldPassword);
            webDriver.FindElement(By.XPath("/html/body/app-root/app-change-password/html/body/div/form/div[3]/input")).SendKeys(oldPassword);

            webDriver.FindElement(By.XPath("/html/body/app-root/app-change-password/html/body/div/form/div[4]/button")).Submit();

            Thread.Sleep(2000);
            var lnkProfile2 = webDriver.Url;

            Assert.AreEqual("http://51.103.112.37/profile", lnkProfile2);

            webDriver.Quit();
        }
        
        [Test]
        public void CreateAMeetingByDeveloper()
        {
            IWebDriver webDriver = new ChromeDriver();
            //Navigate to site
            webDriver.Navigate().GoToUrl("http://51.103.112.37/");

            //username
            webDriver.FindElement(By.XPath("/html/body/app-root/app-login/html/body/div/form/div[1]/input")).SendKeys("danny.green");
            //password
            webDriver.FindElement(By.XPath("/html/body/app-root/app-login/html/body/div/form/div/input[2]")).SendKeys("danny.green");
            //login button
            webDriver.FindElement(By.XPath("/html/body/app-root/app-login/html/body/div/form/button")).Submit();

            Thread.Sleep(3000);
            //click on some date in calendar
            webDriver.FindElement(By.XPath
                ("/html/body/app-root/app-home/html/body/div/div/div/div/div[2]/app-calendar/div/div[1]/full-calendar/div[2]/div/table/tbody/tr/td/div/div/div/table/tbody/tr[3]/td[6]/div"))
                .Click();

            Thread.Sleep(1000);
            //switch to alert window
            var alert = webDriver.SwitchTo().Alert();

            Assert.AreEqual(alert.Text, "You don't have privilege to create new meetings.");

            alert.Accept();

            webDriver.Quit();
        }

    }
}
