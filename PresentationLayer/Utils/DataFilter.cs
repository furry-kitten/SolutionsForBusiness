namespace PresentationLayer.Utils
{
    public class DataFilter<TEntity>
        where TEntity : class
    {
        private readonly Type _type = typeof(TEntity);

        public List<Condition> Conditions { get; } = new();

        internal IEnumerable<TEntity> SetFilter(IEnumerable<TEntity> entities) =>
            entities.Where(Filter);

        internal bool Filter(TEntity entity)
        {
            return Conditions.Aggregate(false,
                (current, condition) => current |
                                        (condition.Type is ConditionType.And ?
                                            GetAllAndConditions(condition, entity) :
                                            condition.Compare(GetValueTuple(entity, condition))));
        }

        private (object, object) GetValueTuple(TEntity entity, Condition condition) =>
            new(_type.GetProperty(condition.ColumnName)!.GetValue(entity)!, condition.Value);

        private bool GetAllAndConditions(Condition condition,
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