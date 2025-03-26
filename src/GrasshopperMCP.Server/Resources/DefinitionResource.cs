using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using GrasshopperMCP.Server.Services;

namespace GrasshopperMCP.Server.Resources
{
    [McpResource("definition")]
    public class DefinitionResource : IMcpResource
    {
        private readonly IDefinitionManager _definitionManager;
        private readonly ILogger<DefinitionResource> _logger;
        
        public DefinitionResource(
            IDefinitionManager definitionManager,
            ILogger<DefinitionResource> logger)
        {
            _definitionManager = definitionManager;
            _logger = logger;
        }
        
        public async Task<IActionResult> GetAsync(string resourcePath)
        {
            try
            {
                // リソースパスからIDを抽出
                var definitionId = resourcePath;
                
                // 定義情報の取得
                var definition = await _definitionManager.GetDefinitionAsync(definitionId);
                if (definition == null)
                {
                    return new NotFoundResult();
                }
                
                // 定義の情報を返却
                return new JsonResult(new
                {
                    id = definitionId,
                    name = definition.DisplayName,
                    components_count = definition.ObjectCount,
                    modified = definition.Modified,
                    file_path = definition.FilePath,
                    is_saved = !definition.IsNew()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"定義の取得中にエラーが発生しました: {resourcePath}");
                return new BadRequestObjectResult(new
                {
                    error = "定義の取得に失敗しました",
                    message = ex.Message
                });
            }
        }
        
        public async Task<IActionResult> PutAsync(string resourcePath, object content)
        {
            // 定義の更新処理（名前変更など）
            // 実装は省略
            return new NotImplementedResult();
        }
        
        public async Task<IActionResult> DeleteAsync(string resourcePath)
        {
            try
            {
                // 定義の削除
                var definitionId = resourcePath;
                var success = await _definitionManager.RemoveDefinitionAsync(definitionId);
                
                if (!success)
                {
                    return new NotFoundResult();
                }
                
                return new OkResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"定義の削除中にエラーが発生しました: {resourcePath}");
                return new BadRequestObjectResult(new
                {
                    error = "定義の削除に失敗しました",
                    message = ex.Message
                });
            }
        }
    }
}
