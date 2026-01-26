using Aroundu.Events.Service.Application.Commands;
using Aroundu.Events.Service.Application.Repositories;
using Aroundu.Events.Service.Domain.Entity;
using Aroundu.SharedKernel.IntegrationEvents;
using Aroundu.SharedKernel.Interfaces;
using NSubstitute;
using Shouldly;

namespace Aroundu.Events.Service.UnitTests
{
    public class CreateEventCommandHandlerTests
    {
        private readonly IEventRepository repoMock;
        private readonly IServiceEventBus busMock;
        private readonly CreateEventCommandHandler handler;

        public CreateEventCommandHandlerTests()
        {
            repoMock = Substitute.For<IEventRepository>();
            busMock = Substitute.For<IServiceEventBus>();

            handler = new CreateEventCommandHandler(repoMock, busMock);
        }

        [Fact]
        public async Task Handle_Should_Create_Event_And_Publish_Notification()
        {
            // Arrange
            var command = new CreateEventCommand { Name = "Arvato Hackathon" };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            await repoMock.Received(1).AddAsync(Arg.Is<Event>(e => e.Name == "Arvato Hackathon"));

            await busMock.Received(1).PublishAsync(
                Arg.Is<EventCreatedEvent>(e => e.Name == "Arvato Hackathon"),
                Arg.Any<CancellationToken>()
            );

            await repoMock.Received(1).SaveAsync(Arg.Any<CancellationToken>());

            result.ShouldNotBe(Guid.Empty);
        }
    }
}
