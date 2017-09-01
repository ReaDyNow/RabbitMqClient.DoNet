using RabbitMQ.Client;
using System;
using System.Text;

namespace Ucsmy.Elink.RabbitMq.SDK
{
    /// <summary>
    /// 消息对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BasicResult<T> : BasicGetResult where T : class
    {
        /// <summary>
        /// AckHandler
        /// </summary>
        internal Action<ulong, bool> AckHandler;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="basicGetResult"></param>
        public BasicResult(BasicGetResult basicGetResult)
            : base(basicGetResult.DeliveryTag, basicGetResult.Redelivered, basicGetResult.Exchange, basicGetResult.RoutingKey, basicGetResult.MessageCount, basicGetResult.BasicProperties, basicGetResult.Body)
        {
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="basicGetResult"></param>
        public BasicResult(ulong deliveryTag, bool redelivered, string exchange, string routingKey, uint messageCount, IBasicProperties basicProperties, byte[] body)
    : base(deliveryTag, redelivered, exchange, routingKey, messageCount, basicProperties, body)
        {
        }

        /// <summary>
        /// 消息内容
        /// </summary>
        private T _content;

        /// <summary>
        /// 消息内容
        /// </summary>
        public T Content 
        {
            get
            {
                if (_content == null && this.Body != null)
                {
                    var t = Encoding.UTF8.GetString(this.Body);

                    if (typeof(T).Name == "String" || typeof(T).Name == "string")
                    {
                        this._content = t as T;
                    }
                    else
                    {
                        this._content = Ucs.Common.JsonUtils.Deserialize<T>(t);
                    }
                }
                return _content;
            }
        }

        /// <summary>
        /// Ack
        /// </summary>
        public void Ack()
        {
            if (this.AckHandler != null)
            {
                this.AckHandler(this.DeliveryTag, false);
            }
        }
    }
}
