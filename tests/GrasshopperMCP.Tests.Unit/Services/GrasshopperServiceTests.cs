using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.Extensions.Logging;
using GrasshopperMCP.Server.Services;
using System.Threading.Tasks;
using System;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace GrasshopperMCP.Tests.Unit.Services
{
    [TestClass]
    public class GrasshopperServiceTests
    {
        private Mock<IDefinitionManager> _mockDefinitionManager;
        private Mock<IComponentManager> _mockComponentManager;
        private Mock<ILogger<GrasshopperService>> _mockLogger;
        private GrasshopperService _service;
        
        [TestInitialize]
        public void Initialize()
        {
            _mockDefinitionManager = new Mock<IDefinitionManager>();
            _mockComponentManager = new Mock<IComponentManager>();
            _mockLogger = new Mock<ILogger<GrasshopperService>>();
            
            _service = new GrasshopperService(
                _mockDefinitionManager.Object,
                _mockComponentManager.Object,
                _mockLogger.Object);
        }
        
        [TestMethod]
        public async Task CreateDefinition_ShouldReturnValidId()
        {
            // Arrange
            string expectedId = Guid.NewGuid().ToString();
            _mockDefinitionManager
                .Setup(m => m.RegisterDefinition(It.IsAny<GH_Document>()))
                .Returns(expectedId);
                
            // Act
            string actualId = await _service.CreateDefinitionAsync("テスト定義");
            
            // Assert
            Assert.AreEqual(expectedId, actualId);
            _mockDefinitionManager.Verify(
                m => m.RegisterDefinition(It.IsAny<GH_Document>()),
                Times.Once);
        }
        
        [TestMethod]
        public async Task AddComponent_ShouldCallComponentManager()
        {
            // Arrange
            string definitionId = Guid.NewGuid().ToString();
            string componentType = "Grasshopper.Kernel.Components.GH_NumberSlider";
            Point3d position = new Point3d(100, 100, 0);
            string nickname = "テストスライダー";
            string expectedComponentId = Guid.NewGuid().ToString();
            
            var mockDocument = new Mock<GH_Document>();
            
            _mockDefinitionManager
                .Setup(m => m.GetDefinitionAsync(definitionId))
                .ReturnsAsync(mockDocument.Object);
                
            _mockComponentManager
                .Setup(m => m.AddComponentAsync(mockDocument.Object, componentType, position, nickname))
                .ReturnsAsync(expectedComponentId);
                
            // Act
            string actualComponentId = await _service.AddComponentAsync(definitionId, componentType, position, nickname);
            
            // Assert
            Assert.AreEqual(expectedComponentId, actualComponentId);
            _mockComponentManager.Verify(
                m => m.AddComponentAsync(mockDocument.Object, componentType, position, nickname),
                Times.Once);
        }
        
        [TestMethod]
        public async Task RunSolution_ShouldHandleErrors()
        {
            // Arrange
            string definitionId = Guid.NewGuid().ToString();
            var mockDocument = new Mock<GH_Document>();
            
            _mockDefinitionManager
                .Setup(m => m.GetDefinitionAsync(definitionId))
                .ReturnsAsync(mockDocument.Object);
                
            // Act
            var result = await _service.RunSolutionAsync(definitionId);
            
            // Assert
            Assert.IsNotNull(result);
            // 実行は呼ばれるはず
            mockDocument.Verify(m => m.NewSolution(true), Times.Once);
        }
        
        [TestMethod]
        public async Task GetDefinition_WhenNotFound_ShouldReturnNull()
        {
            // Arrange
            string definitionId = Guid.NewGuid().ToString();
            
            _mockDefinitionManager
                .Setup(m => m.GetDefinitionAsync(definitionId))
                .ReturnsAsync((GH_Document)null);
                
            // Act
            var result = await _service.GetSolutionResultsAsync(definitionId, "anyComponentId", 0);
            
            // Assert
            Assert.IsNull(result);
        }
    }
}
