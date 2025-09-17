using AutoMapper;
using WebApi8_AdmissionTest.Models;
using WebApi8_AdmissionTest.Dto.Student;

namespace WebApi8_AdmissionTest.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<StudentEditDto, StudentModel>()
            .ForMember(dest => dest.Ra, opt => opt.Ignore())
            .ReverseMap();

            CreateMap<StudentCreateDto, StudentModel>()
                .ForMember(dest => dest.Ra, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
