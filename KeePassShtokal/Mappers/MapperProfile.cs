using AutoMapper;
using KeePassShtokal.AppCore.DTOs.Entry;
using KeePassShtokal.Infrastructure.Entities;

namespace KeePassShtokal.Mappers
{
    // not usable yet
    class MapperProfile:Profile
    {
        public MapperProfile()
        {
            CreateMap<UsersEntries, GetEntryDto>()
                .ForMember(dest => dest.IsOwner, opt => opt.MapFrom(src => src.IsUserOwner))
                .ForMember(dest => dest.PasswordEncrypted, opt => opt.MapFrom(src => src.Entry.PasswordE))
                .ForAllOtherMembers(d=>d.MapFrom(x=>x.Entry));
        }
    }
}
