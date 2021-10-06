using FluentValidation;
using StoryService.BusNamespaces.Flowing.Story.Requests;

namespace StoryService.Validators.Requests
{
    public class PublishStoryValidator : AbstractValidator<PublishStory>
    {
        public PublishStoryValidator()
        {
            RuleFor(psu => psu.Type).NotNull();
            RuleFor(psu => psu.Title).NotEmpty().MaximumLength(70);
            RuleFor(psu => psu.Tale).NotEmpty();
            RuleFor(psu => psu.Latitude).NotEmpty();
            RuleFor(psu => psu.Longitude).NotEmpty();
            RuleFor(psu => psu.UserId).NotEmpty();
        }
    }
}