using System;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace ContactBookWebDriverTests
{
    public class SeleniumWebDriveTests
    {
        private const string url = "https://contactbook.nakov.repl.co";
        private WebDriver driver;
       


        [SetUp]
        public void OpenBrowser()
        {
            this.driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

        }


        [TearDown]
        public void CloseBrowser()
        {
            this.driver.Quit();

        }

        [Test]
        public void Test_ListContacts_CheckFirstContact()
        {
           
            // Arrange
            driver.Navigate().GoToUrl(url);
            var contactsLink = driver.FindElement(By.LinkText("Contacts"));

            //Act
            contactsLink.Click();

            //Assert
            var firstName = driver.FindElement(By.CssSelector("tr.fname > td")).Text;
            var lastName = driver.FindElement(By.CssSelector("tr.lname > td")).Text;
            

            Assert.That(firstName, Is.EqualTo("Gulia637911601531604123"));
            Assert.That(lastName, Is.EqualTo("GuliaLast637911601531604189"));

        }

        [Test]
        public void Test_SearchContacts_CheckFirstResult()
        {

            // Arrange
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.LinkText("Search")).Click();
            
           
            //Act

            var searchField = driver.FindElement(By.Id("keyword"));
            searchField.SendKeys("albert" + Keys.Enter);
            


            //Assert
            var firstName = driver.FindElement(By.CssSelector("tr.fname > td")).Text;
            var lastName = driver.FindElement(By.CssSelector("tr.lname > td")).Text;


            Assert.That(firstName, Is.EqualTo("Albert"));
            Assert.That(lastName, Is.EqualTo("Einstein"));

        }

        [Test]
        public void Test_SearchContacts_EmptyResult()
        {

            // Arrange
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.LinkText("Search")).Click();


            //Act

            var searchField = driver.FindElement(By.Id("keyword"));
            searchField.SendKeys("invalid2635" + Keys.Enter);



            //Assert
            var searchResult = driver.FindElement(By.Id("searchResult")).Text;

            Assert.That(searchResult, Is.EqualTo("No contacts found."));
           
        }

        [Test]
        public void Test_CreateContact_EmptyLastName()
        {

            // Arrange
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.LinkText("Create")).Click();


            //Act

            var firstNameField = driver.FindElement(By.Id("firstName"));
            // var lastNameField = driver.FindElement(By.Id("lastName"));
            firstNameField.SendKeys("Ivan" + Keys.Enter);


            //Assert
            var searchResult = driver.FindElement(By.CssSelector("body > main > div")).Text;

            Assert.That(searchResult, Is.EqualTo("Error: Last name cannot be empty!"));

        }


        [Test]
        public void Test_CreateContact_ValidData()
        {

            // Arrange
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.LinkText("Create")).Click();

            var firstName = "Petar" + DateTime.Now.Ticks;
            var lastName = "Petrov" + DateTime.Now.Ticks;
            var email = DateTime.Now.Ticks + "pesho@abv.bg";
            var phone = "12345678" + DateTime.Now.Ticks;



            //Act
            driver.FindElement(By.Id("firstName")).SendKeys(firstName);
            driver.FindElement(By.Id("lastName")).SendKeys(lastName);
            driver.FindElement(By.Id("email")).SendKeys(email);
            driver.FindElement(By.Id("phone")).SendKeys(phone);

            driver.FindElement(By.Id("create")).Click();


            //Assert
            var allContacts = driver.FindElements(By.CssSelector("table.contact-entry"));
            var lastContact = allContacts.Last();
            var firstNameLabel = lastContact.FindElement(By.CssSelector("tr.fname > td")).Text;
            var lastNameLabel = lastContact.FindElement(By.CssSelector("tr.lname > td")).Text;

            Assert.That(firstNameLabel, Is.EqualTo(firstName));
            Assert.That(lastNameLabel, Is.EqualTo(lastName));

        }
    }
}