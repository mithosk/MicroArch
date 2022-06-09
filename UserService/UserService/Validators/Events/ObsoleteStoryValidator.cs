using FluentValidation;
using UserService.BusNamespaces.Flowing.Story.Events;

namespace UserService.Validators.Events
{
    public class ObsoleteStoryValidator : AbstractValidator<ObsoleteStory>
    {
        public ObsoleteStoryValidator()
        {
            RuleFor(osb => osb.StoryId).NotEmpty();
        }
    }
}