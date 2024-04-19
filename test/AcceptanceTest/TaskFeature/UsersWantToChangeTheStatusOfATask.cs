using Contract;
using Domain.Task;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace AcceptanceTest.TaskFeature
{
    /// <summary>
    /// As a user
    /// I want to change the status of a task
    /// So that I should be able to access the task with the new status
    /// </summary>
    public class UsersWantToChangeTheStatusOfATask : IClassFixture<TaskFixture>
    {
        private IServiceScope _serviceScope;
        private readonly TaskFixture _fixture;
        public UsersWantToChangeTheStatusOfATask(TaskFixture fixture)
        {
            _fixture = fixture;
            _serviceScope = _fixture.ServiceProvider.CreateAsyncScope();
        }
 
        [Fact]
        internal async System.Threading.Tasks.Task GivenUserChangesTheStatusOfATask_WhenChangingTheStatus_ThenShouldBeAbleToAccessItWithTheNewStatus()
        {
            ITaskService service = _serviceScope.ServiceProvider.GetRequiredService<ITaskService>();

            var projectId = await DataFacilitator.DefineProject(
                _serviceScope, name: "Task Management");

            var taskId = await DataFacilitator.AddTask(
                _serviceScope, 
                projectId,
                description: "Define a new module as the task module.",
                sprintId: null);
            
            var newStatus = TaskStatus.InProgress;

            // Given
            var request = new ChangeTaskStatus(taskId, newStatus);

            // When
            Func<System.Threading.Tasks.Task> actual = async () => await service.Process(request);

            var requestToGet = new GetTask(taskId);

            // Then
            await actual.Should().NotThrowAsync();

            var retrievedTask = await service.Process(requestToGet);
            retrievedTask!.Status.Should().Be(newStatus);

            // Tear Down
            _fixture.EnsureRecreatedDatabase(); 
        }
    }
}
