namespace Archery2ArcheryRecords.Core.Models;

public record ScoreCard(string Archer, DateTime Date, string AgeGroup, string ClubName, string RoundName, string BowType, IEnumerable<Round> Rounds);
public record Round(int RoundNumber, IEnumerable<End> Ends)
{
    public int Total => Ends.Sum(e => e.Score.Sum());
    public int Hits => Ends.Sum(e => e.Score.Count(s => s > 0));
    public int Golds => Ends.Sum(e => e.Score.Count(s => s == 10));
}
public record End(int EndNumber, IEnumerable<int> Score);
