using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Xunit;
using System;
using FluentAssertions;
using Contract;
using Domain.ProjectSetting;

namespace AcceptanceTest.ProjectSettingFeature
{
    /// <summary>
    /// As a user
    /// I want to restore a deleted sprint
    /// So that I should be able to access the sprint again
    /// </summary>
    public class UserWantsToRestoreADeletedSprint : IClassFixture<ProjectSettingFixture>
    {
        private IServiceScope _serviceScope;
        private readonly ProjectSettingFixture _fixture;
        public UserWantsToRestoreADeletedSprint(ProjectSettingFixture fixture)
        {
            _fixture = fixture;
            _serviceScope = _fixture.ServiceProvider.CreateAsyncScope();
        }

        [Fact]
        internal async Task GivenUserRestoresAnDeletedSprint_WhenRestoringSprint_ThenShouldAccessIt()
        {
            IProjectSettingService service = _serviceScope.ServiceProvider.GetRequiredService<IProjectSettingService>();
    
            var projectId = await DataFacilitator.DefineProject(
                _serviceScope, name: "Task Management");

            var sprintId = await DataFacilitator.DefineSprint(
                _serviceScope, projectId, name: "Sprint 01");

            await _serviceScope.ServiceProvider.
                GetRequiredService<IProjectSettingService>().Process(
                new DeleteSprint(sprintId));

            // Given
            var request = new RestoreSprint(sprintId);

            // When
            Func<Task> actual = async () => await service.Process(request);

            var requestToGet = new GetSprint(sprintId);
            Func<Task> actualForGetting = async () => await service.Process(requestToGet);

            // Then
            await actual.Should().NotThrowAsync();
            await actualForGetting.Should().NotThrowAsync();

            // Tear down
            _fixture.EnsureRecreatedDatabase();
        }
    }
}
