using Application.DTOs;

namespace Application.Features.Commands.Projects.UpdateProject
{
    public class UpdateProjectCommand
    : MediatR.IRequest<bool>
    {
        public Guid Id { get; set; }
        public ProjectDTO Project { get; set; } = null!;

        public UpdateProjectCommand(Guid id, ProjectDTO project)
        {
            Id = id;
            Project = project;
        }
    }
}
