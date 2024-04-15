using Contract;
using Domain.ProjectSettingAggregation;
using Domainify.Domain;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace AcceptanceTest.ProjectSettingFeature
{
    /// <summary>
    /// As a user
    /// I want to define a sprint to a project
    /// So that I should be able to access the sprint
    /// </summary>
    public class UserWantsToDefineASprint : IClassFixture<ProjectSettingFixture>
    {
        private IServiceScope _serviceScope;
        private readonly ProjectSettingFixture _fixture;
        public UserWantsToDefineASprint(ProjectSettingFixture fixture)
        {
            _fixture = fixture;
            _serviceScope = _fixture.ServiceProvider.CreateAsyncScope();
        }

        [Fact]
        internal async Task GivenUserDefinesASprint_WhenDefiningSprint_ThenShouldAccessIt()
        {
            IProjectSettingService service = _serviceScope.ServiceProvider.GetRequiredService<IProjectSettingService>();

            var projectId = await DataFacilitator.DefineProject(
                _serviceScope, name: "Task Management");

            var sprintName = "Sprint 01";


            // Given
            var request = new DefineSprint(projectId, sprintName);

            // When
            Func<Task>? actualForGetting = null;
            Func<Task> actual = async () =>
            {
                var sprintId = await service.Process(request);
                var requestToGet = new GetSprint(sprintId);
                actualForGetting = async () => await service.Process(requestToGet);
            };

            // Then
            await actual.Should().NotThrowAsync();
            await actualForGetting?.Should().NotThrowAsync()!;

            // Tear down
            _fixture.EnsureRecreatedDatabase();
        }

        [Fact]
        internal async Task GivenUserDefinesASprint_AndGivenASprintWithThisNameHasAlreadyExistedForThisProject_WhenDefiningSprint_ThenShouldBePreventedFromDefiningItAgain()
        {
            IProjectSettingService service = _serviceScope.ServiceProvider.GetRequiredService<IProjectSettingService>();

            var projectId = await DataFacilitator.DefineProject(
                _serviceScope, name: "Task Management");

            var sprintName = "Sprint 01";

            // Given
            var request = new DefineSprint(projectId, sprintName);
            await service.Process(new DefineSprint(projectId, sprintName));

            // When
            Func<Task> actual = async () => await service.Process(request);

            // Then
            await actual.Should().BeSatisfiedWith<AnEntityWithThesePropertiesHasAlreadyExistedFault>();

            // Tear Down
            _fixture.EnsureRecreatedDatabase();
        }
    }
}
