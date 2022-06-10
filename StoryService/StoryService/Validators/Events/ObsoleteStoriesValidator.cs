using FluentValidation;
using StoryService.BusNamespaces.Flowing.Story.Events;
using System;

namespace StoryService.Validators.Events
{
    public class ObsoleteStoriesValidator : AbstractValidator<ObsoleteStories>
    {
        public ObsoleteStoriesValidator()
        {
            RuleFor(osb => osb.DateTo).NotEmpty().LessThanOrEqualTo(DateTime.UtcNow);
        }
    }
}