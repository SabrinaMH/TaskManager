using System;
using TaskManager.Domain.Features.ChangeDeadlineOnTask;

namespace TaskManager.Domain.Models.Task
{
    public class TaskDeadline
    {
        private readonly DateTime _value;

        public TaskDeadline(DateTime value)
        {
            _value = value;
        }

        public static implicit operator string(TaskDeadline deadline)
        {
            return deadline.ToString();
        }

        public override string ToString()
        {
            return _value.ToString();
        }

        public bool InThePast
        {
            get { return _value < DateTime.Now; }
        }
    }
}