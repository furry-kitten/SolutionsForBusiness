using System.Linq.Expressions;

namespace Infrastructure.Utils
{
    public class Condition<TType>
    {
        public Condition(string columnName, TType value, Predicate<(TType? entityValue, TType? conditionValue)> compare)
        {
            ColumnName = columnName;
            Value = value;
            Compare = x => compare.Invoke(x);
        }

        public string ColumnName { get; set; }
        public TType Value { get; set; }
        //public ConditionType Type { get; set; } = ConditionType.Or;
        public Condition<TType>? AddCondition { get; set; }
        public Predicate<(TType? entityValue, TType? conditionValue)> Compare { get; set; }
    }
}