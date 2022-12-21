namespace AdventOfCode.Solutions.Utils;

public static class CollectionUtils
{
    public static IEnumerable<T> IntersectAll<T>(this IEnumerable<IEnumerable<T>> input)
        => input.Aggregate(input.First(), (intersector, next) => intersector.Intersect(next));

    public static string JoinAsStrings<T>(this IEnumerable<T> items, string delimiter = "") =>
        string.Join(delimiter, items);

    public static IEnumerable<IEnumerable<T>> Permutations<T>(this IEnumerable<T> values) => values.Count() == 1
        ? new[] { values }
        : values.SelectMany(v =>
            Permutations(values.Where(x => x?.Equals(v) == false)), (v, p) => p.Prepend(v));

    public static void Deconstruct<T>(this IList<T> list, out T? first, out IList<T> rest)
    {
        first = list.Count > 0 ? list[0] : default;
        rest = list.Skip(1).ToList();
    }

    public static void Deconstruct<T>(this IList<T> list, out T? first, out T? second, out IList<T> rest)
    {
        first = list.Count > 0 ? list[0] : default;
        second = list.Count > 1 ? list[1] : default;
        rest = list.Skip(2).ToList();
    }

    public static IList<T> BubbleSort<T>(this IList<T> list, Func<T, T, int> compare)
    {
        T temp;

        for (int j = 0; j <= list.Count - 2; j++)
        {
            for (int i = 0; i <= list.Count - 2; i++)
            {
                var comparison = compare(list[i], list[i + 1]);

                if (comparison > 0)
                {
                    temp = list[i + 1];
                    list[i + 1] = list[i];
                    list[i] = temp;
                }
            }
        }

        return list;
    }
}
