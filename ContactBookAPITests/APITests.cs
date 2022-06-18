using System.Net;
using NUnit.Framework;
using RestSharp;
using System;
using System.Linq;
using System.Text.Json;
using System.Collections.Generic;

namespace ContactBookAPITests
{
    public class APITests
    {
        private const string url = "https://contactbook.nakov.repl.co/api/contacts";
        private RestClient client;
        private RestRequest request;
        

        [SetUp]

        public void Setup()
        {
           this. client = new RestClient();
            
        }

        [Test]
        public void Test_ListAllContacts_CheckSomeContact()
        {

            //Arrange
            this.request = new RestRequest(url);

            // Act
            var response = this.client.Execute(request);
            var contacts = JsonSerializer.Deserialize<List<Contacts>>(response.Content);
           
            // Assert

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(contacts[56].firstName, Is.EqualTo("Steve"));
            Assert.That(contacts[56].lastName, Is.EqualTo("Jobs"));

        }

        [Test]
        public void Test_SearchlContacts_CheckFirstResult()
        {

            //Arrange
            this.request = new RestRequest(url + "/search/{keyword}");
            request.AddUrlSegment("keyword", "Isaac");

            // Act
            var response = this.client.Execute(request);
            var contacts = JsonSerializer.Deserialize<List<Contacts>>(response.Content);

            // Assert
                       
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(contacts.Count, Is.GreaterThan(0));
            Assert.That(contacts[0].firstName, Is.EqualTo("Isaac"));
            Assert.That(contacts[0].lastName, Is.EqualTo("Newton"));

        }


        [Test]
        public void Test_SearchContacts_EmptyResult()
        {

            //Arrange
            this.request = new RestRequest(url + "/search/{keyword}");
            request.AddUrlSegment("keyword", "missing12345678");

            // Act
            var response = this.client.Execute(request, Method.Get);
            var contacts = JsonSerializer.Deserialize<List<Contacts>>(response.Content);

            // Assert

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(contacts.Count, Is.EqualTo(0));
         

        }

        [Test]
        public void Test_CreateContacts_EmptyLastName()
        {

            //Arrange
            this.request = new RestRequest(url);
            var body = new
            {
                firstName = "Yulia",
                email = "yulia@abv.bg",
                phone = "12345678"
            };

            request.AddJsonBody(body);

            // Act
            var response = this.client.Execute(request, Method.Post);
           

            // Assert

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(response.Content, Is.EqualTo("{\"errMsg\":\"Last name cannot be empty!\"}"));

        }

        [Test]
         public void Test_CreateContacts_Valid()
        {

            //Arrange
            this.request = new RestRequest(url);
            var body = new
            {
                firstName = "Ivan" + DateTime.Now.Ticks,
                lastName = "Ivanov" + DateTime.Now.Ticks,
                email = +DateTime.Now.Ticks + "gulia@abv.bg",
                phone = "12345678" + DateTime.Now.Ticks,
            };
            
            request.AddJsonBody(body);

            // Act
            var response = this.client.Execute(request, Method.Post);
            


            // Assert


            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            var allContacts = this.client.Execute(request, Method.Get);
            var contacts = JsonSerializer.Deserialize<List<Contacts>>(allContacts.Content);

            //var lastContact = contacts[contacts.Count -1];
            var lastContact = contacts.Last();

            Assert.That(lastContact.firstName, Is.EqualTo(body.firstName));
            Assert.That(lastContact.lastName, Is.EqualTo(body.lastName));
        }
    }
}