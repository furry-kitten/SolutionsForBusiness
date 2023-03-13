namespace PresentationLayer.Utils
{
    public class Condition
    {
        public Condition(string columnName, object value, Predicate<(object entityValue, object conditionValue)> compare)
        {
            ColumnName = columnName;
            Value = value;
            Compare = compare;
        }

        public string ColumnName { get; set; }
        public object Value { get; set; }
        public ConditionType Type { get; set; } = ConditionType.Or;
        public Condition? AddCondition { get; set; }
        public Predicate<(object entityValue, object conditionValue)> Compare { get; set; }
    }
}