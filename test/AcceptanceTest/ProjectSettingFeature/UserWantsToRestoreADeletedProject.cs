using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Xunit;
using System;
using FluentAssertions;
using Contract;
using Domain.ProjectSetting;
using Domainify.Domain;

namespace AcceptanceTest.ProjectSettingFeature
{
    /// <summary>
    /// As a user
    /// I want to restore a deleted project
    /// So that I should be able to access the project again
    /// </summary>
    public class UserWantsToRestoreADeletedProject : IClassFixture<ProjectSettingFixture>
    {
        private IServiceScope _serviceScope;
        private readonly ProjectSettingFixture _fixture;
        public UserWantsToRestoreADeletedProject(ProjectSettingFixture fixture)
        {
            _fixture = fixture;
            _serviceScope = _fixture.ServiceProvider.CreateAsyncScope();
        }

        [Fact]
        internal async Task GivenUserRestoresAnDeletedProject_WhenRestoringProject_ThenShouldAccessIt()
        {
            var service = _serviceScope!.ServiceProvider.GetRequiredService<IProjectSettingService>();

            var projectId = await DataFacilitator.DefineProject(
                _serviceScope, name: "Task Management");

            await _serviceScope.ServiceProvider.
                GetRequiredService<IProjectSettingService>().Process(
                new DeleteProject(projectId));

            // Given
            var request = new RestoreProject(projectId);

            // When
            Func<Task> actual = async () => await service.Process(request);

            var requestToGet = new GetProject(projectId);
            Func<Task> actualForGetting = async () => await service.Process(requestToGet);

            // Then
            await actual.Should().NotThrowAsync();
            await actualForGetting.Should().NotThrowAsync();

            // Tear down
            _fixture.EnsureRecreatedDatabase();
        }
    }
}
