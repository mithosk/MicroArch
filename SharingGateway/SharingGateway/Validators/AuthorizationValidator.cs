using FluentValidation;
using SharingGateway.Models;

namespace SharingGateway.Validators
{
    public class AuthorizationValidator : AbstractValidator<Authorization>
    {
        public AuthorizationValidator()
        {
            //login authentication
            RuleFor(aut => aut.Email).NotEmpty().EmailAddress()
                .When(aut => !aut.RefreshToken.HasValue);

            RuleFor(aut => aut.Password).NotEmpty()
                .When(aut => !aut.RefreshToken.HasValue);

            RuleFor(aut => aut.UserId).Null()
               .When(aut => !aut.RefreshToken.HasValue);

            //refresh authentication
            RuleFor(aut => aut.Email).Null()
                .When(aut => aut.RefreshToken.HasValue);

            RuleFor(aut => aut.Password).Null()
                .When(aut => aut.RefreshToken.HasValue);

            RuleFor(aut => aut.UserId).NotEmpty()
               .When(aut => aut.RefreshToken.HasValue);

            //both
            RuleFor(aut => aut.Token).Null();
        }
    }
}