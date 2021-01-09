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
    public class RegistrationDtoValidator : BaseAuthDtoValidator<RegistrationDto>
    {
        public RegistrationDtoValidator()
        {
            RuleFor(x => x.IsPasswordKeptAsHash)
                .NotNull().WithMessage("IsPasswordKeptAsHash is required");
        }
    }
}
