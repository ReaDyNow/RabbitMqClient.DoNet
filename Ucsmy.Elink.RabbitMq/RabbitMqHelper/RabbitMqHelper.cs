using System;
using System.Text;
<<<<<<< .mine
=======
using System.Threading.Tasks;
using Newtonsoft.Json;
using Ucsmy.Elink.RabbitMq.SDK.Config;
>>>>>>> .r11342
using Ucsmy.Elink.RabbitMq.SDK.ConfigLoader;

namespace Ucsmy.Elink.RabbitMq.SDK
{
    /// <summary>
    /// 帮助类 提供与Rabbit服务端的所有交互
    /// </summary>
    public class RabbitMqHelper
    {
        /// <summary>
        /// MqConnectionConfig
        /// </summary>
        public IMqConnectionConfig MqConnectionConfig
        {
            get;
            set;
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="config"></param>
        public RabbitMqHelper(IMqConnectionConfig config)
        {
            this.MqConnectionConfig = config;
        }

        /// <summary>
        /// ctor
        /// </summary>
        public RabbitMqHelper()
        {
            this.MqConnectionConfig = XMLLoader.GetMqConfig();
        }

        /// <summary>
        /// CreateConsumer
        /// </summary>
        /// <param name="queueName"></param>
        /// <returns></returns>
        public MqConsumer CreateConsumer(
            string queueName,
            bool IsAck = false,
            uint PrefetchSize = 0, 
            ushort PrefetchCount = 1,
            IRabbitMQClientContextBuilder Builder = null)
        {
            //创建连接
            var builer = Builder ?? new CommonBuilder();
            var context = builer.Build(this.MqConnectionConfig);
        
            return new MqConsumer(context, queueName,IsAck, PrefetchSize, PrefetchCount);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="exchangeName"></param>
        /// <param name="routingKey"></param>
        /// <param name="IsPersistent"></param>
        public void Publish<T>(
            string exchangeName,
            string routingKey,
            T t,
            bool IsPersistent = true,
            IRabbitMQClientContextBuilder Builder = null)
        {
            //创建连接
            var builer = Builder ?? new CommonBuilder();

            var context = builer.Build(this.MqConnectionConfig);

            var mes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(t));

            var props = context.Channel.CreateBasicProperties();

            props.Persistent = IsPersistent;

            context.Channel.BasicPublish(
                   exchange: exchangeName,
                   routingKey: routingKey,
                   basicProperties: props,
                   mandatory: true,
                   body: mes);

            context.Dispose();
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exchangeName"></param>
        /// <param name="t"></param>
        /// <param name="routingKey"></param>
        /// <param name="IsPersistent"></param>
        /// <param name="Builder"></param>
        public void Publish<T>(
           string queueName, T t,
           bool IsPersistent = true,
           IRabbitMQClientContextBuilder Builder = null)
        {
            //创建连接
            var builer = Builder ?? new CommonBuilder();

            var context = builer.Build(this.MqConnectionConfig);

            var mes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(t));

            var props = context.Channel.CreateBasicProperties();

            props.Persistent = IsPersistent;

            context.Channel.BasicPublish(
                   exchange: "",
                   routingKey: queueName,
                   basicProperties: props,
                   mandatory: true,
                   body: mes);

            context.Dispose();
        }

        /// <summary>
        ///  接受消息  自动确认
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queueName"></param>
        /// <param name="IsAck"></param>
        /// <param name="Builder"></param>
        /// <returns></returns>
        public BasicResult<T> AutoAckReceive<T>(string queueName, IRabbitMQClientContextBuilder Builder = null) where T:class
        {
            //创建连接
            var builer = Builder ?? new CommonWithLockBuilder();

            var context = builer.Build(this.MqConnectionConfig);

            var basicGetResult = context.Channel.BasicGet(queueName, true);

            context.Dispose();

            return basicGetResult != null && basicGetResult.Body != null ? new BasicResult<T>(basicGetResult) : null;
        } 

        /// <summary>
        /// 接受消息  手工确认
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queueName"></param>
        /// <param name="Builder"></param>
        /// <returns></returns>
        public BasicResult<T> Receive<T>(string queueName, IRabbitMQClientContextBuilder Builder = null) where T : class
        {
            //创建连接
            var builer = Builder ?? new CommonWithLockBuilder();

            var context = builer.Build(this.MqConnectionConfig);

            var basicGetResult = context.Channel.BasicGet(queueName, false);

            return
              basicGetResult != null && basicGetResult.Body != null ?
              new BasicResult<T>(basicGetResult) { AckHandler = this.GetAckHandler(basicGetResult.DeliveryTag, false, context) }
              : null;
        }

        /// <summary>
        /// GetAckHandler
        /// </summary>
        /// <param name="deliveryTag"></param>
        /// <param name="context"></param>
        protected  Action<ulong, bool> GetAckHandler(ulong deliveryTag, bool multiple, IRabbitMQClientContext context) 
        {
            return (x, y) => { context.Channel.BasicAck(deliveryTag, multiple); context.Dispose(); };
        }
    }
}
