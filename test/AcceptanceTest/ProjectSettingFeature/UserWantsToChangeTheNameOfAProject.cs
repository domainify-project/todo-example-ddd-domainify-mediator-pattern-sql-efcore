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
    /// I want to change the name of a project
    /// So that I should be able to access the project with the new name
    /// </summary>
    public class UserWantsToChangeTheNameOfAProject : IClassFixture<ProjectSettingFixture>
    {
        private IServiceScope _serviceScope;
        private readonly ProjectSettingFixture _fixture;
        public UserWantsToChangeTheNameOfAProject(ProjectSettingFixture fixture)
        {
            _fixture = fixture;
            _serviceScope = _fixture.ServiceProvider.CreateAsyncScope();
        }

        [Fact]
        
        internal async Task GivenUserChangesTheNameOfAProject_WhenChangingTheName_ThenShouldBeAbleToAccessItWithTheNewName()
        {
            var service = _serviceScope!.ServiceProvider.GetRequiredService<IProjectSettingService>();

            var projectId = await DataFacilitator.DefineProject(
                _serviceScope, name: "Task Management");

            var newProjectName = "Task Board";

            // Given
            var request = new ChangeProjectName(projectId, newProjectName);

            // When
            Func<Task> actual = async () => await service.Process(request);

            var requestToGet = new GetProject(projectId);

            // Then
            await actual.Should().NotThrowAsync();

            var retrievedProject = await service.Process(requestToGet);
            retrievedProject!.Name.Should().Be(newProjectName);

            // Tear down
            _fixture.EnsureRecreatedDatabase();
        }

        [Fact]
        internal async Task GivenUserChangesTheNameOfAProject_AndGivenAProjectWithTheSameNewNameHasAlreadyExisted_WhenChangingTheName_ThenShouldBePreventedFromChangingIt()
        {
            var service = _serviceScope!.ServiceProvider.GetRequiredService<IProjectSettingService>();

            var projectId = await DataFacilitator.DefineProject(
                _serviceScope, name: "Task Management");

            var newProjectName = "Task Board";

            // Given
            var request = new ChangeProjectName(projectId, newProjectName);
            await service.Process(new DefineProject(newProjectName));

            // When
            Func<Task> actual = async () => await service.Process(request);

            // Then
            await actual.Should().BeSatisfiedWith<AnEntityWithThesePropertiesHasAlreadyExistedFault>();

            // Tear down
            _fixture.EnsureRecreatedDatabase();
        }
    }
}
