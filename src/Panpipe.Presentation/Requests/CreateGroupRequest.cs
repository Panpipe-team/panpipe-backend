namespace Panpipe.Presentation.Requests;

public class CreateGroupRequest(string name): BaseRequest
{
    public string Name { get; } = name;
}
