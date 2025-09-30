using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Commands.Projects.UpdateProject
{
    public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, bool>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;

        public UpdateProjectCommandHandler(IProjectRepository projectRepository, IMapper mapper)
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
        }

        public async Task<bool> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetProjectByIdAsync(request.Id);
            if (project == null)
                return false;

            // Map updated fields
            _mapper.Map(request.Project, project);
            var updated = await _projectRepository.UpdateProjectAsync(project);
            return updated;
        }
    }
}
