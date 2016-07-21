using System;
using Newtonsoft.Json;
using Serilog;

namespace TaskManager.Domain.Infrastructure
{
    public class ExceptionDecorator<T>
    {
        private readonly Action<T> _next;
        private readonly ILogger _logger;

        /// <exception cref="ArgumentNullException"><paramref name="next"/> is <see langword="null" />.</exception>
        public ExceptionDecorator(Action<T> next)
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
                var notificationAsJson = JsonConvert.SerializeObject(notification);
                _logger.ForContext("notification", notificationAsJson)
                    .Error(ex, "Error occurred while processing message");
            }
        }
    }
}