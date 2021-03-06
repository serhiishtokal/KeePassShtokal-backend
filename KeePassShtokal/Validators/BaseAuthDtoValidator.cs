﻿using FluentValidation;
using KeePassShtokal.AppCore.DTOs.Auth;

namespace KeePassShtokal.Validators
{
    public class BaseAuthDtoValidator<T>: AbstractValidator<T> where T: BaseAuthDto
    {

        public BaseAuthDtoValidator()
        {
            RuleFor(x => x.Username)
                .NotNull().WithMessage("Username is Empty")
                .NotEmpty().WithMessage("Username is Empty");

            RuleFor(x => x.Password)
                .NotNull().WithMessage("Password is Empty")
                .NotEmpty().WithMessage("Password is Empty");
        }
    }
}
