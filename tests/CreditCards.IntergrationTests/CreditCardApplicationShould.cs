using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Xunit;

namespace CreditCards.IntergrationTests
{
    public class CreditCardApplicationShould
    {
        [Fact]
        public async Task RenderApplicationForm()
        {
            //Arrange
            //creating test server for intergration tests => HTTP CLIENT
            var builder = new WebHostBuilder()
                .UseContentRoot("C:\\Users\\C09950A\\Documents\\Visual Studio 2017\\Projects\\CreditCards\\src\\CreditCards")
                .UseEnvironment("Development")
                .UseStartup<Startup>()
                .UseApplicationInsights();

            //Act
            //pass into the test server to start making requests
            var server = new TestServer(builder);
            var client = server.CreateClient();

            var reponse = await client.GetAsync("/Apply");
            //checking if the reponse is successful or not.
            reponse.EnsureSuccessStatusCode();
            var responseString = await reponse.Content.ReadAsStringAsync();
            
            //Assert
            Assert.Contains("New Credit Card Application", responseString);
        }
    }
}
