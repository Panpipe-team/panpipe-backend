using Microsoft.AspNetCore.Mvc;

namespace Panpipe.Presentation.Controllers;

[ApiController]
[Route("/api/hello-world")]
public class HelloWorldController: ControllerBase {
    [HttpGet]
    public string Get() {
        return "Hello, world! V2";
    }
}
