using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Persistence.Dapper
{
    internal class QueryFiller
    {
        /// <summary>
        ///     Fills query with the values that ORM uses
        /// </summary>
        /// <param name="command">Command run by ORM</param>
        /// <returns>Command run by ORM with query filled with explicit values</returns>
        public string Fill(string command)
        {
            var columnValue = new Dictionary<string, string>();
            var statements = command.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
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
                command = command.Replace($"@{column}", columnValue[column]);
            }

            var i = command.IndexOf("CommandText: ");
            command = command.Substring(0, i + 13) + Environment.NewLine + command.Substring(i + 13);

            return command;
        }
    }
}