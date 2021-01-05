using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using KeePassShtokal.AppCore.DTOs;
using KeePassShtokal.Infrastructure;
using KeePassShtokal.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace KeePassShtokal.Validators
{
    public class RegistrationValidator : AbstractValidator<RegisterDto>
    {
        private readonly MainDbContext _mainDbContext;
        public RegistrationValidator(MainDbContext mainDbContext)
        {
            _mainDbContext = mainDbContext;

            RuleFor(x => x.Username)
                .NotNull().WithMessage("Username is Empty")
                .NotEmpty().WithMessage("Username is Empty")
                //.MustAsync((x, cancellation) => !UsernameExist(x, cancellation)).WithMessage("");
                .MustAsync(UsernameNotExist).WithMessage("Username already exist");

            RuleFor(x => x.Password)
                .NotNull().WithMessage("Password is Empty")
                .NotEmpty().WithMessage("Password is Empty");

            RuleFor(x => x.IsPasswordKeptAsHash)
                .NotNull().WithMessage("IsPasswordKeptAsHash is required");
        }

        private async Task<bool> UsernameNotExist(string username, CancellationToken cancellation)
        {
           return await _mainDbContext.Users.AllAsync(x => x.Username != username, cancellation);
        }

    }
}
