using EasyTest.Models;
using Serilog;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace EasyTest.Factories
{
    public static class ProjectFactory
    {
        private static Project project;
        public static async Task LoadConfigAsync(string projectPath)
        {
            if (!File.Exists(projectPath))
            {
                Log.Error("Error loading configution {configPath}", projectPath);
                return;
            }
            ProjectFactory.project = await LoadFileAsync<Project>(projectPath);
        }

        public static Project GetProject()
        {
            return project;
        }

        public static async Task<T> LoadFileAsync<T>(string fileName)
        {
            string jsonString = await File.ReadAllTextAsync(fileName);
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            return JsonSerializer.Deserialize<T>(jsonString, options);
        }
    }
}
