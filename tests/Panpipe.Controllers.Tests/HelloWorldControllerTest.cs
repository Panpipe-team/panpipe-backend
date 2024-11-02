using Panpipe.Controllers;

namespace Panpipe.Controllers.Tests;

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
        Assert.Equal("Hello, world! V2", result);
    }
}