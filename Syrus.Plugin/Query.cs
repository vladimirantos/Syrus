using System;
using System.Linq;

namespace Syrus.Plugin
{
    public class Query
    {
        public string Original { get; set; }
        public string Command { get; set; }
        public string Arguments { get; set; }
        public string[] ArgumentsArray { get; set; }
        public bool HasCommand => !string.IsNullOrEmpty(Command);
        public bool HasArguments => !string.IsNullOrEmpty(Arguments);

        public Query(string original, string command, string arguments, string[] argumentsArray) 
            => (Original, Command, Arguments, ArgumentsArray) = (original, command, arguments, argumentsArray);

        public static Query FromString(string query)
        {
            var parts = query.ToLower().Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            var arguments = parts.Skip(1);
            return new Query(query, parts.First(), string.Join(' ', arguments), arguments.ToArray());
        }

        public override string ToString() => Original;
    }
}
