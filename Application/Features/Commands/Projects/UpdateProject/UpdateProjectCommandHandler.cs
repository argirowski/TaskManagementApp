using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Commands.Projects.UpdateProject
{
    public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, Unit>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;

        public UpdateProjectCommandHandler(IProjectRepository projectRepository, IMapper mapper)
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetByIdAsync(request.Id);
            if (project == null)
                throw new InvalidOperationException("Project not found");

            // Map updated fields
            _mapper.Map(request.Project, project);
            await _projectRepository.UpdateAsync(project);
            return Unit.Value;
        }
    }
}
