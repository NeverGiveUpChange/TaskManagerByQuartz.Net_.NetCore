using JobManager_Infrastructure.LogUtil;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Mail;
using System.Text;

namespace JobManager_Infrastructure.MailUtil
{
    public class MailClient
    {
        private MailMessageConfigurationInfo mailMessageInfo = null;
        private string sendState = string.Empty;

        public void SendMail(MailMessageConfigurationInfo mailMessageConfigurationInfo)
        {
            DateTime dateStart = DateTime.Now;
            try
            {

                mailMessageInfo = mailMessageConfigurationInfo;
                var smtpClient = _configureSmtpClient();
                var mailMessage = _configureMailMessage();

                smtpClient.SendCompleted += new SendCompletedEventHandler(_stmp_SendCompleted);
                dateStart = DateTime.Now;
                smtpClient.SendAsync(mailMessage, "ok");
            }
            catch (Exception ex)
            {
                var dateEnd = DateTime.Now;
                sendState = "异步发送邮件失败";

                CustomerLogUtil.Error(ex, CustomerLogFormatUtil.LogMailMsgfFormat(SmtpClientConfigurationInfo.UserName, string.Join(",", mailMessageInfo.ToMailAddressList), string.Join(",", mailMessageInfo.CCMailAddressList), mailMessageInfo.Subject, mailMessageInfo.Body, sendState), new CustomerLogParameters(Guid.NewGuid().ToString("N"), "SendMail", mailMessageInfo.Body, sendState, dateStart.ToString("yyyy-MM-dd HH:mm:ss.ffff"), dateEnd.ToString("yyyy-MM-dd HH:mm:ss.ffff"), $"{((dateEnd.Ticks - dateStart.Ticks) / 10000)}ms"));
            }
        }

        private void _stmp_SendCompleted(object sender, AsyncCompletedEventArgs e)
        {
            string sendState = string.Empty;
            Exception ex = null;
            DateTime dateStart = DateTime.Now;
            if (e.Cancelled)
            {
                var dateEnd = DateTime.Now;
                sendState = "异步发送邮件取消";
                CustomerLogUtil.Info(CustomerLogFormatUtil.LogMailMsgfFormat(SmtpClientConfigurationInfo.UserName, string.Join(",", mailMessageInfo.ToMailAddressList), string.Join(",", mailMessageInfo.CCMailAddressList), mailMessageInfo.Subject, mailMessageInfo.Body, sendState), new CustomerLogParameters(Guid.NewGuid().ToString("N"), "SendMail", mailMessageInfo.Body, sendState, dateStart.ToString("yyyy-MM-dd HH:mm:ss.ffff"), dateEnd.ToString("yyyy-MM-dd HH:mm:ss.ffff"), $"{((dateEnd.Ticks - dateStart.Ticks) / 10000)}ms"));
            }
            else if (e.Error != null)
            {
                var dateEnd = DateTime.Now;
                sendState = "异步发送邮件失败";
                ex = e.Error;
                CustomerLogUtil.Error(ex, CustomerLogFormatUtil.LogMailMsgfFormat(SmtpClientConfigurationInfo.UserName, string.Join(",", mailMessageInfo.ToMailAddressList), string.Join(",", mailMessageInfo.CCMailAddressList), mailMessageInfo.Subject, mailMessageInfo.Body, sendState), new CustomerLogParameters(Guid.NewGuid().ToString("N"), "SendMail", mailMessageInfo.Body, sendState, dateStart.ToString("yyyy-MM-dd HH:mm:ss.ffff"), dateEnd.ToString("yyyy-MM-dd HH:mm:ss.ffff"), $"{((dateEnd.Ticks - dateStart.Ticks) / 10000)}ms"));
            }
            else
            {
                var dateEnd = DateTime.Now;
                sendState = "异步发送邮件成功";
                CustomerLogUtil.Info(CustomerLogFormatUtil.LogMailMsgfFormat(SmtpClientConfigurationInfo.UserName, string.Join(",", mailMessageInfo.ToMailAddressList), string.Join(",", mailMessageInfo.CCMailAddressList), mailMessageInfo.Subject, mailMessageInfo.Body, sendState), new CustomerLogParameters(Guid.NewGuid().ToString("N"), "SendMail", mailMessageInfo.Body, sendState, dateStart.ToString("yyyy-MM-dd HH:mm:ss.ffff"), dateEnd.ToString("yyyy-MM-dd HH:mm:ss.ffff"), $"{((dateEnd.Ticks - dateStart.Ticks) / 10000)}ms"));
            }
        }
        private SmtpClient _configureSmtpClient()
        {

            var smtpClient = new SmtpClient();
            smtpClient.DeliveryMethod = SmtpClientConfigurationInfo.SmtpDeliveryMethod;
            smtpClient.Host = SmtpClientConfigurationInfo.Host;
            smtpClient.UseDefaultCredentials = SmtpClientConfigurationInfo.UseDefaultCredentials;
            smtpClient.Credentials = new System.Net.NetworkCredential(SmtpClientConfigurationInfo.UserName, SmtpClientConfigurationInfo.PassWord);
            return smtpClient;
        }

        private MailMessage _configureMailMessage()
        {
            var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(MailMessageConfigurationInfo.FromMailAddress);
            mailMessageInfo.ToMailAddressList.ForEach(x => mailMessage.To.Add(x));
            mailMessageInfo.CCMailAddressList.ForEach(x => mailMessage.CC.Add(x));
            mailMessage.Subject = mailMessageInfo.Subject;
            mailMessage.Body = mailMessageInfo.Body;
            mailMessage.BodyEncoding = MailMessageConfigurationInfo.BodyEncoding;
            mailMessage.IsBodyHtml = MailMessageConfigurationInfo.IsBodyHtml;
            mailMessage.Priority = MailMessageConfigurationInfo.Priority;
            return mailMessage;


        }
    }
}
