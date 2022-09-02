using MyTemplate.Core.Files.Models;

namespace MyTemplate.Core.Files.Interfaces;

public interface IFileManager
{
  Task<string> SaveFileAsync(Stream fileStream);
  Task<string> UpdateFileAsync(Stream fileStream, string oldFileName);
  Task<FileQueryResponse> GetFile(string fileName);
}
