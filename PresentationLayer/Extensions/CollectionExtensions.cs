namespace PresentationLayer.Extensions
{
    public static class CollectionExtensions
    {
        public static IEnumerable<IEnumerable<T>> Paginate<T>(this IEnumerable<T> source, int count = 20)
        {
            return source.Select((value, index) => new
                         {
                             Index = index,
                             Value = value
                         })
                         .GroupBy(model => model.Index / count)
                         .Select(x => x.Select(y => y.Value).ToList())
                         .ToList();
        }
    }
}