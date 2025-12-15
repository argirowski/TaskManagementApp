using Application.DTOs;
using MediatR;

namespace Application.Features.Commands.Projects.UpdateProject
{
    public class UpdateProjectCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public CreateProjectDTO Project { get; set; }
        public Guid UserId { get; set; }

        public UpdateProjectCommand(Guid id, CreateProjectDTO project, Guid userId)
        {
            Id = id;
            Project = project;
            UserId = userId;
        }
    }
}
