using Contract;
using Domain.ProjectSettingAggregation;
using Domain.TaskAggregation;
using FluentAssertions;
using FluentAssertions.Common;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace AcceptanceTest.TaskFeature
{
    /// <summary>
    /// As a user
    /// I want to add a task to a project
    /// So that I should be able to access the task
    /// </summary>
    public class UserWantsToAddATask : IClassFixture<TaskFixture>
    {
        private IServiceScope _serviceScope;
        private readonly TaskFixture _fixture;
        public UserWantsToAddATask(TaskFixture fixture)
        {
            _fixture = fixture;
            _serviceScope = _fixture.ServiceProvider.CreateAsyncScope();
        }

        [Fact]
        internal async System.Threading.Tasks.Task GivenUserAddsATask_WhenAddingTask_ThenShouldAccessIt()
        {
            ITaskService service = _serviceScope.ServiceProvider.GetRequiredService<ITaskService>();
 
            var projectId = await DataFacilitator.DefineProject(
                _serviceScope, name: "Task Management");

            var description = "Add a new module as the task module.";

            // Given
            var request = new AddTask(
                projectId: projectId,
                description,
                sprintId: null);

            // When
            Func<System.Threading.Tasks.Task>? actualForGetting = null;
            Func<System.Threading.Tasks.Task> actual = async () =>
            {
                var taskId = await service.Process(request!);
                var requestToGet = new GetTask(taskId);
                actualForGetting = async () => await service.Process(requestToGet);
            };

            // Then
            await actual.Should().NotThrowAsync();
            await actualForGetting?.Should().NotThrowAsync()!;

            // Tear Down
            _fixture.EnsureRecreatedDatabase();
        }
    }
}
