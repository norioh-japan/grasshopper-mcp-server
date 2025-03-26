using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GrasshopperMCP.Server.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        
        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }
        
        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, "未処理の例外が発生しました");
            
            var code = HttpStatusCode.InternalServerError; // 500
            var result = string.Empty;
            
            // 例外の種類によってステータスコードを変更
            if (exception is McpException)
            {
                code = HttpStatusCode.BadRequest; // 400
            }
            
            // エラーレスポンスの作成
            var error = new
            {
                error = new
                {
                    code = exception.GetType().Name,
                    message = exception.Message,
                    details = exception.InnerException?.Message,
                    timestamp = DateTime.UtcNow.ToString("o")
                }
            };
            
            // JSONに変換
            result = JsonSerializer.Serialize(error);
            
            // ステータスコードとコンテンツタイプの設定
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            
            // レスポンスの送信
            return context.Response.WriteAsync(result);
        }
    }
    
    // MCPサーバー用の例外クラス
    public class McpException : Exception
    {
        public McpException(string message) : base(message)
        {
        }
        
        public McpException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
