using MediatR;

namespace Application.Features.Commands.Projects.DeleteProject
{
    public class DeleteProjectCommand
    : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public DeleteProjectCommand(Guid id)
        {
            Id = id;
        }
    }
}
