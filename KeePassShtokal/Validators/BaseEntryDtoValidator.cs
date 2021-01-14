using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using KeePassShtokal.AppCore.DTOs;

namespace KeePassShtokal.Validators
{
    public class BaseEntryDtoValidator<T>: AbstractValidator<T> where T : BaseEntryDto
    {
        
            public BaseEntryDtoValidator()
            {
                RuleFor(x => x.Username)
                    .NotNull().WithMessage("Username is Empty")
                    .NotEmpty().WithMessage("Username is Empty");

                RuleFor(x => x.PasswordDecrypted)
                    .NotNull().WithMessage("Password is Empty")
                    .NotEmpty().WithMessage("Password is Empty");
        }
        
    }
}
