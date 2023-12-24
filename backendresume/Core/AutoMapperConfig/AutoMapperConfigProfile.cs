using AutoMapper;
using backendresume.Core.Dtos.Candidate;
using backendresume.Core.Dtos.Company;
using backendresume.Core.Dtos.Job;
using backendresume.Core.Entities;

namespace backendresume.Core.AutoMapperConfig
{
    public class AutoMapperConfigProfile : Profile
    {
        public AutoMapperConfigProfile()
        {
            // Company
            CreateMap<ComapanyCreateDto, Company>();
            CreateMap<Company, CompanyGetDto>();

            // Job
            CreateMap<JobCreateDto, Job>();
            CreateMap<Job, JobGetDto>()
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company.Name));

            // Candidate
            CreateMap<CandidateCreateDto, Candidate>();
            CreateMap<Candidate, CandidateGetDto>()
                .ForMember(dest => dest.JobTitle, opt => opt.MapFrom(src => src.Job.Title));

        }
    }
}
