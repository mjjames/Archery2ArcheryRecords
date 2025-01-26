namespace Archery2ArcheryRecords.Core.Models;

public record ScoreCards(IReadOnlyCollection<ScoreCard> Valid, IReadOnlyCollection<string> Invalid);