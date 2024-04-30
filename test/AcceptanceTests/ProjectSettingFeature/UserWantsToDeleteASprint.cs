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
    /// I want to delete a sprint
    /// So that I shold not be able to access the sprint and can restore it
    /// </summary>
    public class UserWantsToDeleteASprint : IClassFixture<ProjectSettingFixture>
    {
        private IServiceScope _serviceScope;
        private readonly ProjectSettingFixture _fixture;
        public UserWantsToDeleteASprint(ProjectSettingFixture fixture)
        {
            _fixture = fixture;
            _serviceScope = _fixture.ServiceProvider.CreateAsyncScope();
        }

        [Fact]
        internal async Task GivenUserDeletesASprint_WhenDeletingSprint_ThenShouldNotAccessButCanRestoreIt()
        {
            IProjectSettingService service = _serviceScope.ServiceProvider.GetRequiredService<IProjectSettingService>();

            var projectId = await DataFacilitator.DefineProject(
                _serviceScope, name: "Task Management");

            var sprintId = await DataFacilitator.DefineSprint(
                _serviceScope, projectId, name: "Sprint 01");

            // Given
            var requestToDelete = new DeleteSprint(sprintId);

            // When
            Func<Task> actualForDeleting = async () => await service.Process(requestToDelete);

            var requestToRestore = new RestoreSprint(sprintId);
            Func<Task> actualForRestoting = async () => await service.Process(requestToRestore);

            var requestToGet = new GetSprint(sprintId);
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
