using Microsoft.AspNetCore.Mvc;

namespace Panpipe.Controllers;


[ApiController]
[Route("/api/hello-world")]
public class HelloWorldController: ControllerBase {
    [HttpGet]
    public string Get() {
        return "Hello, world! V2";
    }
    
    
    
}
