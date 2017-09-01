using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ucsmy.Elink.RabbitMq.SDK;

namespace Demo
{
    /// <summary>
    /// 定义消息 类型
    /// </summary>
    public class Data
    {
        public int ID;

        public string Content;
    }

    class Program
    {
        static void Main(string[] args)
        {
            //实例化 helper 对象     对象负责与mq的一起交互
            var mq = new RabbitMqHelper();

            #region 发布/推送消息 
            //创建消息实体
            var d = new Data() { Content = "HelloWorld", ID = 1 };
            //推送 指定 队列名称（发向哪里） 数据（发什么） 
            mq.Publish<Data>("Elink_Q_TradeIn", d);

            #endregion

            #region  接收消息 每次一条 

            //接受消息 自动确认
            //指定 队列名称（从哪里收消息）
            var result = mq.AutoAckReceive<Data>("Queue_Test");

            if (result != null)
            {
                Console.WriteLine(result.Content.Content);
            }

            //接受消息 手动确认
            //指定 队列名称（从哪里收消息）
            var result1 = mq.Receive<Data>("Queue_Test");

            if (result1 != null)
            {
                //确认
                result.Ack();
                Console.WriteLine(result.Content.Content);
            }

            #endregion

            #region 订阅 

            //接收事件
            ConsumerReciveAction<Data> myAction =
           (x) =>
           {
               if (x != null)
               {
                   Console.Write(x.Content.Content);
                   x.Ack();
               }
           };

            mq.CreateConsumer("Queue_Test").   //指定 队列名称（从哪里收消息）
                Register<Data>(myAction).      //注册事件
                StartSubscribe();              //订阅开始

            #endregion
        }
    }
}
