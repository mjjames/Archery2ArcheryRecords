using Archery2ArcheryRecords.Core.Models;

namespace Archery2ArcheryRecords.Tests.Models;

public class ScoreCardTests
{
    [Test]
    public async ValueTask Round_Total_ShouldReturnCorrectSum()
    {
        // Arrange
        var ends = new List<End>
        {
            new End(1, new List<int> { 10, 9, 8 }),
            new End(2, new List<int> { 7, 6, 5 })
        };
        var round = new Round(1, ends);

        // Act
        var total = round.Total;

        // Assert
        await Assert.That(total).IsEqualTo(45);
    }

    [Test]
    public async ValueTask Round_Hits_ShouldReturnCorrectCount()
    {
        // Arrange
        var ends = new List<End>
        {
            new End(1, new List<int> { 10, 9, 0 }),
            new End(2, new List<int> { 7, 6, 0 })
        };
        var round = new Round(1, ends);

        // Act
        var hits = round.Hits;

        // Assert
        await Assert.That(hits).IsEqualTo(4);
    }

    [Test]
    public async ValueTask Round_Golds_ShouldReturnCorrectCount()
    {
        // Arrange
        var ends = new List<End>
        {
            new End(1, new List<int> { 10, 9, 10 }),
            new End(2, new List<int> { 10, 6, 5 })
        };
        var round = new Round(1, ends);

        // Act
        var golds = round.Golds;

        // Assert
        await Assert.That(golds).IsEqualTo(3);
    }
}