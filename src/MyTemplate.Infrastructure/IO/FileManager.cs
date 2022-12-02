using MyTemplate.Core.Files.Interfaces;
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

  public Task<IFileProxy> GetFile(string directoryName, string fileName)
  {
    return FileProxy(directoryName, fileName);
  }

  public async Task SaveFileAsync(Stream fileStream, string directoryName, string fileName)
  {
    var path = Path.Combine(directoryName);
    var directoryProxy = _fileStorage.CreateDirectoryProxy(path);
    await directoryProxy.CreateAsync();

    var fileProxy = _fileStorage.CreateFileProxy(directoryProxy.RelativePath, fileName);

    await fileProxy.WriteStreamAsync(fileStream);
  }

  public async Task UpdateFileAsync(Stream fileStream, string oldFileName, string directoryName, string fileName)
  {
    var proxy = await FileProxy(directoryName, oldFileName);
    if (await proxy.ExistsAsync())
    {
      await proxy.DeleteAsync();
    }
    await SaveFileAsync(fileStream, directoryName, fileName);
  }

  private async Task<IFileProxy> FileProxy(string directoryName, string fileName)
  {
    var path = Path.Combine(directoryName);
    var directoryProxy = _fileStorage.CreateDirectoryProxy(path);
    if (!await directoryProxy.ExistsAsync())
    {
      await directoryProxy.CreateAsync();
    }
    return _fileStorage.CreateFileProxy(directoryProxy.RelativePath, fileName);
  }

  public string GetContentType(string fileName)
  {
    if (!_contentTypeProvider.TryGetContentType(fileName, out var contentType))
    {
      contentType = "application/octet-stream";
    }
    return contentType;
  }
}
