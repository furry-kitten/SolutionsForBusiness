using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.DataBase.Models;
using Infrastructure.Utils;

namespace Infrastructure.Extensions
{
    public static class ConditionExtension
    {
        public static Condition<DateTime> ChangeDates(this Condition<DateTime> condition, DateTime lastDate, DateTime firstDate)
        {
            return new Condition<DateTime>(condition.ColumnName, lastDate.Date, CompareLast)
            {
                AddCondition =
                    new Condition<DateTime>(condition.ColumnName, firstDate.Date, CompareFirst)
            };
        }

        private static bool CompareLast((DateTime entityValue, DateTime conditionValue) tuple)
        {
            return tuple.entityValue <= tuple.conditionValue;
        }

        private static bool CompareFirst((DateTime entityValue, DateTime conditionValue) tuple)
        {
            return tuple.entityValue > tuple.conditionValue;
        }
    }
}
