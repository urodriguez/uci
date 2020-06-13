using System;
using System.Diagnostics;

namespace Domain.Entities
{
    public abstract class Entity
    {
        public Guid Id { get; set; }
        protected string EntityName => $"{GetType().BaseType.Name}";

        protected string PropertyName
        {
            get
            {
                var setMethodName = new StackTrace().GetFrame(1).GetMethod().Name; //0: current method, 1: previous method
                return setMethodName.Substring(3, setMethodName.Length - 3);
            }
        }
    }
}