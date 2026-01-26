using Aroundu.Events.Service.Application.Commands;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Reqnroll;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace Aroundu.Events.Service.Specs.Steps
{
    [Binding]
    public class CreateEventSteps : IClassFixture<WebApplicationFactory<Aroundu.Events.Service.Api.Program>>
    {
        private readonly HttpClient client;
        private HttpResponseMessage response;
        private CreateEventCommand command;

        public CreateEventSteps(WebApplicationFactory<Aroundu.Events.Service.Api.Program> factory)
        {
            client = factory.CreateClient();
        }

        [Given("I am an authenticated user")]
        public void GivenIAmAnAuthenticatedUser()
        {
        }

        [When("I create an event with name {string}")]
        public async Task WhenICreateAnEventWithName(string eventName)
        {
            command = new CreateEventCommand { Name = eventName };

            response = await client.PostAsJsonAsync("/api/events", command);
        }

        [Then("The event creation should be successful")]
        public void ThenTheEventCreationShouldBeSuccessful()
        {
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Then("The event PublicKey should be returned")]
        public async Task ThenTheEventIdShouldBeReturned()
        {
            var content = await response.Content.ReadAsStringAsync();
            content.Should().NotBeNullOrEmpty();
        }
    }
}
