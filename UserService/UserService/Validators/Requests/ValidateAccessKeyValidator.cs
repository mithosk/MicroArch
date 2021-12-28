using FluentValidation;
using UserService.BusNamespaces.Flowing.User.Requests;

namespace UserService.Validators.Requests
{
    public class ValidateAccessKeyValidator : AbstractValidator<ValidateAccessKey>
    {
        public ValidateAccessKeyValidator()
        {
            RuleFor(vak => vak.UserId).NotEmpty();
            RuleFor(vak => vak.AccessKey).NotEmpty();
        }
    }
}