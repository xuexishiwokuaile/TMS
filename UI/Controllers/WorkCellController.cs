using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMS.EF;
using UI.Models.Workcell;

namespace UI.Controllers
{
    public class WorkCellController : Controller
    {
        // GET: WorkCell
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetWorkCellInfo(int page, int limit)
        {
            using (TMSDbContext context = new TMSDbContext())
            {
                var u = from su in context.user
                        where su.classificationid == 4
                        select su;
                List<Workcell> workcellinfo = (
                    from sWorkcell in context.workcell
                    join sUser in u on sWorkcell.id equals sUser.workcellid
                    into re
                    from dq in re.DefaultIfEmpty()
                    orderby sWorkcell.id ascending
                    select new Workcell
                    {
                        id = sWorkcell.id,
                        name = sWorkcell.name,
                        manager = dq.name,
                    }).ToList();

                workcell workcellEntity;
                foreach (var workcell in workcellinfo)
                {
                    workcell.count = context.user.Where(s => s.workcellid == workcell.id).Count();
                    workcellEntity = context.workcell.FirstOrDefault(s => s.id == workcell.id);
                    workcell.date = workcellEntity.date != null && (DateTime)workcellEntity.date != (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue ? ((DateTime)workcellEntity.date).ToString("yyyy-MM-dd") : "";
                }

                if (!Request.QueryString.Get("keywords").Equals(""))
                {
                    //搜索关键字不为空
                    string keywords = Request.QueryString.Get("keywords");
                    workcellinfo = workcellinfo.Where(s => s.name.Contains(keywords)).ToList();
                }
                List<Workcell> workcells = workcellinfo.Skip(limit * (page - 1)).Take(limit).ToList();
                var result = new
                {
                    code = 0,
                    msg = "",
                    count = workcellinfo.Count(),
                    data = workcells
                };

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UserList(Guid workcellid)
        {
            using (TMSDbContext context = new TMSDbContext())
            {
                List<user> userEntity = context.user.Where(s => s.workcellid == workcellid && s.classificationid < 4).ToList();
                return Json(new { users = userEntity });
            }
        }

        public ActionResult CreateWorkcell(string name, Guid userid)
        {
            using (TMSDbContext context = new TMSDbContext())
            {
                workcell workcellEntity = context.workcell.FirstOrDefault(s => s.name == name);
                if (workcellEntity != null)
                    return Json(new { msg = "添加失败，部门重名", success = false });
                else
                {
                    workcell NewWorkcell = new workcell
                    {
                        id = Guid.NewGuid(),
                        name = name,
                        managerid = userid,
                        date = DateTime.Now
                    };
                    context.workcell.Add(NewWorkcell);
                    user userEntity = context.user.FirstOrDefault(s => s.id == userid);
                    userEntity.classificationid = 4;
                    userEntity.workcellid = NewWorkcell.id;
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

        }

        public ActionResult WUserList(Guid workcellid)
        {
            using (TMSDbContext context = new TMSDbContext())
            {

                List<UserInfo> list = (
                    from sUser in context.user
                    join sClassification in context.classification on sUser.classificationid equals sClassification.id
                    where sUser.workcellid == workcellid
                    orderby sUser.classificationid descending
                    select new UserInfo
                    {
                        name = sUser.name,
                        classification = sClassification.name
                    }).ToList();
                return Json(new { list });
            }
        }


        public ActionResult DeleteWorkcellAndUsers(Guid id)
        {
            using (TMSDbContext context = new TMSDbContext())
            {
                workcell workcellEntity = context.workcell.FirstOrDefault(s => s.id == id);
                List<user> userList = context.user.Where(s => s.workcellid == id).ToList();
                if (workcellEntity != null)
                {
                    context.workcell.Remove(workcellEntity);
                    foreach (user u in userList)
                    {
                        context.user.Remove(u);
                    }
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
                else
                {
                    return Json(new { msg = "部门不存在", success = false });
                }
            }
        }

        public class UserInfo
        {
            //用于返回前端的类
            public string name { get; set; }
            public string classification { get; set; }
        }




    }

}