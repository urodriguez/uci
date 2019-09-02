using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Crosscutting.Logging
{
    internal class QueryFormatter
    {
        public string Format(string query)
        {
            var columnValue = new Dictionary<string, string>();
            var statements = query.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (var statement in statements)
            {
                if (statement.Contains("Name: "))
                {
                    var column = statement.Split(',').First().Substring(6);
                    var value = statement.Split(',').Last().Substring(8);
                    columnValue.Add(column, value);
                }
            }

            foreach (var column in columnValue.Keys)
            {
                query = query.Replace($"@{column}", columnValue[column]);
            }


            var i = query.IndexOf("CommandText: ");
            query = query.Substring(0, i + 13) + Environment.NewLine + query.Substring(i + 13);

            return query;
        }
    }
}