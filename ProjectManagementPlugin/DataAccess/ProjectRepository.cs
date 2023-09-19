// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectRepository.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ProjectManagementPlugin.DataAccess
{
    using Newtonsoft.Json;

    public class ProjectRepository : IProjectRepository
    {
        /// <summary>
        /// Gets project info
        /// </summary>
        /// <param name="projectId">Project Id</param>
        /// <returns>Project details</returns>
        public async Task<string> GetProjectDetails(string projectId)
        {
            var result = JsonConvert.SerializeObject(new { projectId, name = "test project" });
            return await Task.FromResult(result);
        }
    }
}