using FluentValidation;
using SharingGateway.Models.Filters;

namespace SharingGateway.Validators
{
    public class POIFilterValidator : AbstractValidator<POIFilter>
    {
        public POIFilterValidator()
        {
            RuleFor(poi => poi.Latitude).NotEmpty();
            RuleFor(poi => poi.Longitude).NotEmpty();
            RuleFor(poi => poi.Radius).NotEmpty();
        }
    }
}