namespace Application.Features.Commands.Projects.DeleteProject
{
    using Domain.Interfaces;
    using MediatR;

    public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, bool>
    {
        private readonly IProjectRepository _projectRepository;

        public DeleteProjectCommandHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<bool> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            return await _projectRepository.DeleteAsync(request.Id);
        }
    }
}
