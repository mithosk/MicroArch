using FluentValidation;
using StoryService.BusNamespaces.Flowing.Story.Requests;

namespace StoryService.Validators.Requests
{
    public class StoryDetailValidator : AbstractValidator<StoryDetail>
    {
        public StoryDetailValidator()
        {
            RuleFor(sdt => sdt.Id).NotEmpty();
        }
    }
}