using System;

namespace TaskManager.Domain.Models.Project
{
    public class ProjectDeadline
    {
        private readonly DateTime _value;

        public ProjectDeadline(DateTime value)
        {
            _value = value;
        }

        public static implicit operator string(ProjectDeadline deadline)
        {
            return deadline.ToString();
        }

        public override string ToString()
        {
            return _value.ToShortDateString();
        }
    }
}