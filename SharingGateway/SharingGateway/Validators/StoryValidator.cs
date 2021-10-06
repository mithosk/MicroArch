using FluentValidation;
using SharingGateway.Models;

namespace SharingGateway.Validators
{
    public class StoryValidator : AbstractValidator<Story>
    {
        public StoryValidator()
        {
            RuleFor(sto => sto.Id).Null();
            RuleFor(sto => sto.Type).NotNull();
            RuleFor(sto => sto.Title).NotEmpty();
            RuleFor(sto => sto.Tale).NotEmpty();
            RuleFor(sto => sto.Latitude).NotEmpty();
            RuleFor(sto => sto.Longitude).NotEmpty();
            RuleFor(sto => sto.UserId).NotEmpty();
        }
    }
}