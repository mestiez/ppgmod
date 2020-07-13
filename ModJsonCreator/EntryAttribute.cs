using System;

namespace ppgmod
{
    public class EntryAttribute : Attribute
    {
        public EntryAttribute(string information, string defaultValue)
        {
            Information = information;
            DefaultValue = defaultValue;
        }

        public string Information { get; }
        public string DefaultValue { get; }
    }
}
