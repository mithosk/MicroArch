using FluentValidation;
using UserService.BusNamespaces.Flowing.Story.Events;

namespace UserService.Validators.Events
{
    public class PublishedStoryValidator : AbstractValidator<PublishedStory>
    {
        public PublishedStoryValidator()
        {
            RuleFor(psu => psu.StoryId).NotEmpty();
            RuleFor(psu => psu.PublicationDate).NotEmpty();
            RuleFor(psu => psu.UserId).NotEmpty();
        }
    }
}