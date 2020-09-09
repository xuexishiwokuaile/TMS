using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMS.EF;

namespace UI.Controllers
{
    public class HomeController : Controller
    {
        //过滤器，用于防止直接通过url跳过登录
        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (HttpContext.Session["userInfo"] == null || !(HttpContext.Session["userInfo"] is user))
            {
                filterContext.Result = new RedirectResult("../Login/Login");
            }
        }
        public ActionResult Index()//首页
        {
            return View();
        }


        #region 夹具管理
        public ActionResult Class()//夹具类表
        {
            return View();
        }
        public ActionResult BasicInfo()//夹具基础信息表
        {
            return View();
        }
        #endregion

        #region 采购管理
        public ActionResult Purchase()//采购
        {
            return View();
        }
        public ActionResult HandlePurchase()//处理采购
        {
            return View();
        }
        public ActionResult RatifyPurchase()//处理采购
        {
            return View();
        }
        #endregion

        #region 出入库管理
        public ActionResult Inout()//出入库管理
        {
            return View();
        }
        #endregion

        #region 报修管理
        public ActionResult Repair()//报修申请
        {
            return View();
        }
        public ActionResult HandleRepair()//处理报修
        {
            return View();
        }
        public ActionResult RatifyRepair()//批准报修
        {
            return View();
        }
        #endregion

        #region 报废管理
        public ActionResult RatifyScrap()//批准报废
        {
            return View();
        }
        public ActionResult Scrap()//提交报废申请
        {
            return View();
        }

        public ActionResult HandleScrap()//处理报废
        {
            return View();
        }
        #endregion

        #region 预警管理
        public ActionResult Data()//预警实时数据
        {
            return View();
        }
        public ActionResult Analysis()//分析
        {
            return View();
        }
        #endregion

        #region 个人管理
        public ActionResult Person()//个人信息
        {
            return View();
        }
        #endregion

        #region 管理员管理
        public ActionResult Admin()//管理员
        {
            return View();
        }
        #endregion

        #region 部门管理
        public ActionResult WorkCell()//部门管理
        {
            return View();
        }

        #endregion









    }
}