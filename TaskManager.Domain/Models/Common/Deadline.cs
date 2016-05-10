using System;

namespace TaskManager.Domain.Models.Common
{
    public class Deadline
    {
        private readonly DateTime _value;

        public Deadline(DateTime value)
        {
            _value = value;
        }

        public static implicit operator string(Deadline deadline)
        {
            return deadline._value.ToShortDateString();
        }
    }
}