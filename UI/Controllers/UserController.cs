using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using TMS.EF;
using TMS.Helper;
using UI.Models.User;

namespace UI.Controllers.UserController
{
    //已更新--2020.4.17
    public class UserController : Controller
    {
        // User--获取用户信息表初始化table
        public ActionResult GetUserInfo(int page, int limit)
        {
            using (TMSDbContext context = new TMSDbContext())
            {

                
                List<UserInfo> userinfo = (
                    from sUser in context.user
                    join sWorkcell in context.workcell on sUser.workcellid equals sWorkcell.id
                    join sClassification in context.classification on sUser.classificationid equals sClassification.id
                    orderby sUser.id ascending
                    select new UserInfo
                    {
                        id = sUser.id,
                        name = sUser.name,
                        telnumber = sUser.telnumber,
                        apartment = sUser.apartment,
                        gender = sUser.gender,
                        workcell = sWorkcell.name,
                        state = sUser.state,
                        email = sUser.email,
                        classification = sClassification.name
                    }).ToList();

                if (!Request.QueryString.Get("keywords").Equals(""))
                {
                    //搜索关键字不为空
                    string keywords = Request.QueryString.Get("keywords");
                    userinfo = userinfo.Where(s => s.name.Contains(keywords)).ToList();
                }
                List<UserInfo> users = userinfo.Skip(limit * (page - 1)).Take(limit).ToList();
                var result = new
                {
                    code = 0,
                    msg = "",
                    count = userinfo.Count(),
                    data = users
                };

                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }
        //根据id获取用户名---登录后导航栏显示 
        public ActionResult GetUserName()
        {
            using (TMSDbContext context = new TMSDbContext())
            {

                user userentity = (user)HttpContext.Session["userInfo"];
                if (userentity != null)
                {
                    return Json(new { data = userentity.name }, JsonRequestBehavior.AllowGet);
                }
                else
                {

                    return Json(new { data = "未知用户" }, JsonRequestBehavior.AllowGet);
                }
                
            }
        }
        //管理员在编辑个人信息时获取用户信息--管理员
        public ActionResult GetUser(Guid id)
        {
            using (TMSDbContext context = new TMSDbContext())
            {
                user userentity = context.user.FirstOrDefault(u => u.id == id);

                UserDetails user = new UserDetails {
                    id = id,
                    name = userentity.name,
                    phone = userentity.telnumber,
                    email = userentity.email,
                    apartment = userentity.apartment,
                    sex = userentity.gender.ToString(),
                    classification = userentity.classificationid.ToString(),
                    workcellid = userentity.workcellid
                };

                return Json(new { data = user });
                

            }
        }
        //更新和添加用户--管理员
        public ActionResult UpdateUser(UserDetails user)
        {
            
            using (TMSDbContext context = new TMSDbContext())
            {
                 
                int emailFlag = context.user.Count(s => s.email == user.email && s.id != user.id);
                int phoneFlag = context.user.Count(s => s.telnumber == user.phone && s.id != user.id);
                if (emailFlag > 0)
                {
                    return Json(new { msg = "更新失败，该邮箱已存在", success = false });
                }
                else if (phoneFlag > 0)
                {
                    return Json(new { msg = "更新失败，该电话号码已存在", success = false });
                }
                user userEntity = context.user.FirstOrDefault(s => s.id == user.id);
                //新增
                if (userEntity == null)
                {
                    string email;
                    if (user.email == null)
                        email = "null";
                    else
                        email = user.email;
                    //保证部门经理唯一
                    if (user.classification == "4")
                    {
                        int count = context.user.Count(s => s.workcellid == user.workcellid && s.classificationid == 4);
                        if(count>0)
                            return Json(new { msg = "更新失败,该部门已有经理", success = false });
                    }
                    user newUser = new user
                    {//初始密码为123456
                        id = Guid.NewGuid(),
                        name = user.name,
                        classificationid = int.Parse(user.classification),
                        telnumber = user.phone,
                        apartment = user.apartment,
                        workcellid = user.workcellid,
                        pwd = MD5Encrypt.Encrypt("123456"),
                        email =email,
                        gender = int.Parse(user.sex),
                        sessionid = null,
                        state = 1
                    };
                    context.user.Add(newUser);
                }//修改
                else {
                    //保证部门经理唯一
                    if (user.classification == "4")
                    {
                        int count = context.user.Count(s => s.workcellid == user.workcellid && s.classificationid == 4&&s.id!=user.id);
                        if (count > 0)
                            return Json(new { msg = "更新失败,该部门已有经理", success = false });
                    }
                    userEntity.name = user.name;
                    userEntity.telnumber = user.phone;
                    userEntity.workcellid = user.workcellid;
                    userEntity.gender = int.Parse(user.sex);
                    userEntity.email = user.email;
                    userEntity.apartment = user.apartment;
                    userEntity.classificationid = int.Parse(user.classification);
                }
                int state = context.SaveChanges();
                if (state > 0)
                {
                    return Json(new { msg = "更新成功", success = false });
                }
                else
                {
                    return Json(new { msg = "更新失败", success = false });
                }


            }
        }
        //返回classification的id与name的list
        public ActionResult ClassificationList()
        {
            using (TMSDbContext context = new TMSDbContext())
            {
                List<classification> classificationlist = context.classification.ToList();
                return Json(classificationlist, JsonRequestBehavior.AllowGet);
            }
        }
        //返回workcell的id和name的list
        public ActionResult WorkcellList()
        {
            using (TMSDbContext context = new TMSDbContext())
            {
                List<workcell> workcells = context.workcell.ToList();
                return Json(workcells, JsonRequestBehavior.AllowGet);
            }
        }
        //单个删除用户--管理员
        public ActionResult DeleteUser(Guid id)
        {
            using (TMSDbContext context = new TMSDbContext())
            {
                user userentity = context.user.FirstOrDefault(s => s.id == id);
                if (userentity != null)
                {
                    context.user.Remove(userentity);
                    int state = context.SaveChanges();
                    if (state > 0)
                    {
                        return Json(new { msg = "删除成功", success = true });
                    }
                    else
                    {
                        return Json(new { msg = "删除失败", success = false });
                    }
                }
                else {
                    return Json(new { msg = "用户不存在", success = false });
                }
            }
        }
        //批量删除--管理员
        public ActionResult batchDeleteUser(List<Guid> idList)
        {
            using (TMSDbContext context = new TMSDbContext())
            { 
                user userentity;
                for (int i = 0; i < idList.Count; i++)
                {
                    Guid userid = idList[i];
                    userentity = context.user.FirstOrDefault(s => s.id == userid);
                    context.user.Remove(userentity);
                }
                int flag = context.SaveChanges();
                if (flag > 0)
                {
                    return Json(new { msg = "批量删除成功", success = true });
                }
                else
                {
                    return Json(new { msg = "批量删除失败", success = false });
                }
            }
        }
        //获得根据用户在session中的classificationid获得模块信息--模块显示
        public ActionResult GetModuleList()
        {

            using (TMSDbContext context = new TMSDbContext())
            {
                user userEntity = (user)HttpContext.Session["userInfo"];
                classification classificationEntity = context.classification.FirstOrDefault(u => u.id == userEntity.classificationid);
                return Json(new { strHtml = classificationEntity.description }, JsonRequestBehavior.AllowGet);
            }
        }
        //禁用或启用该用户--管理员
        public ActionResult forbidOrRestart(Guid id)
        {
            using (TMSDbContext context = new TMSDbContext())
            {
                user userEntity = context.user.FirstOrDefault(u => u.id == id);
                if (userEntity.state == 0)
                    userEntity.state = 1;
                else
                    userEntity.state = 0;
                int flag = context.SaveChanges();
                if (flag > 0)
                {
                    return Json(new { success = true, msg = "修改成功" });
                }
                else {
                    return Json(new { success = false, msg = "修改失败" });
                }
                
            }
        }
        //个人模块修改密码--个人
        public ActionResult pwdChange(string expwd, string pwd)
        {
            using (TMSDbContext context = new TMSDbContext())
            {
                user userinSession = (user)HttpContext.Session["userinfo"];
                user userEntity = context.user.FirstOrDefault(u => u.id == userinSession.id);
                if (MD5Encrypt.Encrypt(expwd).Equals(userEntity.pwd))
                {
                    userEntity.pwd = MD5Encrypt.Encrypt(pwd);
                    int flag = context.SaveChanges();
                    if (flag > 0)
                    {
                        HttpContext.Session["userinfo"] = null;
                        return Json(new { success = true, msg = "密码重置成功，请重新登录" });
                    }
                    else {
                        return Json(new { success = false, msg = "密码重置失败请稍后重试" });
                    }
                }
                else {
                    return Json(new { success = false, msg = "密码错误" });
                }
            }
        }
        //person模块--获得个人信息--个人
        public ActionResult GetUserDetail()
        {
            user userinSession = (user)HttpContext.Session["userinfo"];
            using (TMSDbContext context = new TMSDbContext())
            {
                string classificationName = context.classification.FirstOrDefault(c => c.id == userinSession.classificationid).name;
                string workcellName = context.workcell.FirstOrDefault(w => w.id == userinSession.workcellid).name;
                return Json( new {name=userinSession.name,phone = userinSession.telnumber,email = userinSession.email,classification = classificationName ,workcellname = workcellName,apartment = userinSession.apartment }, JsonRequestBehavior.AllowGet);
            }
        }
        //更改住址--个人
        public ActionResult apartmentChange(string apartment)
        {
            using (TMSDbContext context = new TMSDbContext())
            {
                user userinSession = (user)HttpContext.Session["userinfo"];
                user userEntity = context.user.FirstOrDefault(u => u.id == userinSession.id);
                userEntity.apartment = apartment;
                int flag = context.SaveChanges();
                if (flag > 0)
                {
                    HttpContext.Session["userinfo"] = userEntity;//更新session
                    return Json(new { success = true, msg = "修改成功" });
                }
                else { 
                    return Json(new { success = false, msg = "修改失败，请稍后重试" });
                }
            }
        }
        //发送验证码--更改邮箱时--个人
        public ActionResult SendCode(string email)
        {
            bool matches = Regex.IsMatch(email, "[A-Za-z\\d]+([-_.][A-Za-z\\d]+)*@([A-Za-z\\d]+[-.])+[A-Za-z\\d]{2,4}");
            if (!matches)
                return Json(new { code = 0, msg = "邮箱格式不正确" });
            using (TMSDbContext context = new TMSDbContext())
            {
                user userEntity = context.user.FirstOrDefault(u => u.email == email);
                if (userEntity == null)
                {

                    string result = CodeSender.SendInfo(email);
                    string[] str = result.Split(',');
                    Cache cache = new Cache();
                    var verifyCodeKey = $"{this.GetType().FullName}_emailCode_changeEmail";
                    object cacheobj = cache.Get(verifyCodeKey);
                    cache.Remove(verifyCodeKey);
                    
                    cache.Insert(verifyCodeKey, str[1], null, DateTime.UtcNow.AddMinutes(2), System.Web.Caching.Cache.NoSlidingExpiration);
                    return Json(new { code = str[0], msg = "验证码已发送，请注意查收！" });
                    //code:0失败，1成功，msg:发送成功或失败原因
                }
                else
                    return Json(new { code = 0, msg = "该邮箱已被绑定，请重新输入" });
            }
        }
        //更改邮箱绑定--个人
        public ActionResult emailChange(string email, string code)
        {
            var verifyCodeKey = $"{this.GetType().FullName}_emailCode_changeEmail";
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
                using (TMSDbContext context = new TMSDbContext())
                {
                    user userinSession = (user)HttpContext.Session["userinfo"];
                    user userEntity = context.user.FirstOrDefault(u => u.id == userinSession.id);
                    userEntity.email = email;
                    int flag = context.SaveChanges();
                    if (flag > 0)
                    {
                        HttpContext.Session["userinfo"] = userEntity;//更新session
                        return Json(new { success = true, msg = "修改成功" });
                    }
                    else
                    {
                        return Json(new { success = false, msg = "修改失败，请稍后重试" });
                    }
                }
            }
            

        }
    }
    public class UserInfo
    {
        //用于返回前端的类
        public Guid id { get; set; }
        public string name { get; set; }
        public string telnumber { get; set; }
        public string apartment { get; set; }
        public int gender { get; set; }
        public string workcell { get; set; }
        public int state { get; set; }
        public string email { get; set; }
        public string classification { get; set; }
    }
}