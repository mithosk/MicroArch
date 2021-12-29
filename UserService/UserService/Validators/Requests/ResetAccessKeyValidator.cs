using FluentValidation;
using UserService.BusNamespaces.Flowing.User.Requests;

namespace UserService.Validators.Requests
{
    public class ResetAccessKeyValidator : AbstractValidator<ResetAccessKey>
    {
        public ResetAccessKeyValidator()
        {
            RuleFor(rak => rak.UserId).NotEmpty();
        }
    }
}