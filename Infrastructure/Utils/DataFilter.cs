using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Utils
{
    public class DataFilter<TEntity, TType>
        where TEntity : class
    {
        private readonly Type _type = typeof(TEntity);

        public List<Condition<TType>> Conditions { get; } = new();

        public bool Filter(TEntity entity)
        {
            return Conditions.Aggregate(false, (current, condition) => current | GetAllAndConditions(condition, entity));
        }

        private (TType?, TType?) GetValueTuple(TEntity entity, Condition<TType> condition) =>
        //new(EF.Property<TType>(entity, condition.ColumnName)!, condition.Value);
        new((TType?)_type.GetProperty(condition.ColumnName)!.GetValue(entity)!, condition.Value);

        private bool GetAllAndConditions(Condition<TType> condition,
            TEntity entity,
            bool previousResult = true)
        {
            var valueTuple = GetValueTuple(entity, condition);
            if (condition.AddCondition is null)
            {
                return previousResult && condition.Compare.Invoke(valueTuple);
            }

            var result = previousResult & condition.Compare.Invoke(valueTuple);
            return result && GetAllAndConditions(condition.AddCondition, entity, result);
        }
    }
}