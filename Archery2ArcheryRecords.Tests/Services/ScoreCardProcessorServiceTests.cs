
using Archery2ArcheryRecords.Core.Errors;
using Archery2ArcheryRecords.Core.Models;
using Archery2ArcheryRecords.Core.Services;
using Moq;
using FluentResults;

namespace Archery2ArcheryRecords.Tests.Services;

public class ScoreCardProcessorServiceTest
{
    private readonly Mock<IScoreCardRetrievalService> _mockScoreCardRetrievalService;
    private readonly Mock<IScoreCardParserService> _mockScoreCardParserService;
    private readonly ScoreCardProcessorService _scoreCardProcessorService;
    private readonly ScoreCard _unknownScoreCard = new ScoreCard("Unknown", DateTime.MinValue, "Unknown", "Unknown", "Unknown", "Unknown", []);

    public ScoreCardProcessorServiceTest()
    {
        _mockScoreCardRetrievalService = new Mock<IScoreCardRetrievalService>();
        _mockScoreCardParserService = new Mock<IScoreCardParserService>();
        _scoreCardProcessorService = new ScoreCardProcessorService(_mockScoreCardRetrievalService.Object, _mockScoreCardParserService.Object);
    }

    [Test]
    public async ValueTask ProcessScoreCardsAsync_AllFilesValid_ReturnsScoreCards()
    {
        // Arrange
        var files = new List<FileResult> { new("file1.pdf"), new("file2.pdf") };
        var scoreCardFiles = files.Select(f => Result.Ok(new RawPDF(f.FileName, new MemoryStream()))).AsAsyncEnumerable();
        
        _mockScoreCardRetrievalService.Setup(s => s.LoadFiles(files)).Returns(scoreCardFiles);
        _mockScoreCardParserService.Setup(s => s.ParseScoreCardAsync(It.IsAny<Stream>())).ReturnsAsync(Result.Ok(_unknownScoreCard));

        // Act
        var result = await _scoreCardProcessorService.ProcessScoreCardsAsync(files);

        // Assert
        await Assert.That(result.Valid.Count).IsEqualTo(2);
        await Assert.That(result.Invalid.Count).IsEqualTo(0);
    }

    [Test]
    public async ValueTask ProcessScoreCardsAsync_SomeFilesInvalid_ReturnsInvalidPaths()
    {
        // Arrange
        var files = new List<FileResult> { new FileResult("file1.pdf"), new FileResult("file2.pdf") };
        var scoreCardFiles = new List<Result<RawPDF>>
            {
                Result.Fail<RawPDF>(new FileLoadError("File access error","file1.pdf")),
                Result.Ok(new RawPDF("file2.pdf", new MemoryStream()))
            }.AsAsyncEnumerable();

        _mockScoreCardRetrievalService.Setup(s => s.LoadFiles(files)).Returns(scoreCardFiles);
        _mockScoreCardParserService.Setup(s => s.ParseScoreCardAsync(It.IsAny<Stream>())).ReturnsAsync(Result.Ok(_unknownScoreCard));

        // Act
        var result = await _scoreCardProcessorService.ProcessScoreCardsAsync(files);

        // Assert
        await Assert.That(result.Valid.Count).IsEqualTo(1);
        await Assert.That(result.Invalid.Count).IsEqualTo(1);
        await Assert.That(result.Invalid.First()).IsEqualTo("file1.pdf");
    }

    [Test]
    public async ValueTask ProcessScoreCardsAsync_AllFilesInvalid_ReturnsOnlyInvalidPaths()
    {
        // Arrange
        var files = new List<FileResult> { new("file1.jpg"), new("file2.pdf") };
        var scoreCardFiles = new List<Result<RawPDF>>
            {
                Result.Fail<RawPDF>(new InvalidFileType("image/jpeg","file1.abc")),
                Result.Fail<RawPDF>(new FileLoadError("Invalid location","file2.pdf"))
            };

        _mockScoreCardRetrievalService.Setup(s => s.LoadFiles(files)).Returns(scoreCardFiles.AsAsyncEnumerable());

        // Act
        var result = await _scoreCardProcessorService.ProcessScoreCardsAsync(files);

        // Assert
        await Assert.That(result.Valid.Count).IsEqualTo(0);
        await Assert.That(result.Invalid.Count).IsEqualTo(2);
        await Assert.That(result.Invalid).IsEquivalentTo(new List<string>() { "file1.abc", "file2.pdf" });
    }
}

