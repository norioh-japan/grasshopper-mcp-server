using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using GrasshopperMCP.Server.Services;
using GrasshopperMCP.Server.Middleware;

namespace GrasshopperMCP.Server
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // MCP related services
            services.AddMcpServer(options =>
            {
                options.ToolsPath = "tools";
                options.ResourcesPath = "resources";
                options.EnableSecurity = Configuration.GetValue<bool>("GrasshopperMcp:Server:EnableSecurity");
                options.ApiKeys = Configuration.GetSection("GrasshopperMcp:Server:ApiKeys").Get<string[]>();
            });
            
            // Grasshopper integration services
            services.AddSingleton<IGrasshopperService, GrasshopperService>();
            services.AddSingleton<IDefinitionManager, DefinitionManager>();
            services.AddSingleton<IComponentManager, ComponentManager>();
            
            // CORS configuration
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins(Configuration.GetSection("GrasshopperMcp:Server:AllowedOrigins").Get<string[]>())
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });
            
            services.AddControllers();
            services.AddLogging();
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            // Error handling middleware
            app.UseMiddleware<ErrorHandlingMiddleware>();
            
            // Enable CORS
            app.UseCors();
            
            // MCP Middleware
            app.UseMcp();
            
            // WebSocket configuration
            app.UseWebSockets(new WebSocketOptions
            {
                KeepAliveInterval = TimeSpan.FromMinutes(2),
                ReceiveBufferSize = 4 * 1024 // 4KB
            });
            
            // WebSocket handler middleware
            app.UseMiddleware<WebSocketMiddleware>();
            
            // Routing and endpoints
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapMcpEndpoints();
                endpoints.MapControllers();
            });
        }
    }
}
