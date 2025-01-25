using Archery2ArcheryRecords.Core.Errors;
using Archery2ArcheryRecords.Core.Models;

namespace Archery2ArcheryRecords.Core.Services;

public class ScoreCardProcessorService(IScoreCardRetrievalService scoreCardRetrievalService, IScoreCardParserService scoreCardParserService)
{
    public async Task<ScoreCards> ProcessScoreCardsAsync(IEnumerable<FileResult> files)
    {
        var scoreCards = new List<ScoreCard>();
        var invalidPaths = new List<string>();
        await foreach (var scoreCardFile in scoreCardRetrievalService.LoadFiles(files))
        {
            if (scoreCardFile.IsFailed)
            {
                invalidPaths.AddRange(scoreCardFile.Errors.OfType<FileError>().Select(fileError => fileError.FileName));
                continue;
            }

            await using var pdf = scoreCardFile.Value;
            var scoreCard = await scoreCardParserService.ParseScoreCardAsync(pdf.FileStream);
            if (scoreCard.IsFailed)
            {
                invalidPaths.AddRange(pdf.FileName);
            }
            scoreCards.Add(scoreCard.Value);
        }
        return new ScoreCards(scoreCards, invalidPaths);
    }
}