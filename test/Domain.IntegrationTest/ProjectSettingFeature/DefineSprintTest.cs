using Contract;
using Domain.ProjectSetting;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;


namespace Application.IntegrationTest.ProjectSettingFeature
{
    public class DefineSprintTest : IClassFixture<ProjectSettingFixture>
    {
        private IServiceScope _serviceScope;
        private readonly ProjectSettingFixture _fixture;
        public DefineSprintTest(ProjectSettingFixture fixture)
        {
            _fixture = fixture;
            _serviceScope = _fixture.ServiceProvider.CreateAsyncScope();
        }

        [Fact]
        internal async System.Threading.Tasks.Task Should_Be_Done_When_Is_Valid()
        {
            IProjectSettingService service = _serviceScope.ServiceProvider.GetRequiredService<IProjectSettingService>();
            // Arrange;
            var projectId = await service.Process(new DefineProject(name: "Task Management"));
            var request = new DefineSprint(projectId, "Sprint 01");

            // Act
            Func<System.Threading.Tasks.Task> actual = async () => await service.Process(request);

            // Assert
            await actual.Should().NotThrowAsync();
        }
    }
}
