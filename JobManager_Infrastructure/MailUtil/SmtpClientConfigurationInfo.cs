using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace JobManager_Infrastructure.MailUtil
{
    public class SmtpClientConfigurationInfo
    {
        public static string Host { get { return "xxxx"; } private set { } }
        public static string UserName { get { return "xxxxx"; } private set { } }
        public static string PassWord { get { return "xxxx"; } private set { } }
        public static SmtpDeliveryMethod SmtpDeliveryMethod { get { return SmtpDeliveryMethod.Network; } private set { } }
        public static bool UseDefaultCredentials { get { return true; } private set { } }
    }
}
