using AutoMapper;
using OutlandsTool.MVC.ViewModels;
using OutlandsTool.ServiceModel.Entities;

namespace OutlandsTool.MVC.Mappings
{
    public class LootProfile : Profile
    {
        public LootProfile()
        {
            CreateMap<LootItem, LootViewModel>()
                .ForMember(dest => dest.Actions, opt => opt.MapFrom(src => LootTransforms.ProcessServerActionsAnchor(src.LootItemId)));
        }
    }
}