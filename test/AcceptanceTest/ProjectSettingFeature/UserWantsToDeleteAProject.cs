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
    /// I want to delete a project
    /// So that I should not be able to access the project and can restore it
    /// </summary>
    public class UserWantsToDeleteAProject : IClassFixture<ProjectSettingFixture>
    {
        private IServiceScope _serviceScope;
        private readonly ProjectSettingFixture _fixture;
        public UserWantsToDeleteAProject(ProjectSettingFixture fixture)
        {
            _fixture = fixture;
            _serviceScope = _fixture.ServiceProvider.CreateAsyncScope();
        }

        [Fact]
        internal async Task GivenUserDeletesAProject_WhenDeletingProject_ThenShouldNotAccessButCanRestoreIt()
        {
            var service = _serviceScope!.ServiceProvider.GetRequiredService<IProjectSettingService>();

            var projectId = await DataFacilitator.DefineProject(
                _serviceScope, name: "Task Management");

            // Given
            var requestToDelete = new DeleteProject(projectId);

            // When
            Func<Task> actualForDeleting = async () => await service.Process(requestToDelete);

            var requestToRestore = new RestoreProject(projectId);
            Func<Task> actualForRestoting = async () => await service.Process(requestToRestore);

            var requestToGet = new GetProject(projectId);
            Func<Task> actualForGetting = async () => await service.Process(requestToGet);

            // Then
            await actualForDeleting.Should().NotThrowAsync();
            await actualForGetting.Should().BeSatisfiedWith<NoEntityWasFoundFault>();
            await actualForRestoting.Should().NotThrowAsync();

            // Tear down
            _fixture.EnsureRecreatedDatabase();
        }
    }
}
