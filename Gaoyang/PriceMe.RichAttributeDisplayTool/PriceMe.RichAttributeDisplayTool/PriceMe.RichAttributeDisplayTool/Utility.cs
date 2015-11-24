using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;

namespace PriceMe.RichAttributeDisplayTool
{
    class Utility
    {
        public Utility() { 
            
        }

        public static string EmailAddress = ConfigurationManager.AppSettings["EmailAddress"].ToString();
        public static string AccessKey = ConfigurationManager.AppSettings["AccessKey"];
        public static string SecretKey = ConfigurationManager.AppSettings["SecretKey"];

        /// <summary>
        /// 返回转换后的类型值
        /// </summary>
        /// <param name="value">要转换的类型值</param>
        /// <param name="type">要转换的类型</param>
        /// <returns></returns>
        public static object ChangeType(object value, Type type)
        {
            try
            {
                if (value == null && type.IsGenericType) return Activator.CreateInstance(type);
                if (value == null) return null;
                if (type == value.GetType()) return value;
                if (type.IsEnum)
                {
                    if (value is string)
                        return Enum.Parse(type, value as string);
                    else
                        return Enum.ToObject(type, value);
                }
                if (!type.IsInterface && type.IsGenericType)
                {
                    Type innerType = type.GetGenericArguments()[0];
                    object innerValue = ChangeType(value, innerType);
                    return Activator.CreateInstance(type, new object[] { innerValue });
                }
                if (value is string && type == typeof(Guid)) return new Guid(value as string);
                if (value is string && type == typeof(Version)) return new Version(value as string);
                if (!(value is IConvertible)) return value;
                return Convert.ChangeType(value, type);
            }
            catch (Exception ex)
            {
                return "change-error";
            }

        }



        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="from">发件人</param>
        /// <param name="subject">标题</param>
        /// <param name="body">内容</param>
        /// <param name="to">收件人</param>
        public static void sendEmail(string title, string body)
        {
            var to = EmailAddress.Split(',');
            string _aWSAccessKey = AccessKey;
            string _aWSSecretKey = SecretKey;
            AmazonSimpleEmailServiceClient ses = new AmazonSimpleEmailServiceClient(_aWSAccessKey, _aWSSecretKey);

            SendEmailRequest seReq = new SendEmailRequest();
            seReq.Source = "Channelyser <info@channelyser.com>";

            Destination det = new Destination();
            det.ToAddresses = to.ToList();

            seReq.Destination = det;

            Message mes = new Message();
            Content con = new Content();
            con.Data = title;
            mes.Subject = con;

            Body bodyObj = new Body();
            Content conHtml = new Content();
            conHtml.Data = body;
            bodyObj.Text = conHtml;
            bodyObj.Html = conHtml;
            mes.Body = bodyObj;

            seReq.Message = mes;

            //list = new List<string>();
            //list.Add(ConfigAppString.ReplyToEmail);
            //seReq.ReplyToAddresses = list;

            seReq.ReplyToAddresses = new List<string>() { "Channelyser <info@channelyser.com>" };

            ses.SendEmail(seReq);
        }

        /// <summary>
        /// 程序执行成功写的日志
        /// </summary>
        /// <param name="info"></param>
        public static void writeSuccLog(string info) {

            var log = new LogFileController("Succ");
            log.writeSuccInfo(info);
            log.succClose();
        }

        /// <summary>
        /// 程序执行成功写的日志
        /// </summary>
        /// <param name="info"></param>
        public static void writeFailLog(string info)
        {
            var log = new LogFileController();
            log.writeFailedInfo(info);
            log.failedClose();
        }

    }
}
