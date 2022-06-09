using AgileServiceBus.Interfaces;
using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Data;
using UserService.Data.Interfaces;
using UserService.Data.Models;
using UserService.Exceptions;
using UserService.Subscribers.EventHandlers;
using Xunit;
using FlowingStoryEvents = UserService.BusNamespaces.Flowing.Story.Events;

namespace UserService.Test.Subscribers.EventHandlers
{
    public class PublishedStoryEventHandlerTest : TestBase
    {
        [Theory]
        [MemberData(nameof(LoadMessages))]
        public async Task SuccessfullyStoryRegistration(FlowingStoryEvents.PublishedStory message)
        {
            //data context fake
            using (IDataContext dataContext = new DataContext(_dataOptions))
            {
                await dataContext.Users.AddAsync(new User
                {
                    ExternalId = message.UserId,
                    Email = "email",
                    PasswordHash = "password",
                    Name = "name",
                    Surname = "surname"
                });

                await dataContext.SaveChangesAsync();
            }

            //execution
            using (IDataContext dataContext = new DataContext(_dataOptions))
            {
                PublishedStoryEventHandler hanlder = new(dataContext);
                hanlder.TraceScope = A.Fake<ITraceScope>();
                await hanlder.HandleAsync(message);
            }

            //check
            using (IDataContext dataContext = new DataContext(_dataOptions))
            {
                User user = await dataContext.Users
                    .Where(use => use.ExternalId == message.UserId)
                    .SingleOrDefaultAsync();

                Assert.NotEmpty(user.Stories);
                Assert.True(user.Stories.Count == 1);
                Assert.Equal(message.StoryId, user.Stories.Single().ExternalId);
                Assert.Equal(message.PublicationDate, user.Stories.Single().PublicationDate);
            }
        }

        [Theory]
        [MemberData(nameof(LoadMessages))]
        public async Task UnsuccessfullyStoryRegistration(FlowingStoryEvents.PublishedStory message)
        {
            using IDataContext dataContext = new DataContext(_dataOptions);
            PublishedStoryEventHandler hanlder = new(dataContext);
            hanlder.TraceScope = A.Fake<ITraceScope>();
            await Assert.ThrowsAsync<ObjectNotFoundException>(() => hanlder.HandleAsync(message));
        }

        public static List<object[]> LoadMessages()
        {
            return LoadJson<FlowingStoryEvents.PublishedStory>("Events/PublishedStory.json");
        }
    }
}