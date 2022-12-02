namespace MyTemplate.Application.ApiModels;

public class ActionResponse
{
  public IEnumerable<string> Messages { get; private set; } = Enumerable.Empty<string>();

  public ActionResponse(IEnumerable<string> messages)
  {
    Messages = messages;
  }
}