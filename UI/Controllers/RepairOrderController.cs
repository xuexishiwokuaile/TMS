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
    public class RepairOrderController : Controller
    {
        private IQueryable<RepairOrderTableModel> repairOrderQuery;


        public ActionResult HandleRepair()//处理保修
        {
            return View();
        }


        public ActionResult RatifyRepair()//批准报修
        {
            return View();
        }


        public ActionResult GetRepairOrderList(int page, int limit)
        {
            using (TMSDbContext entities = new TMSDbContext())
            {
                repairOrderQuery = from A in entities.repair_order
                                   join B in entities.user on A.proposerId equals B.id
                                   select new RepairOrderTableModel
                                   {
                                       repairOrderId = A.repairOrderId,
                                       proposerId = A.proposerId,
                                       toolId = A.toolId,
                                       faultPicUrl = A.faultPicUrl,
                                       dealerId = A.dealerId,
                                       description = A.description,
                                       state = A.state,
                                       name = B.name
                                   };
                //query = entities.repair_order.AsQueryable();

                var pageQuery = repairOrderQuery.OrderByDescending(p => p.repairOrderId).Skip(limit * (page - 1)).Take(limit).ToList();

                var result = new
                {
                    code = 0,
                    msg = "",
                    count = repairOrderQuery.Count(),
                    data = pageQuery
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult GetRepairOrderListPending(int page, int limit)
        {
            using (TMSDbContext entities = new TMSDbContext())
            {
                repairOrderQuery = from A in entities.repair_order
                                   join B in entities.user on A.proposerId equals B.id
                                   where A.state == -1
                                   select new RepairOrderTableModel
                                   {
                                       repairOrderId = A.repairOrderId,
                                       proposerId = A.proposerId,
                                       toolId = A.toolId,
                                       faultPicUrl = A.faultPicUrl,
                                       dealerId = A.dealerId,
                                       description = A.description,
                                       state = A.state,
                                       name = B.name
                                   };
                //query = entities.repair_order.AsQueryable();

                var pageQuery = repairOrderQuery.OrderByDescending(p => p.repairOrderId).Skip(limit * (page - 1)).Take(limit).ToList();

                var result = new
                {
                    code = 0,
                    msg = "",
                    count = repairOrderQuery.Count(),
                    data = pageQuery
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult GetRepairOrderListResolved(int page, int limit)
        {
            using (TMSDbContext entities = new TMSDbContext())
            {
                repairOrderQuery = from A in entities.repair_order
                                   join B in entities.user on A.proposerId equals B.id
                                   where A.state == 0
                                   select new RepairOrderTableModel
                                   {
                                       repairOrderId = A.repairOrderId,
                                       proposerId = A.proposerId,
                                       toolId = A.toolId,
                                       faultPicUrl = A.faultPicUrl,
                                       dealerId = A.dealerId,
                                       description = A.description,
                                       state = A.state,
                                       name = B.name
                                   };
                //query = entities.repair_order.AsQueryable();

                var pageQuery = repairOrderQuery.OrderByDescending(p => p.repairOrderId).Skip(limit * (page - 1)).Take(limit).ToList();

                var result = new
                {
                    code = 0,
                    msg = "",
                    count = repairOrderQuery.Count(),
                    data = pageQuery
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult GetRepairOrderListRejected(int page, int limit)
        {
            using (TMSDbContext entities = new TMSDbContext())
            {
                repairOrderQuery = from A in entities.repair_order
                                   join B in entities.user on A.proposerId equals B.id
                                   where A.state == 2
                                   select new RepairOrderTableModel
                                   {
                                       repairOrderId = A.repairOrderId,
                                       proposerId = A.proposerId,
                                       toolId = A.toolId,
                                       faultPicUrl = A.faultPicUrl,
                                       dealerId = A.dealerId,
                                       description = A.description,
                                       state = A.state,
                                       name = B.name
                                   };
                //query = entities.repair_order.AsQueryable();

                var pageQuery = repairOrderQuery.OrderByDescending(p => p.repairOrderId).Skip(limit * (page - 1)).Take(limit).ToList();

                var result = new
                {
                    code = 0,
                    msg = "",
                    count = repairOrderQuery.Count(),
                    data = pageQuery
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult BatchDelRepairOrders(List<string> repairOrderIds)
        {
            using (TMSDbContext context = new TMSDbContext())
            {
                var delRepairOrderQuery = context.repair_order.Where(t => repairOrderIds.Contains(t.repairOrderId.ToString()));
                context.repair_order.RemoveRange(delRepairOrderQuery);

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

        public ActionResult BatchHandlePassRepairOrders(List<string> repairOrderIds)
        {
            using (TMSDbContext context = new TMSDbContext())
            {
                var repair = context.repair_order.Where(t => repairOrderIds.Contains(t.repairOrderId.ToString())).ToList();
                foreach (var t in repair)
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

        public ActionResult BatchHandleFailRepairOrders(List<string> repairOrderIds)
        {
            using (TMSDbContext context = new TMSDbContext())
            {
                var repair = context.repair_order.Where(t => repairOrderIds.Contains(t.repairOrderId.ToString())).ToList();
                foreach (var t in repair)
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



        public ActionResult BatchRatifyPassRepairOrders(List<string> repairOrderIds)
        {
            using (TMSDbContext context = new TMSDbContext())
            {
                var repair = context.repair_order.Where(t => repairOrderIds.Contains(t.repairOrderId.ToString())).ToList();
                foreach (var t in repair)
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

        public ActionResult BatchRatifyFailRepairOrders(List<string> repairOrderIds)
        {
            using (TMSDbContext context = new TMSDbContext())
            {
                var repair = context.repair_order.Where(t => repairOrderIds.Contains(t.repairOrderId.ToString())).ToList();
                foreach(var t in repair)
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

        public ActionResult GetRepairRatifyOrderListPending(int page, int limit)
        {
            using (TMSDbContext entities = new TMSDbContext())
            {
                repairOrderQuery = from A in entities.repair_order
                                   join B in entities.user on A.proposerId equals B.id
                                   where A.state == 0
                                   select new RepairOrderTableModel
                                   {
                                       repairOrderId = A.repairOrderId,
                                       proposerId = A.proposerId,
                                       toolId = A.toolId,
                                       faultPicUrl = A.faultPicUrl,
                                       description = A.description,
                                       state = A.state,
                                       name = B.name
                                   };
                //query = entities.repair_order.AsQueryable();

                var pageQuery = repairOrderQuery.OrderByDescending(p => p.repairOrderId).Skip(limit * (page - 1)).Take(limit).ToList();

                var result = new
                {
                    code = 0,
                    msg = "",
                    count = repairOrderQuery.Count(),
                    data = pageQuery
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult GetRepairRatifyOrderListResolved(int page, int limit)
        {
            using (TMSDbContext entities = new TMSDbContext())
            {
                repairOrderQuery = from A in entities.repair_order
                                   join B in entities.user on A.proposerId equals B.id
                                   where A.state == 1
                                   select new RepairOrderTableModel
                                   {
                                       repairOrderId = A.repairOrderId,
                                       proposerId = A.proposerId,
                                       toolId = A.toolId,
                                       faultPicUrl = A.faultPicUrl,
                                       description = A.description,
                                       state = A.state,
                                       name = B.name
                                   };
                //query = entities.repair_order.AsQueryable();

                var pageQuery = repairOrderQuery.OrderByDescending(p => p.repairOrderId).Skip(limit * (page - 1)).Take(limit).ToList();

                var result = new
                {
                    code = 0,
                    msg = "",
                    count = repairOrderQuery.Count(),
                    data = pageQuery
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }



        }

        public ActionResult GetRepairRatifyOrderListRejected(int page, int limit)
        {
            using (TMSDbContext entities = new TMSDbContext())
            {
                repairOrderQuery = from A in entities.repair_order
                                   join B in entities.user on A.proposerId equals B.id
                                   where A.state == 3
                                   select new RepairOrderTableModel
                                   {
                                       repairOrderId = A.repairOrderId,
                                       proposerId = A.proposerId,
                                       toolId = A.toolId,
                                       faultPicUrl = A.faultPicUrl,
                                       description = A.description,
                                       state = A.state,
                                       name = B.name
                                   };
                //query = entities.repair_order.AsQueryable();

                var pageQuery = repairOrderQuery.OrderByDescending(p => p.repairOrderId).Skip(limit * (page - 1)).Take(limit).ToList();

                var result = new
                {
                    code = 0,
                    msg = "",
                    count = repairOrderQuery.Count(),
                    data = pageQuery
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }



        }
    }
}