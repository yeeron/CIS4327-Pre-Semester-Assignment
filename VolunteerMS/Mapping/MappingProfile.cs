using AutoMapper;
using VolunteerMS.Models.ViewModels.Skill;
using VolunteerMS.Models.ViewModels.Volunteer;
using VolunteerMS.Models;
using VolunteerMS.Models.ViewModels.Center;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Mapping configurations for Volunteer entity and its corresponding ViewModels
        //CreateMap<Source, Destination>();
        CreateMap<Volunteer, VolunteerCreateVM>().ReverseMap();
        
        CreateMap<VolunteerEditVM, Volunteer>().ReverseMap();
        CreateMap<Volunteer, VolunteerEditVM>()
            .ForMember(dest => dest.Username,
                opt => opt.MapFrom(src => src.User!.Username)
            )
            .ForMember(dest => dest.Password,
                opt => opt.MapFrom(src => src.User!.PasswordHash)
            ); 

        CreateMap<Volunteer, VolunteerListVM>()
            .ForMember(
                dest => dest.FullName,
                opt => opt.MapFrom(src =>
                    $"{src.FirstName} {src.LastName}")
            );

        CreateMap<Volunteer, VolunteerDetailsVM>()
            .ForMember(
                dest => dest.Username,
                opt => opt.MapFrom(src => src.User!.Username)
            )
            .ForMember(
                dest => dest.Skills,
                opt => opt.MapFrom(src =>
                    src.VolunteerSkills.Select(vs => vs.Skill!.Name))
            )
            .ForMember(
                dest => dest.Centers,
                opt => opt.MapFrom(src =>
                    src.VolunteerCenters.Select(vc => vc.Center!.Name))
            )
            .ForMember(
                dest => dest.Opportunities,
                opt => opt.MapFrom(src =>
                    src.VolunteerOpportunities.Select(vo => vo.Opportunity!.Name))
            );;

        //Mapping configurations for Center entity and its corresponding ViewModels
        CreateMap<Center, CenterCreateVM>().ReverseMap();
        CreateMap<Center, CenterEditVM>().ReverseMap();

        CreateMap<Center, CenterListVM>()
            .ForMember(dest => dest.VolunteerCount,
                opt => opt.MapFrom(src => src.VolunteerCenters.Count))
            .ForMember(dest => dest.OpportunityCount,
                opt => opt.MapFrom(src => src.Opportunities.Count));

        CreateMap<Center, CenterDetailsVM>()
            .ForMember(dest => dest.VolunteerCount,
                opt => opt.MapFrom(src => src.VolunteerCenters.Count))
            .ForMember(dest => dest.OpportunityCount,
                opt => opt.MapFrom(src => src.Opportunities.Count));

        //Mapping configurations for Skill entity and its corresponding ViewModels
        CreateMap<Skill, SkillListVM>()
            .ForMember(dest => dest.VolunteerCount,
                opt => opt.MapFrom(src => src.VolunteerSkills.Count));

        CreateMap<Skill, SkillEditVM>().ReverseMap();
        CreateMap<Skill, SkillCreateVM>().ReverseMap();
        
    }
}