using ExtCore.FileStorage.Abstractions;

namespace MyTemplate.Application.Interfaces.IO;
public interface IFileManager
{
  Task SaveFileAsync(Stream fileStream, string directoryName, string fileName);
  Task UpdateFileAsync(Stream fileStream, string oldFileName, string directoryName, string newFileName);
  Task<IFileProxy> GetFile(string directoryName, string fileName);

  string GetContentType(string fileName);
}