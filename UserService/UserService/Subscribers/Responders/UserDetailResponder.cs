using AgileServiceBus.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.BusNamespaces.Flowing.User.Requests;
using UserService.Data.FilterBy;
using UserService.Data.Interfaces;
using UserService.Data.Models;
using UserService.Data.Repositories;
using FlowingUserModels = UserService.BusNamespaces.Flowing.User.Models;

namespace UserService.Subscribers.Responders
{
    public class UserDetailResponder : IResponder<UserDetail>
    {
        private readonly IDataContext _dataContext;

        public IMicroserviceBus Bus { get; set; }
        public ITraceScope TraceScope { get; set; }

        public UserDetailResponder(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<object> RespondAsync(UserDetail message)
        {
            //find user
            User user;
            using (TraceScope.CreateSubScope("FindUser"))
            {
                user = await _dataContext.Users
                    .AsNoTracking()
                    .FindByAsync(message.Id);
            }

            //user not found
            if (user == null)
                return null;

            //count user stories
            uint userStoriesCount;
            using (TraceScope.CreateSubScope("CountUserStories"))
            {
                userStoriesCount = await _dataContext.Stories
                    .CountByAsync(new StoryFilterBy
                    {
                        UserExternalId = user.ExternalId
                    });
            }

            //find last user story
            Story lastUserStory;
            using (TraceScope.CreateSubScope("FindLastUserStory"))
            {
                List<Story> lastUserStories = await _dataContext.Stories
                    .AsNoTracking()
                    .FindByAsync(new StoryFilterBy
                    {
                        UserExternalId = user.ExternalId
                    }, 1);

                lastUserStory = lastUserStories.SingleOrDefault();
            }

            //response mapping
            return new FlowingUserModels.User
            {
                Id = user.ExternalId,
                Email = user.Email,
                Name = user.Name,
                Surname = user.Surname,
                RegistrationDate = user.RegistrationDate,
                PublishedStories = (ushort)userStoriesCount,
                LastPublishDate = lastUserStory?.PublicationDate
            };
        }
    }
}