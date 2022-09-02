namespace MyTemplate.Core.Files.Models;

public class FileModel
{
  public FileModel(string name, long length, Stream stream)
  {
    Name = name;
    Length = length;
    Stream = stream;
  }
  public string Name { get; private set; } 
  public Stream Stream { get; private set; }
  public long Length { get; private set; }
}
