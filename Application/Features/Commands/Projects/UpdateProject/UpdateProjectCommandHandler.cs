using Application.Exceptions;
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
            // Check if userId is empty
            if (request.UserId == Guid.Empty)
            {
                throw new UnauthorizedException("No user ID provided. User must be authenticated.");
            }

            var project = await _projectRepository.GetProjectByIdAsync(request.Id);
            if (project == null)
            {
                throw new NotFoundException($"Project with ID {request.Id} not found.");
            }

            // Authorization check
            var isOwner = await _authorizationService.IsUserOwnerAsync(request.Id, request.UserId);
            if (!isOwner)
            {
                throw new ForbiddenException("You do not have permission to update this project.");
            }

            // Map updated fields
            _mapper.Map(request.Project, project);
            var updated = await _projectRepository.UpdateProjectAsync(project);
            if (!updated)
            {
                throw new BadRequestException("Failed to update the project. Please try again.");
            }
            return true;
        }
    }
}
