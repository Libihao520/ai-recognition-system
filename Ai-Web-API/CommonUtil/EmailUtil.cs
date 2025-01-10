using System.Net.Mail;
using Microsoft.Extensions.Options;
using Model.Options;

namespace CommonUtil;

/// <summary>
/// 邮件发送工具类
/// </summary>
public class EmailUtil
{
    private readonly EmailOptions _emailOptions;

    public EmailUtil(IOptionsMonitor<EmailOptions> emailOptions)
    {
        _emailOptions = emailOptions.CurrentValue;
    }

    /// <summary>
    /// 发送邮件
    /// </summary>
    /// <param name="strMailText">邮件内容</param>
    /// <param name="strTitle">标题</param>
    /// <param name="to">收件人(多人用;隔开)</param>
    /// <param name="cc">抄送(多人用;隔开)</param>
    /// <param name="bcc">密抄(多人用;隔开)</param>
    /// <returns></returns>
    public string NetSendEmail(string strMailText, string strTitle, string to, string cc = "", string bcc = "")
    {
        try
        {
            List<string> toList = to.Split(';').ToList();
            List<string> ccList = cc.Split(';').ToList();
            List<string> bccList = bcc.Split(';').ToList();
            return NetSendEmail(strMailText, strTitle, toList, ccList, bccList);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    /// <summary>
    /// 发送邮件
    /// </summary>
    /// <param name="strMailText"></param>
    /// <param name="strTitle"></param>
    /// <param name="to">收件人</param>
    /// <param name="cc">抄送</param>
    /// <param name="bcc">密抄</param>
    /// <returns></returns>
    public string NetSendEmail(string strMailText, string strTitle, List<string> to, List<string> cc,
        List<string> bcc = null, string path = "")
    {
        try
        {
            System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
            mailMessage.Subject = strTitle;
            mailMessage.Body = strMailText;
            var emailAndKeys = _emailOptions.EmailAndKeys;
            int emailCount = emailAndKeys.Count;
            if (emailCount == 0)
            {
                throw new InvalidOperationException("未配置任何邮件账户信息。");
            }

            Random random = new Random();
            int randomNumber = random.Next(0, emailCount);
            var emailOptionsEmailAndKey = emailAndKeys[randomNumber];
            mailMessage.From = new System.Net.Mail.MailAddress(emailOptionsEmailAndKey.MyEmail);

            if (to == null || to.Count == 0)
            {
                return "请填写收件人";
            }

            foreach (string semail in to)
            {
                if (!string.IsNullOrEmpty(semail))
                {
                    mailMessage.To.Add(semail);
                }
            }

            foreach (string semail in cc)
            {
                if (!string.IsNullOrEmpty(semail))
                {
                    mailMessage.CC.Add(semail);
                }
            }

            if (bcc != null)
            {
                foreach (string semail in bcc)
                {
                    if (!string.IsNullOrEmpty(semail))
                    {
                        mailMessage.Bcc.Add(semail);
                    }
                }
            }

            mailMessage.Priority = System.Net.Mail.MailPriority.Normal;
            mailMessage.IsBodyHtml = true;
            System.Net.Mail.SmtpClient smtpClient = new System.Net.Mail.SmtpClient();
            smtpClient.Port = 587; // 使用587端口，通常用于SMTP over TLS
            smtpClient.EnableSsl = true; // 启用SSL加密


            smtpClient.Credentials =
                new System.Net.NetworkCredential(emailOptionsEmailAndKey.MyEmail, emailOptionsEmailAndKey.MyKey);

            smtpClient.Host = "smtp.qq.com";
            if (!string.IsNullOrEmpty(path))
            {
                mailMessage.Attachments.Add(new Attachment(path));
            }

            smtpClient.Send(mailMessage);
            return "发送成功！";
        }
        catch (Exception ex)
        {
            throw new Exception("发送邮件时出错: " + ex.Message, ex);
        }
    }
}