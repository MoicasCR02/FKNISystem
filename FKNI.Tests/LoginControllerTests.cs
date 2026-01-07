using FKNI.Application.Services.Interfaces;
using FKNI.Web.Controllers;
using FKNI.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace FKNI.Tests
{
    public class LoginControllerTests
    {
        [Fact]
        public async Task LogIn_ModeloInvalido_RetornaVistaIndex()
        {
            // Arrange
            var mockServicio = new Mock<IServiceUsuarios>();
            var mockLogger = new Mock<ILogger<LoginController>>();
            var controller = new LoginController(mockServicio.Object, mockLogger.Object);
            controller.ModelState.AddModelError("User", "Campo requerido");
            var viewModel = new ViewModelLogin { User = "", Password = "" };

            // Act
            var resultado = await controller.LogIn(viewModel);

            // Assert
            var vista = Assert.IsType<ViewResult>(resultado); 
            Assert.Equal("Index", vista.ViewName);
        }
    }
}