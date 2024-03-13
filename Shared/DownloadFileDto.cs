namespace Shared;
public record DownloadFileDto(MemoryStream memoryStream, string contentType, string fileName);