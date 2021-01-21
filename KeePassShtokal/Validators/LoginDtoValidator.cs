using FluentValidation;
using KeePassShtokal.AppCore.DTOs.Auth;

namespace KeePassShtokal.Validators
{
    public class LoginDtoValidator: BaseAuthDtoValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.IpAddress)
                .NotNull().NotEmpty();
        }
    }
}
