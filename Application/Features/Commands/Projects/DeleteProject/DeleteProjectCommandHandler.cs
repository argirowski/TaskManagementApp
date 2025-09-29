using Domain.Interfaces;
using MediatR;

namespace Application.Features.Commands.Projects.DeleteProject
{
    public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, bool>
    {
        private readonly IProjectRepository _projectRepository;

        public DeleteProjectCommandHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<bool> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            // Check if project exists
            var project = await _projectRepository.GetByIdAsync(request.Id);
            if (project == null)
            {
                return false;
            }
            // Delete the project
            var deleted = await _projectRepository.DeleteAsync(request.Id);
            return deleted;
        }
    }
}
