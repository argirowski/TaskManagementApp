using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Commands.Projects.UpdateProject
{
    public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, bool>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectAuthorizationService _authorizationService;
        private readonly IMapper _mapper;

        public UpdateProjectCommandHandler(IProjectRepository projectRepository, IProjectAuthorizationService authorizationService, IMapper mapper)
        {
            _projectRepository = projectRepository;
            _authorizationService = authorizationService;
            _mapper = mapper;
        }

        public async Task<bool> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetProjectByIdAsync(request.Id);
            if (project == null)
                return false;

            // Authorization check
            var isOwner = await _authorizationService.IsUserOwnerAsync(request.Id, request.UserId);
            if (!isOwner)
                return false;

            // Map updated fields
            _mapper.Map(request.Project, project);
            var updated = await _projectRepository.UpdateProjectAsync(project);
            return updated;
        }
    }
}
