using FluentValidation;
using StoryService.BusNamespaces.Flowing.Story.Requests;

namespace StoryService.Validators.Requests
{
    public class SearchPOIValidator : AbstractValidator<SearchPOI>
    {
        public SearchPOIValidator()
        {
            RuleFor(spo => spo.Latitude).NotEmpty();
            RuleFor(spo => spo.Longitude).NotEmpty();
            RuleFor(spo => spo.Radius).NotEmpty();
        }
    }
}