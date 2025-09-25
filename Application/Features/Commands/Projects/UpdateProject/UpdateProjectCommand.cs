using Application.DTOs;

namespace Application.Features.Commands.Projects.UpdateProject
{
    public class UpdateProjectCommand
    : MediatR.IRequest<bool>
    {
        public Guid Id { get; set; }
        public CreateProjectDTO Project { get; set; }

        public UpdateProjectCommand(Guid id, CreateProjectDTO project)
        {
            Id = id;
            Project = project;
        }
    }
}
