using System;
using System.IO;
using System.Web;
using System.Xml;

namespace Ucsmy.Elink.RabbitMq.SDK.ConfigLoader
{
    /// <summary>
    /// XMLLoader
    /// </summary>
    public class XMLLoader
    {
        /// <summary>
        /// 加载  从配置文件加载到实体对象
        /// </summary>
        /// <returns></returns>
        public static IMqConnectionConfig GetMqConfig()
        {
            var result = new MqConnectionConfig();

            XmlDocument xml = new XmlDocument();
          
            string filePath = AppDomain.CurrentDomain.BaseDirectory;
   
#if (DEBUG)
            if (HttpContext.Current == null)
            {
                filePath = new DirectoryInfo(filePath).Parent.Parent.FullName;
            }
#endif

            xml.Load(Path.Combine(filePath, "Config", "RabbitMqConfig.xml"));

            XmlNode root = xml.SelectSingleNode("//mqConnectionConfig");

            if (root != null)
            {
                XmlNode node = null;

                node = root.SelectSingleNode("HostName");

                result.HostName = node != null && !string.IsNullOrWhiteSpace(node.InnerText) ? node.InnerText : string.Empty;

                node = root.SelectSingleNode("Password");

                result.Password = node != null && !string.IsNullOrWhiteSpace(node.InnerText) ? node.InnerText : string.Empty;

                node = root.SelectSingleNode("UserName");

                result.UserName = node != null && !string.IsNullOrWhiteSpace(node.InnerText) ? node.InnerText : string.Empty;

                node = root.SelectSingleNode("RequestedHeartbeat");

                result.RequestedHeartbeat = node != null && !string.IsNullOrWhiteSpace(node.InnerText) ? Convert.ToUInt16(node.InnerText) : default(ushort);

                node = root.SelectSingleNode("VirtualHost");

                result.VirtualHost = node != null && !string.IsNullOrWhiteSpace(node.InnerText) ? node.InnerText : "/";

                node = root.SelectSingleNode("Port");

                result.Port = node != null && !string.IsNullOrWhiteSpace(node.InnerText)
                    ? Convert.ToInt32(node.InnerText) : default(int);

                node = root.SelectSingleNode("AutomaticRecoveryEnabled");

                result.AutomaticRecoveryEnabled = node != null && !string.IsNullOrWhiteSpace(node.InnerText) ?
                    Convert.ToBoolean(node.InnerText) : false;

            }
            return result;
        }
    }
}

