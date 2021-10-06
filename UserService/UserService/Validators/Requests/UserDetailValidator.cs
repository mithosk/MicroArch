using FluentValidation;
using UserService.BusNamespaces.Flowing.User.Requests;

namespace UserService.Validators.Requests
{
    public class UserDetailValidator : AbstractValidator<UserDetail>
    {
        public UserDetailValidator()
        {
            RuleFor(uds => uds.Id).NotEmpty();
        }
    }
}