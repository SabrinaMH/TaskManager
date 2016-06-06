using System;
using TaskManager.Domain.Models.Common;

namespace TaskManager.Domain.Features.RegisterProject
{
    public class DoesProjectWithTitleExistQuery
    {
        public Title Title { get; private set; }

        /// <exception cref="ArgumentNullException"><paramref name="title"/> is <see langword="null" />.</exception>
        public DoesProjectWithTitleExistQuery(Title title)
        {
            if (title == null) throw new ArgumentNullException("title");
            Title = title;
        }
    }
}