using NUnit.Framework;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ucsmy.Elink.RabbitMq.SDK;
using Ucsmy.Elink.RabbitMq.SDK.ConfigLoader;

namespace Test
{
    public class Data
    {
        public int ID;

        public string Content = "This is the MesContent";

    }

    /// <summary>
    /// 
    /// </summary>
    class Program
    {
        public static ConsumerReciveAction<Data> myAction =
            (x) =>
            {
                if (x != null)
                {
                    Console.Write(x.Content.Content);
                    x.Ack();
                }
            };

        public static RabbitMqHelper mq = new RabbitMqHelper();

        public static Data d = new Data() { Content = "123", ID = 1 };

        /// <summary>
        /// 发布
        /// </summary>
        [Test]
        public static void Publish_Test()
        {
            for (int i = 0; i < 4; i++)
            {
                Task.Factory.StartNew(
                    () =>
                    {
                        while (true)
                        {
                            try
                            {
                                //发布消息 
                                mq.Publish<Data>("Queue_Test", d);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.ToString());
                            }
                        }
                    }
                    );
            }
        }

        #region 发布测试

        /// <summary>
        /// 发布
        /// </summary>
        [Test]
        public static void Publish_Test1()
        {
            Assert.DoesNotThrow( () => mq.Publish<Data>("Queue_Test", d));
        }

        /// <summary>
        /// 并发发布
        /// </summary>
        [Test]
        public static void Publish_Test2()
        {

            Data d = new Test.Data()
            {
                ID = 0,
                Content = Encoding.UTF8.GetString(System.IO.File.ReadAllBytes(@"C:\Users\ucs_huangxiaofan\Downloads\01.exe"))
            };

            for (int i = 0; i < 4; i++)
            {
                Task.Factory.StartNew(
                    () =>
                    {
                        while (true)
                        {
                            Assert.DoesNotThrow(() => mq.Publish<Data>("Queue_Test", d));
                        }
                    }
                    );
            }
        }


        /// <summary>
        /// 数据较大的请求
        /// </summary>
        public static void Publish_Test3()
        {
            Data d = new Test.Data()
            {
                ID = 0,
                Content = Encoding.UTF8.GetString(System.IO.File.ReadAllBytes(@"C:\Users\ucs_huangxiaofan\Downloads\02.zip"))
            };

            mq.Publish<string>("Queue_Test", d.Content);

            Assert.DoesNotThrow(() => mq.Publish<Data>("Queue_Test", d));
        }

        #endregion 


        [Test]
        public static void Receive()
        {
            for (int i = 0; i < 4; i++)
            {
                Task.Factory.StartNew(
                    () =>
                    {
                        while (true)
                        {
                            try
                            {
                                //发布消息 
                                var t = mq.Receive<Data>("Queue_Test");
                                if (t != null)
                                {
                                    t.Ack();
                                    Console.WriteLine(t.Content.Content);
                                }

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.ToString());
                            }
                        }
                    }
                    );
            }

        }

        [Test]
        public static void AutoAckReceive()
        {
            for (int i = 0; i < 4; i++)
            {
                Task.Factory.StartNew(
                    () =>
                    {
                        while (true)
                        {
                            try
                            {
                                //发布消息 
                                var t = mq.AutoAckReceive<Data>("Queue_Test");
                                if (t != null)
                                {
                                    t.Ack();
                                    Console.WriteLine(t.Content.Content);
                                }

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.ToString());
                            }
                        }
                    }
                    );
            }
        }

        [Test]
        public static void Test1()
        {
            ConsumerReciveAction<string> myAction = (x) =>
            {
                x.Ack();
                var t = x.Content;
                if (t != null)
                {
                    throw (new Exception("error"));
                    //Console.WriteLine(t);
                }
            };
            //监控接收日志消息
            mq.CreateConsumer("Queue_Test").Register<string>(myAction).StartSubscribe();
        }

        static void Main(string[] args)
        {

            Publish_Test3();


            //mq.Publish<Data>("Queue_Test", new Data() { ID = 1 });
            //mq.Publish<Data>("Queue_Test", new Data() { ID = 2 });
            //mq.Publish<Data>("Queue_Test", new Data() { ID = 3 });
            //mq.Publish<Data>("Queue_Test", new Data() { ID = 1 });
            //Test1();

            //var b = new CommonBuilder().Build(XMLLoader.GetMqConfig());

            //b.Dispose();

            ////发布消息


            //Publish();
            //Receive();
            //AutoAckReceive();

            //mq.CreateConsumer("Queue_Test").Register<Data>(myAction).StartSubscribe();


            //var t = Ucs.Common.JsonUtils.Deserialize<string>("\"123\"");

            //var t = Ucs.Common.JsonUtils.Deserialize<string>("Queue_Test");
            //Publish();

            //Test1();

            //Thread.Sleep(5000);

            //Test1();

            //Thread.Sleep(5000);

            //Test1();

            Console.Read();
        }
    }
}




