using Archery2ArcheryRecords.Core.Errors;
using Archery2ArcheryRecords.Core.Models;
using FluentResults;

namespace Archery2ArcheryRecords.Core.Services;

public class ScoreCardRetrievalService : IScoreCardRetrievalService
{
    public async IAsyncEnumerable<Result<RawPDF>> LoadFiles(IEnumerable<FileBase> files)
    {
        foreach (var file in files)
        {
            yield return await LoadFile(file);
        }
    }

    private async Task<Result<RawPDF>> LoadFile(FileBase file)
    {
        if(file.ContentType != "application/pdf")
        {
            return Result.Fail<RawPDF>(new InvalidFileType(file.ContentType, file.FileName));
        }
        try
        {
            return Result.Ok(new RawPDF(file.FileName, await file.OpenReadAsync()));
        }
        catch (Exception ex)
        {
            return Result.Fail<RawPDF>(new FileLoadError(ex.Message, file.FileName));
        }
    }
}

public interface IScoreCardRetrievalService
{
    IAsyncEnumerable<Result<RawPDF>> LoadFiles(IEnumerable<FileBase> files);
}