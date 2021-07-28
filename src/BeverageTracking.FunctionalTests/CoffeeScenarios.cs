using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace BeverageTracking.FunctionalTests
{
    public class CoffeeScenarios : ServerTestBase
    {
        [Fact]
        public async Task Brew_Coffee_Return_Service_Unavailable_After_Every_Fifth_Call()
        {
            using var server = CreateServer();
            var client = server.CreateClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            var apiUrl = "/brew-coffee";

            // 1st call
            var response = await client.GetAsync(apiUrl);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // 2nd call
            response = await client.GetAsync(apiUrl);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // 3th call
            response = await client.GetAsync(apiUrl);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // 4th call
            response = await client.GetAsync(apiUrl);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // 5th call
            response = await client.GetAsync(apiUrl);
            Assert.Equal(HttpStatusCode.ServiceUnavailable, response.StatusCode);

            // 6th call should return ok
            response = await client.GetAsync(apiUrl);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Brew_Coffee_Return_Teapot_On_April_Fool_Day()
        {
            using var server = CreateServer(new DateTime(2021, 4, 1));
            var client = server.CreateClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            var apiUrl = "/brew-coffee";
            
            var response = await client.GetAsync(apiUrl);            
            Assert.Equal(StatusCodes.Status418ImATeapot, (int)response.StatusCode);
        }
    }
}
