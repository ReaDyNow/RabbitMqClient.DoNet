using RabbitMQ.Client.Events;
using System;
using System.Text;
using Ucs.Common;
using RabbitMQ.Client;

namespace Ucsmy.Elink.RabbitMq.SDK
{
    /// <summary>
    /// 消费者
    /// </summary>
    public class MqConsumer : IDisposable
    {
        /// <summary>
        /// 队列名称
        /// </summary>
        protected string QueueName { get; set; }

        /// <summary>
        /// 生产者唯一标识
        /// </summary>
        public string ConsumerTag { get; protected set; }

        /// <summary>
        /// 上下文
        /// </summary>
        protected IRabbitMQClientContext Context { get; private set; }

        /// <summary>
        /// 每次最多接受消息 Size
        /// </summary>
        public uint PrefetchSize { get; set; }

        /// <summary>
        /// 每次最多接受消息 Count
        /// </summary>
        public ushort PrefetchCount { get; set; }

        /// <summary>
        /// 是否自动确认
        /// </summary>
        public bool IsAck { get; set; }

        /// <summary>
        /// ConsumerEvent
        /// </summary>
        public EventingBasicConsumer ConsumerEvent { get; protected set; }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="Context"></param>
        /// <param name="queueName"></param>
        public MqConsumer(IRabbitMQClientContext Context, string queueName,bool IsAck, uint PrefetchSize, ushort PrefetchCount)
        {
            this.Context = Context;
            this.QueueName = queueName;
            this.PrefetchCount = PrefetchCount;
            this.PrefetchSize = PrefetchSize;
            this.IsAck = IsAck;
        }


        /// <summary>
        /// 注册接受事件
        /// </summary>
        /// <returns></returns>
        public MqConsumer Register<T>(ConsumerReciveAction<T> action) where T:class
        {
            this.ConsumerEvent = new EventingBasicConsumer(this.Context.Channel);

            this.Context.Channel.BasicQos(this.PrefetchSize, this.PrefetchCount, false);

            this.ConsumerEvent.Received += (ch, ea) =>
            {
                try
                {
                    BasicResult<T> basicResult = null;

                    if (ea != null)
                    {
                        basicResult = new BasicResult<T>(
                            deliveryTag:ea.DeliveryTag, 
                            redelivered:ea.Redelivered, 
                            exchange : ea.Exchange, 
                            routingKey : ea.RoutingKey, 
                            messageCount : 0, 
                            basicProperties: ea.BasicProperties, 
                            body:ea.Body);
                          
                        //手动确认 注册ack事件
                        if (!IsAck)
                        {
                            basicResult.AckHandler = (x, y) => { this.Context.Channel.BasicAck(ea.DeliveryTag, false); };
                        }
                    }
                    action(basicResult);
                }
                catch (Exception ex)
                {
                    //错误日志
                    Ucs.Common.Logging.LogError(ex.ToString(), ex);
                    //处理失败消息持久化
                    Ucs.Common.Logging.LogInfo(
                        string.Format("DeliveryTag :{0},Redelivered:{1},Exchange:{2},RoutingKey:{3},Body:{4}", ea.DeliveryTag,
                             ea.Redelivered,
                             ea.Exchange,
                             ea.RoutingKey,
                             Encoding.UTF8.GetString(ea.Body)), LogType.Info);
                }
            };

            return this;
        }

        /// <summary>
        /// 启动订阅
        /// </summary>
        public void StartSubscribe()
        {
            this.ConsumerTag = this.Context.Channel.BasicConsume(this.QueueName, IsAck, this.ConsumerEvent);
        }

        /// <summary>
        /// 析构 关闭连接
        /// </summary>
        public void Dispose()
        {
            this.Context.Dispose();
        }
    }
}
