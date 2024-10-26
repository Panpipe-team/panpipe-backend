using Panpipe.Controllers;

namespace PanpipeTests;

public class HelloWorldControllerTest
{
    [Fact]
    public void ReturnHelloWorld()
    {
        // Arrange
        var controller = new HelloWorldController();
        
        // Act
        var result = controller.Get();

        // Assert
        Assert.Equal("Hello, world!", result);
    }
}