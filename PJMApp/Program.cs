// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PJMApp
{
    using System.Text;
    using Microsoft.SemanticKernel;
    using Microsoft.SemanticKernel.Memory;
    using Microsoft.SemanticKernel.Orchestration;
    using Microsoft.SemanticKernel.SkillDefinition;
    using ProjectManagementPlugin;
    using ProjectManagementPlugin.DataAccess;
    using ProjectManagementPlugin.Domain;

    /// <summary>
    /// Program class
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Main method
        /// </summary>
        /// <param name="args">Commandline arguments</param>
        public static async Task Main(string[] args)
        {
            // Initialize SK and Import skills - Done by Iris Copilot Platform's Zero-Dev
            var semanticKernel = Kernel.Builder.WithAzureChatCompletionService("iris-selfhelp", "https://iris-openai-dev.openai.azure.com/", "INPUT_KEY_HERE")
                            .WithAzureTextEmbeddingGenerationService("text-embedding-ada-002", "https://iris-openai-dev.openai.azure.com/", "INPUT_KEY_HERE")
                            .WithMemoryStorage(new VolatileMemoryStore())
                            .Build();

            semanticKernel.ImportSkill(new ProjectNativePlugin(semanticKernel, new ProjectService(new ProjectRepository())));
            semanticKernel.ImportSemanticSkillFromDirectory("C:\\Users\\pvelmurugan\\Downloads\\PJMCopilotSolution\\PJMCopilotSolution\\ProjectManagementPlugin", "SemanticPlugins");

            // Take out the function and invoke - Done by Iris Copilot Platform's Orchestrator
            semanticKernel.Skills.TryGetFunction("_GLOBAL_FUNCTIONS_", "GetProjectDetails", out ISKFunction getProjectDetails);

            // Passing chat transcript - Done by Iris Copilot Platform's Orchestrator
            var chatTranscript = new StringBuilder("Hello, How can I help you?");
            Console.WriteLine($"AI: {chatTranscript}\r\nUser:");
            var userInput = Console.ReadLine();
            do
            {
                ContextVariables variables = new ContextVariables();
                chatTranscript.AppendLine($"User: {userInput}");
                variables["ChatTranscript"] = chatTranscript.ToString();

                var pluginResponse = await semanticKernel.RunAsync(variables, getProjectDetails);
                chatTranscript.AppendLine($"AI: {pluginResponse.Result}");
                Console.WriteLine($"AI:{pluginResponse.Result}\r\nUser:");
                
                userInput = Console.ReadLine();
            } 
            while (!string.Equals(userInput, "exit", StringComparison.InvariantCultureIgnoreCase));
        }
    }
}