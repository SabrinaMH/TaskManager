﻿using System;
using TaskManager.Domain.Common;

namespace TaskManager.Domain.Features.RegisterProject
{
    public class RegisterProject : Command 
    {
        public string Title { get; private set; }
        public DateTime? Deadline { get; private set; }

        public RegisterProject(string title, DateTime? deadline)
        {
            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("title cannot be null or empty", "title");
            Title = title;
            Deadline = deadline;
        }
    }
}