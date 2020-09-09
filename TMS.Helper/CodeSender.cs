using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Helper
{//用于绑定或修改密码时发送随机验证码
    public class CodeSender
    {
        //receivingMailBox-收件邮箱
        public static string SendInfo(string receivingMailBox)
        {
            SmtpClient client = new SmtpClient("smtp.163.com", 25);
            Random Rdm = new Random();
            string emailContentA = "【工夹具管理系统】亲爱的用户，\n您好！我们已收到您的验证码获取申请。本次验证码为：";
            string emailContentB = "【验证码有效期：2分钟】（验证码告知他人将导致帐号被盗，请勿泄露）";
            //产生0到100000的随机数
            int iRdm = Rdm.Next(00000, 99999);
            MailMessage msg = new MailMessage("ljj_sgg_ddd666@163.com", receivingMailBox, "验证码", emailContentA+iRdm.ToString()+ emailContentB);
            client.UseDefaultCredentials = false;
            System.Net.NetworkCredential basicAuthenticationInfo =
             new System.Net.NetworkCredential("ljj_sgg_ddd666@163.com", "DRKUYILCQPVTWZXC");
            client.Credentials = basicAuthenticationInfo;
            client.EnableSsl = true;
            client.Send(msg);
            try
            {
                client.Send(msg);
                return "1,"+ iRdm.ToString();
            }
            catch (Exception e)
            {
                //Console.WriteLine(e.Message);
                return "0,"+ e.Message;
            }
        }
    }
}
