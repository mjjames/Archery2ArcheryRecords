namespace Archery2ArcheryRecords.Tests;

public static class AsyncEnumerableHelpers
{
    public static async IAsyncEnumerable<T> AsAsyncEnumerable<T>(this IEnumerable<T> input)
    {
        foreach(var value in input)
        {
            yield return value;
        }
    }
}