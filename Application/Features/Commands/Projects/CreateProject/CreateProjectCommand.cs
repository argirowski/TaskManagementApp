using Application.DTOs;
using MediatR;

namespace Application.Features.Commands.Projects.CreateProject
{
    public class CreateProjectCommand
    : IRequest<bool>
    {
        public CreateProjectDTO Project { get; set; } = null!;
        public Guid UserId { get; set; }

        public CreateProjectCommand(CreateProjectDTO project, Guid userId)
        {
            Project = project;
            UserId = userId;
        }
    }
}
