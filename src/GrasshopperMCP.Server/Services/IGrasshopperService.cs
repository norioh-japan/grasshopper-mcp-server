using System.Threading.Tasks;
using Rhino.Geometry;
using System.Collections.Generic;

namespace GrasshopperMCP.Server.Services
{
    public interface IGrasshopperService
    {
        Task<string> CreateDefinitionAsync(string name);
        Task<(string definitionId, bool success)> OpenDefinitionAsync(string filePath);
        Task<bool> SaveDefinitionAsync(string definitionId, string filePath, string format = "gh");
        Task<string> AddComponentAsync(string definitionId, string componentType, Point3d position, string nickname = null);
        Task<bool> RemoveComponentAsync(string definitionId, string componentId);
        Task<bool> SetComponentParameterAsync(string definitionId, string componentId, string paramName, object value);
        Task<string> ConnectComponentsAsync(string definitionId, ComponentPort source, ComponentPort target);
        Task<bool> DisconnectComponentsAsync(string definitionId, string connectionId);
        Task<SolutionResult> RunSolutionAsync(string definitionId);
        Task<object> GetSolutionResultsAsync(string definitionId, string componentId, int paramIndex, string format = "json");
    }

    public class ComponentPort
    {
        public string ComponentId { get; set; }
        public int ParamIndex { get; set; }
    }

    public class SolutionResult
    {
        public bool Success { get; set; }
        public int RuntimeMs { get; set; }
        public List<string> Warnings { get; set; } = new List<string>();
        public List<string> Errors { get; set; } = new List<string>();
    }
}
