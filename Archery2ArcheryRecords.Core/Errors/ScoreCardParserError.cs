using FluentResults;

namespace Archery2ArcheryRecords.Core.Errors;

public class ScoreCardParserError(string message) : Error(message);