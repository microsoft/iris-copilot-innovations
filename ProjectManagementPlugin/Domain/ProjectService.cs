// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IProjectService.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ProjectManagementPlugin.Domain
{
    using System.Threading.Tasks;
    using ProjectManagementPlugin.DataAccess;

    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository projectRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectService"/> class.
        /// Constructor for GenerateSearchIndex
        /// </summary>
        /// <param>
        public ProjectService(IProjectRepository projectRepository)
        {
            this.projectRepository = projectRepository;
        }

        /// <summary>
        /// Gets project info
        /// </summary>
        /// <param name="projectId">Project Id</param>
        /// <returns>Project details</returns>
        public async Task<string> GetProjectDetails(string projectId)
        {
            return await this.projectRepository.GetProjectDetails(projectId);
        }
    }
}