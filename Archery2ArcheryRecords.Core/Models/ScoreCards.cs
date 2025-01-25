namespace Archery2ArcheryRecords.Core.Models;

public record ScoreCards(IEnumerable<ScoreCard> Valid, IEnumerable<string> Invalid);