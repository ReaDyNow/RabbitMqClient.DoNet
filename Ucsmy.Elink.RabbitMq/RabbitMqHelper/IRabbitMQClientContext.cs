using RabbitMQ.Client;
using System;

namespace Ucsmy.Elink.RabbitMq.SDK
{
    /// <summary>
    /// IRabbitMQClientContext
    /// </summary>
    public interface IRabbitMQClientContext : IDisposable
    {
        /// <summary>
        /// Channel
        /// </summary>
        IModel Channel { get; }
    }

}
