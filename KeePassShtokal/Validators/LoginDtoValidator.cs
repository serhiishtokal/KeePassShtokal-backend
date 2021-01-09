using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using KeePassShtokal.AppCore.DTOs;

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
