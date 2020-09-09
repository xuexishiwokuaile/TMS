using System;
using System.Linq;
using System.Web.Mvc;
using TMS.Helper;
using System.Drawing;
using System.Web.Caching;
using System.IO;
using System.Drawing.Imaging;
using Newtonsoft.Json.Linq;
using TMS.EF;
using System.Text.RegularExpressions;

namespace UI.Controllers
{
    public class LoginController : Controller
    {
        //登录页面
        public ActionResult Login()
        {
            return View();
        }
        //登录表单验证---登录
        public ActionResult LoginSubmit(string userid, string userpassword,string verifycode)
        {
            using (TMSDbContext context = new TMSDbContext())
            {
                Cache cache = new Cache();
                var verifyCodeKey = $"{this.GetType().FullName}_verifyCode";
                object cacheobj = cache.Get(verifyCodeKey);
                user userEntity = context.user.FirstOrDefault(u => u.email == userid);
                if (cacheobj == null)
                {//验证码过期
                    
                    return Json(new { Success = -1, Message = "验证码已过期",userinfo="" });
                }
                else if (cacheobj.ToString().Equals(verifycode, StringComparison.CurrentCultureIgnoreCase))
                {//验证码成功
                    if (userEntity == null)
                    {
                        return Json(new { Success = 0, Message = "当前用户不存在", userinfo = "" });
                    }
                    else if (userEntity.email == userid && userEntity.pwd == MD5Encrypt.Encrypt(userpassword))
                    {
                        if (userEntity.state == 0)
                            return Json(new { Success = 0, Message = "该用户账号已冻结", userinfo = "" });
                        else
                        {
                            string jsonText = TMS.Helper.JsonSerializer.ToJSON(userEntity);
                            HttpContext.Session["userInfo"] = userEntity;
                            return Json(new { Success = 1, Message = "登录成功", userinfo = userEntity });
                        }
                            
                    }
                    else
                    {
                        return Json(new { Success = 0, Message = "用户密码错误请重新输入", userinfo = "" });
                    }
                }
                else
                {//验证码错误
                    return Json(new { Success = -1, Message = "验证码错误", userinfo = "" });
                }
               
            }
            //return Json(new { pwd = MD5Encrypt.Encrypt(userpassword) });
        }
        //生成验证码，并将验证码加入Cache---登录
        public ActionResult CreateVerifyCode()
        {
            string verifyCode =string.Empty;
            Bitmap bitmap = VerifyCodeHelper.CreateVerifyCode(out verifyCode);

            #region 缓存Key 
            Cache cache = new Cache();
            // 先用当前类的全名称拼接上字符串 “verifyCode” 作为缓存的key
            //先移除上一次生成的串
            var verifyCodeKey = $"{this.GetType().FullName}_verifyCode";
            cache.Remove(verifyCodeKey);
            cache.Insert(verifyCodeKey, verifyCode);
            #endregion

            MemoryStream memory = new MemoryStream();
            bitmap.Save(memory, ImageFormat.Gif);
            return File(memory.ToArray(), "image/gif");
        }   
        //发送邮箱验证码--用于重置密码（忘记密码）
        public ActionResult SendCode(string email)
        {
            bool matches = Regex.IsMatch(email, "[A-Za-z\\d]+([-_.][A-Za-z\\d]+)*@([A-Za-z\\d]+[-.])+[A-Za-z\\d]{2,4}");
            if (!matches)
                return Json(new { code = 0, msg = "邮箱格式不正确" });
            using (TMSDbContext context = new TMSDbContext())
            {
                user userEntity = context.user.FirstOrDefault(u => u.email == email);
                if (userEntity != null)
                {
                    string result = CodeSender.SendInfo(email);
                    string[] str = result.Split(',');
                    Cache cache = new Cache();
                    var verifyCodeKey = $"{this.GetType().FullName}_emailCode";
                    cache.Remove(verifyCodeKey);
                    cache.Insert(verifyCodeKey, str[1], null, DateTime.UtcNow.AddMinutes(2), System.Web.Caching.Cache.NoSlidingExpiration);
                    return Json(new { code = str[0], msg = "验证码已发送，请注意查收！" });
                    //code:0失败，1成功，msg:发送成功或失败原因
                }
                else
                    return Json(new { code = 0, msg = "账户不存在，请重新输入" });
            }
        }
        //判断邮箱验证码是否正确--用于重置密码（忘记密码）
        public ActionResult VerifyECode(string email,string code,string pwd1,string pwd2)
        {
            var verifyCodeKey = $"{this.GetType().FullName}_emailCode";
            Cache cache = new Cache();
            object cacheobj = cache.Get(verifyCodeKey);
            
            if (cacheobj == null)
            {
                return Json(new { success = false, msg = "验证码已过期" });
            }
            else if (!cacheobj.ToString().Equals(code))
            {
                return Json(new { success = false, msg = "验证码错误请重新输入" });
            }
            else {
                if (pwd1.Equals(pwd2))
                {
                    using (TMSDbContext context = new TMSDbContext())
                    {
                        user userEntity = context.user.FirstOrDefault(u => u.email == email);
                        userEntity.pwd = MD5Encrypt.Encrypt(pwd1);
                        int flag = context.SaveChanges();
                        if (flag > 0)
                        {
                            return Json(new { success = true, msg = "重置密码成功，请重新登录" });
                        }
                        else
                        {
                            return Json(new { success = false, msg = "重置密码失败，请稍后重试" });
                        }
                    }
                }
                else {
                    return Json(new { success = false, msg = "两次输入密码不对称请重新输入" });
                }
            }
        }










    }
}