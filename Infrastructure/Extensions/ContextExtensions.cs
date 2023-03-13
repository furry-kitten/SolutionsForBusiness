using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Extensions
{
    internal static class ContextExtensions
    {
        public static void Detach<T>(this DbContext context, T entry)
        {
            if (entry == null)
            {
                return;
            }

            context.Entry(entry).State = EntityState.Detached;
        }

        public static void DetachAllEntries<T>(this DbContext context, IEnumerable<T> attachedObjects)
        {
            if (attachedObjects == null)
            {
                return;
            }

            foreach (var entry in attachedObjects)
            {
                context.Entry(entry).State = EntityState.Detached;
            }
        }

        public static void DetachAllEntries(this DbContext context)
        {
            context.ChangeTracker.Clear();
        }
    }
}
