// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IProjectRepository.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ProjectManagementPlugin.DataAccess
{
    public interface IProjectRepository
    {
        /// <summary>
        /// Gets project info
        /// </summary>
        /// <param name="projectId">Project Id</param>
        /// <returns>Project details</returns>
        Task<string> GetProjectDetails(string projectId);
    }
}