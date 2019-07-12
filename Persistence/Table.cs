using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Domain.Contracts.Aggregates;

namespace Persistence
{
    internal class Table<TAggregateRoot> where TAggregateRoot : IAggregateRoot
    {
        public string Name { get; set; }
        public IEnumerable<string> Columns { get; set; }

        public Table()
        {
            Name = typeof(TAggregateRoot).Name;
            Columns = GetColumns();
        }

        public string GetColumnsJoined(string separator = ", ")
        {
            return string.Join(separator, Columns);
        }

        public string GetColumnParameters(string separator = ", ")
        {
            return string.Join(separator, Columns.Select(c => "@" + c));
        }

        public string GetColumnsJoinedWithParameters(string separator = ", ")
        {
            return string.Join(separator, Columns.Select(c => $"{c} = @{c}"));
        }

        private IEnumerable<string> GetColumns()
        {
            return typeof(TAggregateRoot)
                .GetProperties()
                .Where(e => !e.PropertyType.GetTypeInfo().IsGenericType)
                .Select(e => e.Name);
        }
    }
}
