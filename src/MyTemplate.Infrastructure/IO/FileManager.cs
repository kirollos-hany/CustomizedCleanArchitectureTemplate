using MyTemplate.Core.Files.Interfaces;
using MyTemplate.Core.Files.Models;
using ExtCore.FileStorage.Abstractions;

namespace MyTemplate.Infrastructure.IO;

public class FileManager : IFileManager
{
  private readonly IFileStorage _fileStorage;
  private readonly IContentTypeProvider _contentTypeProvider;
  public FileManager(IFileStorage fileStorage, IContentTypeProvider contentTypeProvider)
  {
    _fileStorage = fileStorage;
    _contentTypeProvider = contentTypeProvider;
  }

  public async Task<FileQueryResponse> GetFile(string fileName)
  {
    var fileProxy = await FileProxy(fileName);
    return new FileQueryResponse(GetContentType(fileName), await fileProxy.ReadStreamAsync());
  }

  public async Task<string> SaveFileAsync(Stream fileStream)
  {
    var directoryProxy = _fileStorage.CreateDirectoryProxy($"\\");
    await directoryProxy.CreateAsync();

    var fileName = Guid.NewGuid().ToString();

    var fileProxy = _fileStorage.CreateFileProxy(directoryProxy.RelativePath, fileName);

    await fileProxy.WriteStreamAsync(fileStream);
    return fileName;
  }

  public async Task<string> UpdateFileAsync(Stream fileStream, string oldFileName)
  {
    var a = await FileProxy(oldFileName);
    if (await a.ExistsAsync())
    {
      await a.DeleteAsync();
    }
    return await SaveFileAsync(fileStream);
  }

  private async Task<IFileProxy> FileProxy(string fileName)
  {
    var directoryProxy = _fileStorage.CreateDirectoryProxy($"\\");
    if (!await directoryProxy.ExistsAsync())
    {
      await directoryProxy.CreateAsync();
    }
    return _fileStorage.CreateFileProxy(directoryProxy.RelativePath, fileName);
  }

  private string GetContentType(string fileName)
  {
    if (!_contentTypeProvider.TryGetContentType(fileName, out var contentType))
    {
      contentType = "application/octet-stream";
    }
    return contentType;
  }
}
