using Contract;
using Domain.ProjectSetting;
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
    /// I want to change the name of a sprint of a project
    /// So that I should be able to access the sprint with the new name
    /// </summary>
    public class UserWantsToChangeTheNameOfASprint : IClassFixture<ProjectSettingFixture>
    {
        private IServiceScope _serviceScope;
        private readonly ProjectSettingFixture _fixture;
        public UserWantsToChangeTheNameOfASprint(ProjectSettingFixture fixture)
        {
            _fixture = fixture;
            _serviceScope = _fixture.ServiceProvider.CreateAsyncScope();
        }
        [Fact]
        internal async Task GivenUserChangesTheNameOfASprint_WhenChangineTheName_ThenShouldBeAbleToAccessItWithTheNewName()
        {
            IProjectSettingService service = _serviceScope.ServiceProvider.GetRequiredService<IProjectSettingService>();

            var projectId = await DataFacilitator.DefineProject(
                _serviceScope, name: "Task Management");

            var sprintId = await DataFacilitator.DefineSprint(
                _serviceScope, projectId, name: "Sprint 01");

            var newSprintName = "Sprint 02";

            // Given
            var request = new ChangeSprintName(sprintId, newSprintName);

            // When
            Func<Task> actual = async () => await service.Process(request);

            var requestToGet = new GetSprint(sprintId);

            // Then
            await actual.Should().NotThrowAsync();

            var retrievedSprint = await service.Process(requestToGet);
            retrievedSprint!.Name.Should().Be(newSprintName);

            // Tear down
            _fixture.EnsureRecreatedDatabase();
        }
        [Fact]
        internal async Task GivenUserChangesTheNameOfASprint_AndGivenASprintWithTheSameNewNameHasAlreadyExistedForThisProject_WhenChangingTheName_ThenShouldBePreventedFromChangingIt()
        {
            IProjectSettingService service = _serviceScope.ServiceProvider.GetRequiredService<IProjectSettingService>();

            var projectId = await DataFacilitator.DefineProject(
                _serviceScope, name: "Task Management");

            var sprintId = await DataFacilitator.DefineSprint(
                _serviceScope, projectId, name: "Sprint 01");

            var newSprintName = "Sprint 02";

            // Given
            var request = new ChangeSprintName(sprintId, newSprintName);
            await service.Process(new DefineSprint(projectId, newSprintName));

            // When
            Func<Task> actual = async () => await service.Process(request);

            // Then
            await actual.Should().BeSatisfiedWith<AnEntityWithThesePropertiesHasAlreadyExistedFault>();

            // Tear down
            _fixture.EnsureRecreatedDatabase();
        }
    }
}
