using System;
using Serilog;
using TaskManager.Domain.Common;

namespace TaskManager.Domain.Infrastructure
{
    public class ExceptionDecorator<T> where T : Event
    {
        private readonly Action<Event> _next;
        private readonly ILogger _logger;

        /// <exception cref="ArgumentNullException"><paramref name="next"/> is <see langword="null" />.</exception>
        public ExceptionDecorator(Action<Event> next)
        {
            if (next == null) throw new ArgumentNullException("next");
            _next = next;
            _logger = Logging.Logger;
        }

        public void Handle(T notification)
        {
            try
            {
                _next(notification);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while processing event");
            }
        }
    }
}