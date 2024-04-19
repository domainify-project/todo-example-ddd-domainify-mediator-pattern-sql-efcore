using Microsoft.Extensions.DependencyInjection;
using Xunit;
using System;
using FluentAssertions;
using Contract;
using Domain.Task;

namespace AcceptanceTest.TaskFeature
{
    /// <summary>
    /// As a user
    /// I want to restore a deleted task
    /// So that I should be able to access the task again
    /// </summary>
    public class UserWantSToRestoreADeletedTask : IClassFixture<TaskFixture>
    {
        private IServiceScope _serviceScope;
        private readonly TaskFixture _fixture;
        public UserWantSToRestoreADeletedTask(TaskFixture fixture)
        {
            _fixture = fixture;
            _serviceScope = _fixture.ServiceProvider.CreateAsyncScope();
        }

        [Fact]
        internal async System.Threading.Tasks.Task GivenUserRestoresAnDeletedTask_WhenRestoringTask_ThenShouldAccessIt()
        {
            ITaskService service = _serviceScope.ServiceProvider.GetRequiredService<ITaskService>();

            var projectId = await DataFacilitator.DefineProject(
                _serviceScope, name: "Task Management");

            var taskId = await DataFacilitator.AddTask(
                _serviceScope,
                projectId,
                description: "Define a new module as the task module.",
                sprintId: null);

            await _serviceScope.ServiceProvider.
                GetRequiredService<ITaskService>().Process(
            new DeleteTask(taskId));

            // Given
            var request = new RestoreTask(taskId);

            // When
            Func<System.Threading.Tasks.Task> actual = async () => await service.Process(request);

            var requestToGet = new GetTask(taskId);
            Func<System.Threading.Tasks.Task> actualForGetting = async () => await service.Process(requestToGet);

            // Then
            await actual.Should().NotThrowAsync();
            await actualForGetting.Should().NotThrowAsync();

            // Tear Down
            _fixture.EnsureRecreatedDatabase();
        }
    }
}
