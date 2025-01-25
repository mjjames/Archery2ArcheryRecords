using System.Text;
using Archery2ArcheryRecords.Core.Errors;
using Archery2ArcheryRecords.Core.Models;
using FluentResults;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;

namespace Archery2ArcheryRecords.Core.Services;
public interface IScoreCardParserService
{
    Task<Result<ScoreCard>> ParseScoreCardAsync(Stream stream);
}

public class ScoreCardParserService : IScoreCardParserService
{
    public async Task<Result<ScoreCard>> ParseScoreCardAsync(Stream stream)
    {
        using var reader = new PdfReader(stream);
        try
        {
            var pdfDocument = new PdfDocument(reader);
            var pdfText = new StringBuilder();
            for (var i = 1; i <= pdfDocument.GetNumberOfPages(); i++)
            {
                var page = pdfDocument.GetPage(i);
                pdfText.Append(PdfTextExtractor.GetTextFromPage(page));
            }
            return Result.Ok(new ScoreCard("Unknown", DateTime.MinValue, "Unknown", "Unknown", "Unknown", "Unknown", [] ));
        }
        catch (Exception ex)
        {
            //todo handle different types of parser errors
            return Result.Fail(new ScoreCardParserError(ex.Message));
        }
        finally
        {
            await stream.DisposeAsync();
        }
    }
}