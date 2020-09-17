using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Domain.Enums;
using Domain.Exceptions;

namespace Domain.Entities
{
    public abstract class Entity
    {
        public Guid Id { get; set; }
        protected string EntityName => $"{GetType().Name}";

        private string PropertyName
        {
            get
            {
                var setMethodName = new StackTrace().GetFrame(2).GetMethod().Name; //0: current method, 1: previous method
                return setMethodName.Substring(3, setMethodName.Length - 3);
            }
        }

        protected void ValidateRequiredString(string value, int[] lenghtRange)
        {
            if (string.IsNullOrEmpty(value)) throw new BusinessRuleException($"{EntityName}: {PropertyName} can not be null or empty");

            if (lenghtRange.Length != 1 && lenghtRange.Length != 2)
                throw new BusinessRuleException($"{EntityName}: {PropertyName} length is out of valid range");

            if (lenghtRange.Length == 1 && value.Length != lenghtRange.First())
                throw new BusinessRuleException($"{EntityName}: {PropertyName} length must be equals than {lenghtRange.First()}");

            if (lenghtRange.Length == 2 && value.Length < lenghtRange.First())
                throw new BusinessRuleException($"{EntityName}: {PropertyName} length must be greater than {lenghtRange.First()}");

            if (lenghtRange.Length == 2 && value.Length > lenghtRange.Last())
                throw new BusinessRuleException($"{EntityName}: {PropertyName} length must be less than {lenghtRange.Last()}");
        }

        protected void ValidateRequiredEnum(Enum value, Type enumType)
        {
            if (!Enum.IsDefined(enumType, value)) throw new BusinessRuleException($"{EntityName}: {PropertyName} type is invalid");
        }        
        
        protected void ValidateRequiredInt(int value, ComparisonOperator lowerLimitOp, int lowerLimit, ComparisonOperator upperLimitOp, int upperLimit)
        {
            ValidateRequiredDecimal(value, lowerLimitOp, lowerLimit, upperLimitOp, upperLimit);
        }

        protected void ValidateRequiredDecimal(decimal value, ComparisonOperator? lowerLimitOp = null, decimal? lowerLimit = null, ComparisonOperator? upperLimitOp = null, decimal? upperLimit = null)
        {
            if (lowerLimitOp.HasValue && upperLimitOp.HasValue && lowerLimit == upperLimit && lowerLimitOp == ComparisonOperator.Ge && upperLimitOp == ComparisonOperator.Le && lowerLimit != value)//takes lowerLimit as comparator (use lowerLimit is the same)
                throw new BusinessRuleException($"{EntityName}: {PropertyName} must be equal than {lowerLimit}");

            if (lowerLimitOp.HasValue && upperLimitOp.HasValue && lowerLimit == upperLimit && lowerLimitOp == ComparisonOperator.Gt && upperLimitOp == ComparisonOperator.Lt && lowerLimit == value)//takes lowerLimit as comparator (use lowerLimit is the same)
                throw new BusinessRuleException($"{EntityName}: {PropertyName} must be not equal than {lowerLimit}");

            if (lowerLimitOp.HasValue && lowerLimitOp == ComparisonOperator.Ge && value < lowerLimit)
                throw new BusinessRuleException($"{EntityName}: {PropertyName} must be greater or equal than {lowerLimit}");

            if (lowerLimitOp.HasValue && lowerLimitOp == ComparisonOperator.Gt && value <= lowerLimit)
                throw new BusinessRuleException($"{EntityName}: {PropertyName} must be greater than {lowerLimit}");

            if (upperLimitOp.HasValue && upperLimitOp == ComparisonOperator.Le && value > upperLimit)
                throw new BusinessRuleException($"{EntityName}: {PropertyName} must be less or equal than {upperLimit}");

            if (upperLimitOp.HasValue && upperLimitOp == ComparisonOperator.Lt && value >= upperLimit)
                throw new BusinessRuleException($"{EntityName}: {PropertyName} must be less than {upperLimit}");
        }
    }
}