using FluentResults;

namespace Archery2ArcheryRecords.Core.Errors;

public class InvalidFileType(string contentType, string fileName) : FileError($"Invalid file type: {contentType}", fileName)
{
}

public class FileLoadError(string message, string fileName) : FileError(message, fileName)
{
}

public abstract class FileError(string message, string fileName) : Error(message)
{
    public string FileName { get; } = fileName;
}