// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectNativePlugin.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ProjectManagementPlugin
{
    using System.ComponentModel;
    using System.Text;
    using Microsoft.SemanticKernel;
    using Microsoft.SemanticKernel.Orchestration;
    using Microsoft.SemanticKernel.SkillDefinition;
    using Newtonsoft.Json;
    using ProjectManagementPlugin.Domain;

    public class ProjectNativePlugin
    {
        private readonly IKernel semanticKernel;
        private readonly IProjectService projectService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectNativePlugin"/> class.
        /// </summary>
        /// <param name="semanticKernel">Semantic Kernel instance</param>
        /// <param name="projectService">Project service instance</param>
        public ProjectNativePlugin(IKernel semanticKernel, IProjectService projectService) 
        {
            this.semanticKernel = semanticKernel ?? throw new ArgumentNullException(nameof(semanticKernel));
            this.projectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
        }

        // This example would be used as few-shot examples along-side RAG Pattern
        private const string EntityExtractorProjectExamples = @"1. Need: list of project ids of last user ask
        Input: Give me info on project id 3696573
        Output: ['3696573']

        2. Need: list of project ids of last user ask
        Input: Give me a 20 word description of project 1234567 and 1234253
        Output: ['1234567', '1234253']

        3. Need: list of project ids of last user ask
        Input: User: Give me info on project
        Output: Missing project id, please provide the project id

        4. Need: list of project ids of last user ask
        Input: ........
               AI: {'projectId': '12345', 'name': 'test project'}
               User: Give me info on project 12345
        Output: ['12345']";

        /// <summary>
        /// Gets project details using external APIs/SDKs. This is a Native function
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [SKFunction, SKName("GetProjectDetails"), Description("Get details of the projectId")]
        public async Task<string> GetProjectDetails(SKContext chatVariables)
        {
            this.semanticKernel.Skills.TryGetFunction("SemanticPlugins", "EntityExtractorFunction", out ISKFunction entityExtractor);

            ContextVariables eeContext = new ContextVariables();
            eeContext["Example"] = EntityExtractorProjectExamples;
            eeContext["Need"] = "list of project ids of last user ask";
            eeContext["Input"] = chatVariables["ChatTranscript"];

            var extractorResult = await this.semanticKernel.RunAsync(eeContext, entityExtractor);
            var projectIdsList = JsonConvert.DeserializeObject<List<string>>(extractorResult.Result);

            var pluginResponse = new StringBuilder();
            foreach(var projectId in projectIdsList)
            {
                var projectDetails = await this.projectService.GetProjectDetails(projectId);
                pluginResponse.AppendLine(projectDetails);
            }
            
            return pluginResponse.ToString();
        }
    }
}