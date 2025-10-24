using Application.DTOs;
using Application.Features.Queries.Projects.GetAllProjects;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace TaskManagementAppUnitTests.HandlerTests
{
    public class GetAllProjectsQueryHandlerTests
    {
        private readonly Mock<IProjectRepository> _projectRepoMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly GetAllProjectsQueryHandler _handler;

        public GetAllProjectsQueryHandlerTests()
        {
            _handler = new GetAllProjectsQueryHandler(_projectRepoMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_NoProjects_ReturnsEmptyList()
        {
            // Arrange
            _projectRepoMock.Setup(x => x.GetAllProjectsAsync()).ReturnsAsync(new List<Project>());
            _mapperMock.Setup(x => x.Map<List<ProjectDTO>>(It.IsAny<List<Project>>())).Returns(new List<ProjectDTO>());

            // Act
            var result = await _handler.Handle(new GetAllProjectsQuery(), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_ProjectsExist_ReturnsMappedList()
        {
            // Arrange
            var projects = new List<Project> { new Project { Id = Guid.NewGuid(), ProjectName = "P1" } };
            var dtos = new List<ProjectDTO> { new ProjectDTO { Id = projects[0].Id, ProjectName = "P1", ProjectDescription = "Desc" } };
            _projectRepoMock.Setup(x => x.GetAllProjectsAsync()).ReturnsAsync(projects);
            _mapperMock.Setup(x => x.Map<List<ProjectDTO>>(projects)).Returns(dtos);

            // Act
            var result = await _handler.Handle(new GetAllProjectsQuery(), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result[0].ProjectName.Should().Be("P1");
        }
    }
}
