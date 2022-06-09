using FluentValidation;
using StoryService.BusNamespaces.Flowing.Story.Events;

namespace StoryService.Validators.Events
{
    public class ObsoleteStoryValidator : AbstractValidator<ObsoleteStory>
    {
        public ObsoleteStoryValidator()
        {
            RuleFor(osb => osb.StoryId).NotEmpty();
        }
    }
}