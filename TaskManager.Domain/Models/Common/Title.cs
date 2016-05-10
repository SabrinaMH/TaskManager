using System;

namespace TaskManager.Domain.Models.Common
{
    public class Title
    {
        private readonly string _value;

        public Title(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("value cannot be null or empty", "value");
            _value = value;
        }

        public static implicit operator string(Title id)
        {
            return id._value;
        }
    }
}