namespace MyTemplate.Core.Files.Models;

public class FileQueryResponse
{
  public FileQueryResponse(string contentType, Stream stream)
  {
    ContentType = contentType;
    Stream = stream;
  }
  public string ContentType { get; private set; }

  public Stream Stream { get; private set; }
}
