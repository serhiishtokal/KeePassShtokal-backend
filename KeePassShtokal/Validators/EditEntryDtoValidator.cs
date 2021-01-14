using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using KeePassShtokal.AppCore.DTOs;

namespace KeePassShtokal.Validators
{
    public class EditEntryDtoValidator: BaseEntryDtoValidator<EditEntryDto>
    {
        
            public EditEntryDtoValidator()
            {
                RuleFor(x => x.EntryId)
                    .GreaterThan(0);
            }
    }
}
