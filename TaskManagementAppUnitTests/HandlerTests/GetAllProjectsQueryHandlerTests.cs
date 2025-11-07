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
        public async Task Handle_NoProjects_ReturnsEmptyPagedResult()
        {
            // Arrange
            _projectRepoMock
                .Setup(x => x.GetAllProjectsAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync((new List<Project>(), 0));
            _mapperMock
                .Setup(x => x.Map<List<ProjectDTO>>(It.IsAny<List<Project>>()))
                .Returns(new List<ProjectDTO>());

            var query = new GetAllProjectsQuery { Page = 1, PageSize = 10 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().BeEmpty();
            result.TotalCount.Should().Be(0);
            result.Page.Should().Be(1);
            result.PageSize.Should().Be(10);
        }

        [Fact]
        public async Task Handle_ProjectsExist_ReturnsPagedResult()
        {
            // Arrange
            var projects = new List<Project> { new Project { Id = Guid.NewGuid(), ProjectName = "P1" } };
            var dtos = new List<ProjectDTO> { new ProjectDTO { Id = projects[0].Id, ProjectName = "P1", ProjectDescription = "Desc" } };
            _projectRepoMock
                .Setup(x => x.GetAllProjectsAsync(1, 10))
                .ReturnsAsync((projects, projects.Count));
            _mapperMock
                .Setup(x => x.Map<List<ProjectDTO>>(projects))
                .Returns(dtos);

            var query = new GetAllProjectsQuery { Page = 1, PageSize = 10 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(1);
            result.Items[0].ProjectName.Should().Be("P1");
            result.TotalCount.Should().Be(1);
            result.Page.Should().Be(1);
            result.PageSize.Should().Be(10);
        }
    }
}
