using Contract;
using Domain.ProjectSetting;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace AcceptanceTest.ProjectSettingFeature
{
    /// <summary>
    /// As a user
    /// I want to change the time span of a sprint
    /// So that I can be able to access the sprint with the new time span
    /// </summary>
    public class UserWantsToChangeTheTimeSpanOfASprint : IClassFixture<ProjectSettingFixture>
    {
        private IServiceScope _serviceScope;
        private readonly ProjectSettingFixture _fixture;
        public UserWantsToChangeTheTimeSpanOfASprint(ProjectSettingFixture fixture)
        {
            _fixture = fixture;
            _serviceScope = _fixture.ServiceProvider.CreateAsyncScope();
        }

        [Theory]
        [MemberData(nameof(GetDatePairs))]
        internal async Task GivenUserChangesTheTimeSpanOfASprintToANew_AandGivenStartDateOrEndDateIsEarlierThanTheLastTwelveMonths_WhenChangingTheTimeSpan_ThenShouldBePreventedFromChangingIt(
            DateTime startDate, DateTime endDate)
        {
            IProjectSettingService service = _serviceScope.ServiceProvider.GetRequiredService<IProjectSettingService>();

            var projectId = await DataFacilitator.DefineProject(
                _serviceScope, name: "Task Management");

            var sprintId = await DataFacilitator.DefineSprint(
                _serviceScope, projectId, name: "Sprint 01");

            // Given
            var request = new ChangeSprintTimeSpan(sprintId, startDate, endDate);

            // When
            Func<Task> actual = async () => await service.Process(request);

            // Then
            await actual.Should().BeSatisfiedWith<StartDateAndEndDateOfSprintCanNotBeEarlierThanLastTwelveMonthsFault>();

            // Tear down
            _fixture.EnsureRecreatedDatabase();
        }

        private static IEnumerable<object[]> GetDatePairs()
        {
            yield return new object[] { DateTime.UtcNow.AddMonths(-16), DateTime.UtcNow.AddMonths(-15) };
            yield return new object[] { DateTime.UtcNow.AddMonths(-14), DateTime.UtcNow.AddMonths(-2) };
        }
    }
}
