using Application.DTOs;
using Application.Features.Queries.Projects.GetSingleProject;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace TaskManagementAppUnitTests.HandlerTests
{
    public class GetSingleProjectQueryHandlerTests
    {
        private readonly Mock<IProjectRepository> _projectRepoMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly GetSingleProjectQueryHandler _handler;

        public GetSingleProjectQueryHandlerTests()
        {
            _handler = new GetSingleProjectQueryHandler(_projectRepoMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ProjectDoesNotExist_ReturnsNull()
        {
            _projectRepoMock.Setup(x => x.GetProjectByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Project?)null);
            var query = new GetSingleProjectQuery(Guid.NewGuid());
            await FluentActions.Invoking(() => _handler.Handle(query, CancellationToken.None))
                .Should().ThrowAsync<Application.Exceptions.NotFoundException>()
                .WithMessage("Project with ID * not found.");
        }

        [Fact]
        public async Task Handle_ProjectExists_ReturnsMappedDto()
        {
            var project = new Project { Id = Guid.NewGuid(), ProjectName = "Test Project", ProjectDescription = "Desc" };
            var dto = new ProjectDetailsDTO { ProjectName = "Test Project", ProjectDescription = "Desc", Users = new(), Tasks = new() };
            _projectRepoMock.Setup(x => x.GetProjectByIdAsync(project.Id)).ReturnsAsync(project);
            _mapperMock.Setup(x => x.Map<ProjectDetailsDTO>(project)).Returns(dto);
            var query = new GetSingleProjectQuery(project.Id);
            var result = await _handler.Handle(query, CancellationToken.None);
            result.Should().NotBeNull();
            result.ProjectName.Should().Be("Test Project");
            result.ProjectDescription.Should().Be("Desc");
        }
    }
}
