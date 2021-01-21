using FluentValidation;
using KeePassShtokal.AppCore.DTOs.Entry;

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
