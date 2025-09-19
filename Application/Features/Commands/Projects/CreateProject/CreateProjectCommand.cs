using Application.DTOs;
using MediatR;

namespace Application.Features.Commands.Projects.CreateProject
{
    public class CreateProjectCommand
    : IRequest<bool>
    {
        public ProjectDTO Project { get; set; } = null!;
        public Guid UserId { get; set; }

        public CreateProjectCommand(ProjectDTO project, Guid userId)
        {
            Project = project;
            UserId = userId;
        }
    }
}
