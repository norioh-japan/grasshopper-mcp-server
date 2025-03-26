using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Net.Http;
using System.Threading.Tasks;

namespace GrasshopperMCP.Plugin
{
    public class GrasshopperMCPComponent : GH_Component
    {
        private string _serverUrl = "http://localhost:5000";
        private bool _isConnected = false;
        private List<string> _logMessages = new List<string>();
        private HttpClient _httpClient;
        
        public GrasshopperMCPComponent()
            : base(
                "MCP Server",
                "MCP",
                "MCPサーバーに接続してAIによるパラメトリックデザインを可能にします",
                "Extra",
                "Utility")
        {
            _httpClient = new HttpClient();
        }
        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("サーバーURL", "URL", "MCPサーバーのURL", GH_ParamAccess.item, _serverUrl);
            pManager.AddBooleanParameter("有効化", "E", "MCPサーバー接続の有効化", GH_ParamAccess.item, false);
        }
        
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("状態", "S", "サーバー接続状態", GH_ParamAccess.item);
            pManager.AddTextParameter("ログ", "L", "操作ログ", GH_ParamAccess.list);
        }
        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string serverUrl = _serverUrl;
            bool enable = false;
            
            if (!DA.GetData(0, ref serverUrl)) return;
            if (!DA.GetData(1, ref enable)) return;
            
            if (enable && !_isConnected)
            {
                // サーバー接続の開始
                StartServer(serverUrl);
                _isConnected = true;
                _logMessages.Add($"MCPサーバーに接続しました: {serverUrl}");
            }
            else if (!enable && _isConnected)
            {
                // サーバー接続の停止
                StopServer();
                _isConnected = false;
                _logMessages.Add("MCPサーバーから切断しました");
            }
            
            // 出力の設定
            DA.SetData(0, _isConnected ? "接続済み" : "未接続");
            DA.SetDataList(1, _logMessages);
        }
        
        private void StartServer(string serverUrl)
        {
            try
            {
                _serverUrl = serverUrl;
                
                // サーバー接続の確認
                Task.Run(async () =>
                {
                    try
                    {
                        var response = await _httpClient.GetAsync($"{serverUrl}/health");
                        if (response.IsSuccessStatusCode)
                        {
                            AddRuntimeMessage(GH_RuntimeMessageLevel.Remark, "MCPサーバーに正常に接続しました");
                        }
                        else
                        {
                            AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "MCPサーバーは実行中ですが、状態が正常ではありません");
                        }
                    }
                    catch (Exception ex)
                    {
                        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, $"サーバー接続エラー: {ex.Message}");
                        _isConnected = false;
                    }
                });
                
                _logMessages.Add("サーバー接続を確立しました");
            }
            catch (Exception ex)
            {
                _logMessages.Add($"サーバー起動エラー: {ex.Message}");
                _isConnected = false;
            }
        }
        
        private void StopServer()
        {
            try
            {
                // サーバー接続の停止
                _logMessages.Add("サーバー接続を終了しました");
            }
            catch (Exception ex)
            {
                _logMessages.Add($"サーバー停止エラー: {ex.Message}");
            }
            
            _isConnected = false;
        }
        
        public override GH_Exposure Exposure => GH_Exposure.primary;
        
        public override Guid ComponentGuid => new Guid("9E27C8E7-B397-4CDB-91E3-2AC3F06F56BB");
        
        protected override System.Drawing.Bitmap Icon => null; // アイコンが必要な場合は実装
    }
}
