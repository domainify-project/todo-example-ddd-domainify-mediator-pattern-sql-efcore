﻿using Contract;
using Domain.ProjectSetting;
using Domain.Task;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace AcceptanceTest
{
    public static class DataFacilitator
    {
        public static async Task<string> DefineProject(
            IServiceScope _serviceScope, string name)
        {
            return await _serviceScope.ServiceProvider.
                GetRequiredService<IProjectSettingService>().Process(
                new DefineProject(name));
        }

        public static async Task<string> DefineSprint(
            IServiceScope _serviceScope, string projectId, string name)
        {
            return await _serviceScope.ServiceProvider.
                GetRequiredService<IProjectSettingService>().Process(
                new DefineSprint(projectId, name));
        }

        public static async Task<string> AddTask(
            IServiceScope _serviceScope, string projectId, string description, string? sprintId = null)
        {
            return await _serviceScope.ServiceProvider.
                GetRequiredService<ITaskService>().Process(
                new AddTask(
                    projectId,
                    description: description,
                    sprintId));
        }
    }
}
