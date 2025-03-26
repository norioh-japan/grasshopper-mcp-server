using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Rhino.Geometry;
using GrasshopperMCP.Server.Services;

namespace GrasshopperMCP.Server.Tools
{
    [McpTool("add_component")]
    public class AddComponentTool : IMcpTool
    {
        private readonly IGrasshopperService _grasshopperService;
        private readonly ILogger<AddComponentTool> _logger;
        
        public AddComponentTool(
            IGrasshopperService grasshopperService,
            ILogger<AddComponentTool> logger)
        {
            _grasshopperService = grasshopperService;
            _logger = logger;
        }
        
        public async Task<IActionResult> ExecuteAsync(JObject parameters)
        {
            try
            {
                // パラメータの検証
                if (!parameters.ContainsKey("definition_id") || 
                    !parameters.ContainsKey("component_type") || 
                    !parameters.ContainsKey("position"))
                {
                    return new BadRequestObjectResult(new
                    {
                        error = "必須パラメータが不足しています"
                    });
                }
                
                var definitionId = parameters["definition_id"].Value<string>();
                var componentType = parameters["component_type"].Value<string>();
                var position = new Point3d(
                    parameters["position"]["x"].Value<double>(),
                    parameters["position"]["y"].Value<double>(),
                    0.0);
                
                var nickname = parameters["nickname"]?.Value<string>();
                
                // サービスを通じてコンポーネントを追加
                var componentId = await _grasshopperService.AddComponentAsync(
                    definitionId, componentType, position, nickname);
                
                if (componentId == null)
                {
                    return new BadRequestObjectResult(new
                    {
                        error = "コンポーネントの追加に失敗しました"
                    });
                }
                
                // 結果の返却
                return new JsonResult(new
                {
                    component_id = componentId
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "コンポーネント追加中にエラーが発生しました");
                return new BadRequestObjectResult(new
                {
                    error = "コンポーネントの追加に失敗しました",
                    message = ex.Message
                });
            }
        }
    }
}
