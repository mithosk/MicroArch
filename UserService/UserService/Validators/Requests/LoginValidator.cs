using FluentValidation;
using UserService.BusNamespaces.Flowing.User.Requests;

namespace UserService.Validators.Requests
{
    public class LoginValidator : AbstractValidator<Login>
    {
        public LoginValidator()
        {
            RuleFor(log => log.Email).EmailAddress().NotEmpty();
            RuleFor(log => log.Password).NotEmpty();
        }
    }
}