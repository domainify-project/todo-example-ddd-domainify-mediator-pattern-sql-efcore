using Microsoft.Extensions.DependencyInjection;
using Xunit;
using System;
using FluentAssertions;
using Domain.TaskAggregation;
using Contract;
using Domainify.Domain;

namespace AcceptanceTest.TaskFeature
{
    /// <summary>
    /// As a user
    /// I want to delete a task
    /// So that I can not to be able to access the task and can restore it
    /// </summary>
    public class UserWantsToDeleteATask : IClassFixture<TaskFixture>
    {
        private IServiceScope _serviceScope;
        private readonly TaskFixture _fixture;
        public UserWantsToDeleteATask(TaskFixture fixture)
        {
            _fixture = fixture;
            _serviceScope = _fixture.ServiceProvider.CreateAsyncScope();
        }

        [Fact]
        internal async System.Threading.Tasks.Task GivenUserDeletesATask_WhenDeletingTask_ThenShouldNotAccessButCanRestoreIt()
        {
            ITaskService service = _serviceScope.ServiceProvider.GetRequiredService<ITaskService>();

            var projectId = await DataFacilitator.DefineProject(
                _serviceScope, name: "Task Management");

            var taskId = await DataFacilitator.AddTask(
                _serviceScope, 
                projectId,
                description: "Define a new module as the task module.",
                sprintId: null);

            // Given
            var requestToDelete = new DeleteTask(taskId);

            // When
            Func<System.Threading.Tasks.Task> actualForDeleting = async () => await service.Process(requestToDelete);

            var requestToRestore = new RestoreTask(taskId);
            Func<System.Threading.Tasks.Task> actualForRestoting = async () => await service.Process(requestToRestore);

            var requestToGet = new GetTask(taskId);
            Func<System.Threading.Tasks.Task> actualForGetting = async () => await service.Process(requestToGet);

            // Then
            await actualForDeleting.Should().NotThrowAsync();
            await actualForGetting.Should().BeSatisfiedWith<NoEntityWasFoundFault>();
            await actualForRestoting.Should().NotThrowAsync();

            // Tear Down
            _fixture.EnsureRecreatedDatabase();
        }
    }
}
