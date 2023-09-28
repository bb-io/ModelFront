namespace Apps.ModelFront.Extensions;

public static class EnumerableExtensions
{
    public static T[][] ChunkBy<T>(this IEnumerable<T> source, int chunkSize)
    {
        return source
            .Select((x, i) => new { Index = i, Value = x })
            .GroupBy(x => x.Index / chunkSize)
            .Select(x => x.Select(v => v.Value).ToArray())
            .ToArray();
    }
}