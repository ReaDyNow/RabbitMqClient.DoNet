using RabbitMQ.Client;
using System.Linq;

namespace Ucsmy.Elink.RabbitMq.SDK
{
    /// <summary>
    /// CommonWithLockBuilder  channel只允许单线程访问
    /// </summary>
    public class CommonWithLockBuilder : IRabbitMQClientContext, IRabbitMQClientContextBuilder
    {
        /// <summary>
        /// _lockObj
        /// </summary>

        private object _lockObj = new object();

        /// <summary>
        /// _channel
        /// </summary>
        private IModel _channel;

        /// <summary>
        /// Channel
        /// </summary>
        public IModel Channel
        {
            get
            {
                lock (_lockObj)
                {
                    return this._channel;
                }
            }
            set
            {
                this._channel = value;
            }
        }

        /// <summary>
        /// _conn
        /// </summary>

        private IConnection _conn;

        /// <summary>
        /// Conn
        /// </summary>

        public IConnection Conn { get; set; }


        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            if (this.Channel != null)
            {
                if (this.Channel.IsOpen)
                {
                    this.Channel.Close();
                }
                this.Channel.Dispose();
            }
            if (this.Conn != null)
            {
                if (this.Conn.IsOpen)
                {
                    this.Conn.Close();
                }
                this.Conn.Dispose();
            }
        }

        /// <summary>
        /// Build
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public IRabbitMQClientContext Build(IMqConnectionConfig config)
        {
            CreateConnection(config);
            CreateChannel(this.Conn);
            this.Conn.ConnectionShutdown += this.Connection_ConnectionShutdown;
            this.Channel.ModelShutdown += this.Channel_ModelShutdown;
            return this;
        }

        /// <summary>
        /// CreateConnection
        /// </summary>
        /// <param name="config"></param>
        private void CreateConnection(IMqConnectionConfig config)
        {
            ConnectionFactory factory = new ConnectionFactory
            {
                HostName = config.HostName,
                UserName = config.UserName,
                Password = config.Password,
                VirtualHost = config.VirtualHost,
                RequestedHeartbeat = config.RequestedHeartbeat,
                AutomaticRecoveryEnabled = config.AutomaticRecoveryEnabled
            };
            //有配置
            if (config.Port != 0)
            {
                factory.Port = config.Port;
            }
            this.Conn = factory.CreateConnection();
        }

        /// <summary>
        ///CreateChannel
        /// </summary>
        /// <param name="conn"></param>
        /// <returns></returns>
        private void CreateChannel(IConnection conn)
        {
            var channel = conn.CreateModel();
            //conn.AutoClose = true;
            this.Channel = channel;
        }

        /// <summary>
        /// Channel_ModelShutdown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Channel_ModelShutdown(object sender, ShutdownEventArgs e)
        {
            if (e.ReplyCode != 200)
            {
                Ucs.Common.Logging.LogInfo(e.ReplyText, Ucs.Common.LogType.Info);
            }
        }

        /// <summary>
        /// Connection_ConnectionShutdown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Connection_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            if (e.ReplyCode != 200)
            {
                var conn = (IConnection)sender;

                Ucs.Common.Logging.LogInfo(e.ReplyText, Ucs.Common.LogType.Info);
                conn.ShutdownReport.Select(
                    x =>
                    {
                        Ucs.Common.Logging.LogInfo(x.Description, Ucs.Common.LogType.Info);
                        return x;
                    });
            }
        }
    }
}
