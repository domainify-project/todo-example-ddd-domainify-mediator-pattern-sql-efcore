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
    /// I want to delete a sprint permanently
    /// So that I shold not be able to access the sprint and can not restore it
    /// </summary>
    public class UserWantsToDeleteASprintPermanently : IClassFixture<ProjectSettingFixture>
    {
        private IServiceScope _serviceScope;
        private readonly ProjectSettingFixture _fixture;
        public UserWantsToDeleteASprintPermanently(ProjectSettingFixture fixture)
        {
            _fixture = fixture;
            _serviceScope = _fixture.ServiceProvider.CreateAsyncScope();
        }

        [Fact]
        internal async Task GivenUserDeletesASprintPermanently_WhenDeletingSprint_ThenShouldNotAccessButCanRestoreIt()
        {
            IProjectSettingService service = _serviceScope.ServiceProvider.GetRequiredService<IProjectSettingService>();

            var projectId = await DataFacilitator.DefineProject(
                _serviceScope, name: "Task Management");

            var sprintId = await DataFacilitator.DefineSprint(
                _serviceScope, projectId, name: "Sprint 01");

            await service.Process(new DeleteSprint(sprintId));

            // Given
            var requestToDeletePermanently = new DeleteSprintPermanently(sprintId);

            // When
            Func<Task> actualForDeleting = async () => await service.Process(requestToDeletePermanently);

            var requestToGet = new GetSprint(sprintId);
            Func<Task> actualForGetting = async () => await service.Process(requestToGet);

            var requestToRestore = new RestoreSprint(sprintId);
            Func<Task> actualForRestoting =
                async () => await service.Process(requestToRestore);

            // Then
            await actualForDeleting.Should().NotThrowAsync();
            await actualForGetting.Should().BeSatisfiedWith<NoEntityWasFoundFault>();
            await actualForRestoting.Should().BeSatisfiedWith<NoEntityWasFoundFault>();

            // Tear down
            _fixture.EnsureRecreatedDatabase();
        }
    }
}
