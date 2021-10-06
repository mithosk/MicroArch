using FluentValidation;
using StoryService.BusNamespaces.Flowing.Story.Requests;

namespace StoryService.Validators.Requests
{
    public class StoryListValidator : AbstractValidator<StoryList>
    {
        public StoryListValidator()
        {
            RuleFor(slt => slt.PageIndex).NotEmpty();
            RuleFor(slt => (int)slt.PageSize).LessThanOrEqualTo(300);
        }
    }
}