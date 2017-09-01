namespace Ucsmy.Elink.RabbitMq.SDK
{
    /// <summary>
    /// 消费者 接受消息事件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="mes"></param>
    public delegate void ConsumerReciveAction<T>(BasicResult<T> mes) where T : class;
}
