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
    /// I want to edit a task
    /// So that I should be able to access the task with the new information
    /// </summary>
    public class UserWantsToEditATask : IClassFixture<TaskFixture>
    {
        private IServiceScope _serviceScope;
        private readonly TaskFixture _fixture;
        public UserWantsToEditATask(TaskFixture fixture)
        {
            _fixture = fixture;
            _serviceScope = _fixture.ServiceProvider.CreateAsyncScope();
        }

        [Fact]
        internal async System.Threading.Tasks.Task GivenUserEditsATask_WhenEditityTask_ThenShouldBeAbleToAccessItWithTheNewValues()
        {
            ITaskService service = _serviceScope.ServiceProvider.GetRequiredService<ITaskService>();

            var projectId = await DataFacilitator.DefineProject(
                _serviceScope, name: "Task Management");

            var taskId = await DataFacilitator.AddTask(
                _serviceScope,
                projectId,
                description: "Define a new module as the task module.",
                sprintId: null);

            var newDescription = "Implement the project feature as an application service.";

            var newSprintName = "Sprint 01";
            var newSprintId = await DataFacilitator.DefineSprint(
                _serviceScope, projectId, newSprintName); ;

            var newStatus = TaskStatus.Completed;

            // Given
            var request = new EditTask(taskId, newDescription, newStatus, newSprintId);

            // When
            Func<System.Threading.Tasks.Task> actual = async () => await service.Process(request);

            var requestToGet = new GetTask(taskId);

            // Then
            await actual.Should().NotThrowAsync();

            var retrievedTask = await service.Process(requestToGet);
            retrievedTask!.Description.Should().Be(newDescription);
            retrievedTask!.SprintName.Should().Be(newSprintName);

            // Tear Down
            _fixture.EnsureRecreatedDatabase();
        }
    }
}
