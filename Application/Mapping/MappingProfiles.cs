using Application.DTOs;
using Application.Features.Commands.Auth.Register;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<User, UserDTO>();
            CreateMap<User, UserDetailsDTO>();
            CreateMap<User, UserResponseDTO>();
            CreateMap<UserDTO, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.ProjectUsers, opt => opt.Ignore())
                .ForMember(dest => dest.AssignedProjectTasks, opt => opt.Ignore());

            CreateMap<UserDTO, RegisterUserCommand>();

            CreateMap<Project, ProjectDTO>().ReverseMap();
            CreateMap<Project, ProjectDetailsDTO>();
            CreateMap<Project, CreateProjectDTO>().ReverseMap();

            CreateMap<ProjectTask, TaskDTO>();
            CreateMap<TaskDTO, ProjectTask>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ProjectId, opt => opt.Ignore())
                .ForMember(dest => dest.Project, opt => opt.Ignore())
                .ForMember(dest => dest.ProjectTaskStatus, opt => opt.Ignore())
                .ForMember(dest => dest.AssignedUserId, opt => opt.Ignore())
                .ForMember(dest => dest.AssignedUser, opt => opt.Ignore());
            CreateMap<ProjectTask, TaskDetailsDTO>();

            CreateMap<Project, ProjectDetailsDTO>()
                .ForMember(dest => dest.Users, opt => opt.MapFrom(src => src.ProjectUsers.Select(pu => pu.User)))
                .ForMember(dest => dest.Tasks, opt => opt.MapFrom(src => src.ProjectTasks));
        }
    }
}
