using Contract;
using Domain.ProjectSetting;
using Domainify.Domain;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace AcceptanceTests.ProjectSettingFeature
{
    /// <summary>
    /// As a user
    /// I want to delete a project permanently
    /// So that I should not be able to access the project and can not restore it
    /// </summary>
    public class UserWantsToDeleteAProjectPermanently : IClassFixture<ProjectSettingFixture>
    {
        private IServiceScope _serviceScope;
        private readonly ProjectSettingFixture _fixture;
        public UserWantsToDeleteAProjectPermanently(ProjectSettingFixture fixture)
        {
            _fixture = fixture;
            _serviceScope = _fixture.ServiceProvider.CreateAsyncScope();
        }

        [Fact]
        internal async Task GivenUserDeletesAProjectPermanently_WhenDeletingProject_ThenShouldNotAccessButCanRestoreIt()
        {
            var service = _serviceScope!.ServiceProvider.GetRequiredService<IProjectSettingService>();

            var projectId = await DataFacilitator.DefineProject(
                _serviceScope, name: "Task Management");

            await service.Process(new DeleteProject(projectId));

            // Given
            var requestToDeletePermanently = new DeleteProjectPermanently(projectId);

            // When
            Func<Task> actualForDeletingPermanently = async () => await service.Process(requestToDeletePermanently);

            var requestToGet = new GetProject(projectId);
            Func<Task> actualForGetting = async () => await service.Process(requestToGet);

            var requestToRestore = new RestoreProject(projectId);
            Func<Task> actualForRestoting = async () => await service.Process(requestToRestore);

            // Then
            await actualForDeletingPermanently.Should().NotThrowAsync();
            await actualForGetting.Should().BeSatisfiedWith<NoEntityWasFoundFault>();
            await actualForRestoting.Should().BeSatisfiedWith<NoEntityWasFoundFault>();

            // Tear down
            _fixture.EnsureRecreatedDatabase();
        }
    }
}
