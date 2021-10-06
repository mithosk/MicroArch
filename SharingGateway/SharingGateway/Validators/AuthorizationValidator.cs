using FluentValidation;
using SharingGateway.Models;

namespace SharingGateway.Validators
{
    public class AuthorizationValidator : AbstractValidator<Authorization>
    {
        public AuthorizationValidator()
        {
            RuleFor(aut => aut.Email).NotEmpty().EmailAddress();
            RuleFor(aut => aut.Password).NotEmpty();
            RuleFor(aut => aut.Token).Null();
            RuleFor(aut => aut.UserId).Null();
        }
    }
}