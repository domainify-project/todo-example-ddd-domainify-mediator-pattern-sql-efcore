using Microsoft.Extensions.DependencyInjection;
using Xunit;
using System;
using FluentAssertions;
using Domain.Task;
using Contract;
using Domainify.Domain;

namespace AcceptanceTest.TaskFeature
{
    /// <summary>
    /// As a user
    /// I want to delete a task permanently
    /// So that I can not to be able to access the task and can not restore it
    /// </summary>
    public class UserWantsToDeleteATaskPermanently : IClassFixture<TaskFixture>
    {
        private IServiceScope _serviceScope;
        private readonly TaskFixture _fixture;
        public UserWantsToDeleteATaskPermanently(TaskFixture fixture)
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

            await service.Process(new DeleteTask(taskId));

            // Given
            var requestToDeletePermanently = new DeleteTaskPermanently(taskId);

            // When
            Func<System.Threading.Tasks.Task> actualForDeletingPermanently = async () => await service.Process(requestToDeletePermanently);

            var requestToRestore = new RestoreTask(taskId);
            Func<System.Threading.Tasks.Task> actualForRestoting = async () => await service.Process(requestToRestore);

            var requestToGet = new GetTask(taskId);
            Func<System.Threading.Tasks.Task> actualForGetting = async () => await service.Process(requestToGet);

            // Then
            await actualForDeletingPermanently.Should().NotThrowAsync();
            await actualForGetting.Should().BeSatisfiedWith<NoEntityWasFoundFault>();
            await actualForRestoting.Should().BeSatisfiedWith<NoEntityWasFoundFault>();

            // Tear Down
            _fixture.EnsureRecreatedDatabase();
        }
    }
}
