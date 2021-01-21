using FluentValidation;
using KeePassShtokal.AppCore.DTOs.Auth;

namespace KeePassShtokal.Validators
{
    public class RegistrationDtoValidator : BaseAuthDtoValidator<RegistrationDto>
    {
        public RegistrationDtoValidator()
        {
            RuleFor(x => x.IsPasswordKeptAsHash)
                .NotNull().WithMessage("IsPasswordKeptAsHash is required");
        }
    }
}
