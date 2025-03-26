using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino;

namespace GrasshopperMCP.Plugin
{
    public class GrasshopperMCPPlugin : GH_AssemblyInfo
    {
        public override string Name => "Grasshopper MCP";
        
        public override string Description => "MCPサーバーを通じてAIとGrasshopperを連携させるプラグイン";
        
        public override Guid Id => new Guid("FEA145D6-6E4A-4A95-BC1B-E04D5950E4AE");
        
        public override string AuthorName => "MCP Grasshopper Team";
        
        public override string AuthorContact => "https://github.com/norioh-japan/grasshopper-mcp-server";
        
        public override string Version => "1.0.0";
        
        public override Bitmap Icon => null;
        
        public override string AssemblyVersion => "1.0.0.0";
    }
    
    public class GrasshopperMCPPriority : GH_AssemblyPriority
    {
        public override GH_LoadingInstruction PriorityLoad()
        {
            // プラグイン起動時のフック
            RhinoApp.WriteLine("Grasshopper MCP プラグインが読み込まれました");
            return GH_LoadingInstruction.Proceed;
        }
    }
}
