using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System;
using Xunit;
using FluentAssertions;
using Contract;
using Domain.ProjectSettingAggregation;
using Domainify.Domain;

namespace AcceptanceTest.ProjectSettingFeature
{
    /// <summary>
    /// As a user
    /// I want to define a project
    /// So that I should be able to access the project
    /// </summary>
    public class UserWantsToDefineAProject : IClassFixture<ProjectSettingFixture>
    {
        private IServiceScope _serviceScope;
        private readonly ProjectSettingFixture _fixture;
        public UserWantsToDefineAProject(ProjectSettingFixture fixture)
        {
            _fixture = fixture;
            _serviceScope = _fixture.ServiceProvider.CreateAsyncScope();
        }

        [Fact]
        internal async Task GivenUserDefinesAProject_WhenDefiningProject_ThenShouldAccessIt()
        {
            IProjectSettingService service = _serviceScope!.ServiceProvider.GetRequiredService<IProjectSettingService>();

            var projectName = "Task Management";

            // Given
            var request = new DefineProject(projectName);

            // When
            Func<Task>? actualForGetting = null;
            Func <Task> actual = async () => {
                var projectId = await service.Process(request);
                var requestToGet = new GetProject(projectId);
                actualForGetting = async () => await service.Process(requestToGet);
            };
 
            // Then
            await actual.Should().NotThrowAsync();
            await actualForGetting?.Should().NotThrowAsync()!;

            // Tear down
            _fixture.EnsureRecreatedDatabase();
        }
        [Fact]
        internal async Task GivenUserDefinesAProject_AndGivenAProjectWithThisNameHasAlreadyExisted_WhenDefiningProject_ThenShouldBePreventedFromDefiningItAgain()
        {
            IProjectSettingService service = _serviceScope!.ServiceProvider.GetRequiredService<IProjectSettingService>();

            var projectName = "Task Management";

            // Given
            var request = new DefineProject(projectName);
            await service.Process(new DefineProject(projectName));

            // When
            Func<Task> actual = async () => await service.Process(request);

            // Then
            await actual.Should().BeSatisfiedWith<AnEntityWithThesePropertiesHasAlreadyExistedFault>();

            // Tear down
            _fixture.EnsureRecreatedDatabase();
        }
    }
}
