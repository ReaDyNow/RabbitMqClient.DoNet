namespace Ucsmy.Elink.RabbitMq.SDK
{
    /// <summary>
    /// 连接配置
    /// </summary>
    public class MqConnectionConfig : IMqConnectionConfig 
    {
        /// <summary>
        /// HostName
        /// </summary>

        public string HostName
        {
            get;
            set;
        }

        /// <summary>
        /// UserName
        /// </summary>
   
        public string UserName
        {
            get;
            set;
        }

        /// <summary>
        /// Password
        /// </summary>
  
        public string Password
        {
            get;
            set;
        }

        /// <summary>
        /// VirtualHost
        /// </summary>
       
        public string VirtualHost
        {
            get;
            set;
        }

        /// <summary>
        /// 心跳频率
        /// </summary>
       
        public ushort RequestedHeartbeat
        {
            get;
            set;
        }

        /// <summary>
        /// 自动恢复连接
        /// </summary>
 
        public bool AutomaticRecoveryEnabled
        {
            get;
            set;
        }

        /// <summary>
        /// 端口
        /// </summary>
    
        public int Port
        {
            get;
            set;
        }
    }
}
