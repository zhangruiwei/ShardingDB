using Newtonsoft.Json;
using System.Xml;
using System.Xml.Linq;

namespace Warren.OrderService.Infrastructure.Common
{
    /// <summary>
    ///     JSON帮助类
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>
        ///     把对象序例化成JSON
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string JsonSerializeInternal(dynamic obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        ///     以指定时间戳格式把对象序例化成JSON
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="dateTimeFormat">时间戳格式</param>
        /// <returns></returns>
        public static string JsonSerializeInternal(dynamic obj, DateFormatHandling dateTimeFormat)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                DateFormatHandling = dateTimeFormat
            };
            return JsonConvert.SerializeObject(obj, settings);
        }


        /// <summary>
        /// JSON反序列化
        /// </summary>
        public static T JsonDeserialize<T>(string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        /// <summary>
        /// 将对象序列化成XML
        /// </summary>
        /// <param name="obj">要序列化的对象</param>
        /// <param name="rootNode">根结点</param>
        /// <returns></returns>
        public static XNode JsonSerializeToXml(dynamic obj, string rootNode)
        {
            return JsonConvert.DeserializeXNode(JsonConvert.SerializeObject(obj), rootNode);
        }

        /// <summary>
        /// 将xml序列化为json
        /// </summary>
        /// <param name="xml">xml对象</param>
        /// <returns>json字符串</returns>
        public static string SerializeXmlToJson(XmlDocument xml)
        {
            return JsonConvert.SerializeXmlNode(xml);
        }
    }
}
