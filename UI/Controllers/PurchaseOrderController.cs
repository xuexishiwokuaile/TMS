using TMS.EF;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using UI.Models;

namespace UI.Controllers
{
    public class PurchaseOrderController : Controller
    {
        private IQueryable<PurchaseOrderTableModel> purchaseOrderQuery;

       
        public ActionResult HandlePurchase()//处理采购
        {
            return View();
        }

        public ActionResult RatifyPurchase()//审核采购
        {
            return View();
        }



        public ActionResult GetPurchaseOrderList(int page, int limit)
        {
            using (TMSDbContext entities = new TMSDbContext())
            {
                purchaseOrderQuery = from A in entities.purchase_order
                                     join B1 in entities.user on A.proposerId equals B1.id
                                     join B2 in entities.user on A.supervisorId equals B2.id into dc
                                     from dci in dc.DefaultIfEmpty()
                                     join B3 in entities.user on A.managerId equals B3.id into ec
                                     from eci in ec.DefaultIfEmpty()
                                     select new PurchaseOrderTableModel
                                     {
                                         purchaseId = A.purchaseId,
                                         proposerId = A.proposerId,
                                         typeId = A.typeId,
                                         toolId = A.toolId,
                                         supervisorId = A.supervisorId,
                                         managerId = A.managerId,
                                         state = A.state,
                                         code = A.code,
                                         proposerName = B1.name,
                                         supervisorName = dci.name,
                                         managerName = eci.name
                                     };

                var pageQuery = purchaseOrderQuery.OrderByDescending(p => p.purchaseId).Skip(limit * (page - 1)).Take(limit).ToList();

                var result = new
                {
                    code = 0,
                    msg = "",
                    count = purchaseOrderQuery.Count(),
                    data = pageQuery
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult GetPurchaseOrderListPending(int page, int limit)
        {
            using (TMSDbContext entities = new TMSDbContext())
            {
                purchaseOrderQuery = from A in entities.purchase_order
                                     join B1 in entities.user on A.proposerId equals B1.id
                                     join B2 in entities.user on A.supervisorId equals B2.id into dc
                                     from dci in dc.DefaultIfEmpty()
                                     join B3 in entities.user on A.managerId equals B3.id into ec
                                     from eci in ec.DefaultIfEmpty()
                                     where A.state == -1
                                     select new PurchaseOrderTableModel
                                     {
                                         purchaseId = A.purchaseId,
                                         proposerId = A.proposerId,
                                         typeId = A.typeId,
                                         toolId = A.toolId,
                                         supervisorId = A.supervisorId,
                                         managerId = A.managerId,
                                         state = A.state,
                                         code = A.code,
                                         proposerName = B1.name,
                                         supervisorName = dci.name,
                                         managerName = eci.name
                                     };

                var pageQuery = purchaseOrderQuery.OrderByDescending(p => p.purchaseId).Skip(limit * (page - 1)).Take(limit).ToList();

                var result = new
                {
                    code = 0,
                    msg = "",
                    count = purchaseOrderQuery.Count(),
                    data = pageQuery
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult GetPurchaseOrderListResolved(int page, int limit)
        {
            using (TMSDbContext entities = new TMSDbContext())
            {
                purchaseOrderQuery = from A in entities.purchase_order
                                     join B1 in entities.user on A.proposerId equals B1.id
                                     join B2 in entities.user on A.supervisorId equals B2.id into dc
                                     from dci in dc.DefaultIfEmpty()
                                     join B3 in entities.user on A.managerId equals B3.id into ec
                                     from eci in ec.DefaultIfEmpty()
                                     where A.state == 0
                                     select new PurchaseOrderTableModel
                                     {
                                         purchaseId = A.purchaseId,
                                         proposerId = A.proposerId,
                                         typeId = A.typeId,
                                         toolId = A.toolId,
                                         supervisorId = A.supervisorId,
                                         managerId = A.managerId,
                                         state = A.state,
                                         code = A.code,
                                         proposerName = B1.name,
                                         supervisorName = dci.name,
                                         managerName = eci.name
                                     };

                var pageQuery = purchaseOrderQuery.OrderByDescending(p => p.purchaseId).Skip(limit * (page - 1)).Take(limit).ToList();

                var result = new
                {
                    code = 0,
                    msg = "",
                    count = purchaseOrderQuery.Count(),
                    data = pageQuery
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult GetPurchaseOrderListRejected(int page, int limit)
        {
            using (TMSDbContext entities = new TMSDbContext())
            {
                purchaseOrderQuery = from A in entities.purchase_order
                                     join B1 in entities.user on A.proposerId equals B1.id
                                     join B2 in entities.user on A.supervisorId equals B2.id into dc
                                     from dci in dc.DefaultIfEmpty()
                                     join B3 in entities.user on A.managerId equals B3.id into ec
                                     from eci in ec.DefaultIfEmpty()
                                     where A.state == 2
                                     select new PurchaseOrderTableModel
                                     {
                                         purchaseId = A.purchaseId,
                                         proposerId = A.proposerId,
                                         typeId = A.typeId,
                                         toolId = A.toolId,
                                         supervisorId = A.supervisorId,
                                         managerId = A.managerId,
                                         state = A.state,
                                         code = A.code,
                                         proposerName = B1.name,
                                         supervisorName = dci.name,
                                         managerName = eci.name
                                     };

                var pageQuery = purchaseOrderQuery.OrderByDescending(p => p.purchaseId).Skip(limit * (page - 1)).Take(limit).ToList();

                var result = new
                {
                    code = 0,
                    msg = "",
                    count = purchaseOrderQuery.Count(),
                    data = pageQuery
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult BatchDelPurchaseOrders(List<string> purchaseIds)
        {
            using (TMSDbContext context = new TMSDbContext())
            {
                var delPurchaseOrderQuery = context.purchase_order.Where(t => purchaseIds.Contains(t.purchaseId.ToString()));
                context.purchase_order.RemoveRange(delPurchaseOrderQuery);

                if (context.SaveChanges() > 0)
                {
                    return Json(new
                    {
                        Success = true,
                        Message = "删除成功"
                    });
                }
                return Json(new
                {
                    Success = false,
                    Message = "删除失败"
                });
            }

        }

        public ActionResult BatchHandlePassPurchaseOrders(List<string> purchaseIds)
        {
            using (TMSDbContext context = new TMSDbContext())
            {
                var purchase = context.purchase_order.Where(t => purchaseIds.Contains(t.purchaseId.ToString())).ToList();
                foreach (var t in purchase)
                {
                    t.state = 0;
                }

                if (context.SaveChanges() > 0)
                {
                    return Json(new
                    {
                        Success = true,
                        Message = "处理成功"
                    });
                }
                return Json(new
                {
                    Success = false,
                    Message = "处理失败"
                });
            }

        }

        public ActionResult BatchHandleFailPurchaseOrders(List<string> purchaseIds)
        {
            using (TMSDbContext context = new TMSDbContext())
            {
                var purchase = context.purchase_order.Where(t => purchaseIds.Contains(t.purchaseId.ToString())).ToList();
                foreach (var t in purchase)
                {
                    t.state = 2;
                }

                if (context.SaveChanges() > 0)
                {
                    return Json(new
                    {
                        Success = true,
                        Message = "处理成功"
                    });
                }
                return Json(new
                {
                    Success = false,
                    Message = "处理失败"
                });
            }

        }

        public ActionResult GetPurchaseRatifyOrderListPending(int page, int limit)
        {
            using (TMSDbContext entities = new TMSDbContext())
            {
                purchaseOrderQuery = from A in entities.purchase_order
                                     join B1 in entities.user on A.proposerId equals B1.id
                                     join B2 in entities.user on A.supervisorId equals B2.id into dc
                                     from dci in dc.DefaultIfEmpty()
                                     join B3 in entities.user on A.managerId equals B3.id into ec
                                     from eci in ec.DefaultIfEmpty()
                                     where A.state == 0
                                     select new PurchaseOrderTableModel
                                     {
                                         purchaseId = A.purchaseId,
                                         proposerId = A.proposerId,
                                         typeId = A.typeId,
                                         toolId = A.toolId,
                                         supervisorId = A.supervisorId,
                                         managerId = A.managerId,
                                         state = A.state,
                                         code = A.code,
                                         proposerName = B1.name,
                                         supervisorName = dci.name,
                                         managerName = eci.name
                                     };

                var pageQuery = purchaseOrderQuery.OrderByDescending(p => p.purchaseId).Skip(limit * (page - 1)).Take(limit).ToList();

                var result = new
                {
                    code = 0,
                    msg = "",
                    count = purchaseOrderQuery.Count(),
                    data = pageQuery
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult GetPurchaseRatifyOrderListResolved(int page, int limit)
        {
            using (TMSDbContext entities = new TMSDbContext())
            {
                purchaseOrderQuery = from A in entities.purchase_order
                                     join B1 in entities.user on A.proposerId equals B1.id
                                     join B2 in entities.user on A.supervisorId equals B2.id into dc
                                     from dci in dc.DefaultIfEmpty()
                                     join B3 in entities.user on A.managerId equals B3.id into ec
                                     from eci in ec.DefaultIfEmpty()
                                     where A.state == 1
                                     select new PurchaseOrderTableModel
                                     {
                                         purchaseId = A.purchaseId,
                                         proposerId = A.proposerId,
                                         typeId = A.typeId,
                                         toolId = A.toolId,
                                         supervisorId = A.supervisorId,
                                         managerId = A.managerId,
                                         state = A.state,
                                         code = A.code,
                                         proposerName = B1.name,
                                         supervisorName = dci.name,
                                         managerName = eci.name
                                     };

                var pageQuery = purchaseOrderQuery.OrderByDescending(p => p.purchaseId).Skip(limit * (page - 1)).Take(limit).ToList();

                var result = new
                {
                    code = 0,
                    msg = "",
                    count = purchaseOrderQuery.Count(),
                    data = pageQuery
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult GetPurchaseRatifyOrderListRejected(int page, int limit)
        {
            using (TMSDbContext entities = new TMSDbContext())
            {
                purchaseOrderQuery = from A in entities.purchase_order
                                     join B1 in entities.user on A.proposerId equals B1.id
                                     join B2 in entities.user on A.supervisorId equals B2.id into dc
                                     from dci in dc.DefaultIfEmpty()
                                     join B3 in entities.user on A.managerId equals B3.id into ec
                                     from eci in ec.DefaultIfEmpty()
                                     where A.state == 3
                                     select new PurchaseOrderTableModel
                                     {
                                         purchaseId = A.purchaseId,
                                         proposerId = A.proposerId,
                                         typeId = A.typeId,
                                         toolId = A.toolId,
                                         supervisorId = A.supervisorId,
                                         managerId = A.managerId,
                                         state = A.state,
                                         code = A.code,
                                         proposerName = B1.name,
                                         supervisorName = dci.name,
                                         managerName = eci.name
                                     };

                var pageQuery = purchaseOrderQuery.OrderByDescending(p => p.purchaseId).Skip(limit * (page - 1)).Take(limit).ToList();

                var result = new
                {
                    code = 0,
                    msg = "",
                    count = purchaseOrderQuery.Count(),
                    data = pageQuery
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult BatchRatifyPassPurchaseOrders(List<string> purchaseIds)
        {
            using (TMSDbContext context = new TMSDbContext())
            {
                var purchase = context.purchase_order.Where(t => purchaseIds.Contains(t.purchaseId.ToString())).ToList();
                foreach (var t in purchase)
                {
                    t.state = 1;
                }

                if (context.SaveChanges() > 0)
                {
                    return Json(new
                    {
                        Success = true,
                        Message = "处理成功"
                    });
                }
                return Json(new
                {
                    Success = false,
                    Message = "处理失败"
                });
            }

        }

        public ActionResult BatchRatifyFailPurchaseOrders(List<string> purchaseIds)
        {
            using (TMSDbContext context = new TMSDbContext())
            {
                var purchase = context.purchase_order.Where(t => purchaseIds.Contains(t.purchaseId.ToString())).ToList();
                foreach (var t in purchase)
                {
                    t.state = 3;
                }

                if (context.SaveChanges() > 0)
                {
                    return Json(new
                    {
                        Success = true,
                        Message = "处理成功"
                    });
                }
                return Json(new
                {
                    Success = false,
                    Message = "处理失败"
                });
            }

        }

    }
}