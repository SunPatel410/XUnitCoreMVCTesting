using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CreditCards.IntergrationTests
{
    [Trait("Category", "Web API Integration Tests")]
    public class ValuesApiShould : IClassFixture<TestServerFixture>
    {
        private readonly TestServerFixture _fixture;

        public ValuesApiShould(TestServerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetValidValue()
        {
            //Arrange
            var response = await _fixture.Client.GetAsync("/api/values/1");
            response.EnsureSuccessStatusCode();

            //Act
            var responseString = await response.Content.ReadAsStringAsync();

            //Assert
            Assert.Equal("Value1", responseString);
        }

        [Fact]
        public async Task ErrorOnInValidValue()
        {
            //Arrange
            var response = await _fixture.Client.GetAsync("/api/values/0");

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task StartJob()
        {
            var response = await _fixture.Client.PostAsync("/api/values/startjob", null);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Equal("Batch Job Started", responseString);
        }
    }
}
