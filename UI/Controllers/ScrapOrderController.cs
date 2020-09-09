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
    public class ScrapOrderController : Controller
    {
        private IQueryable<ScrapOrderTableModel> scrapOrderQuery;

        
        public ActionResult HandleScrap()//处理报废
        {
            return View();
        }

        
        public ActionResult RatifyScrap()//批准报废
        {
            return View();
        }


        public ActionResult GetScrapOrderList(int page, int limit)
        {
            using (TMSDbContext entities = new TMSDbContext())
            {
                scrapOrderQuery = from A in entities.scrap_order
                                  join B1 in entities.user on A.proposerId equals B1.id
                                  join B2 in entities.user on A.supervisorId equals B2.id into dc
                                  from dci in dc.DefaultIfEmpty()
                                  join B3 in entities.user on A.managerId equals B3.id into ec
                                  from eci in ec.DefaultIfEmpty()
                                  select new ScrapOrderTableModel
                                  {
                                      scrapId = A.scrapId,
                                      proposerId = A.proposerId,
                                      supervisorId = A.supervisorId,
                                      managerId = A.managerId,
                                      toolId = A.toolId,
                                      lifeCount = A.lifeCount,
                                      description = A.description,
                                      state = A.state,
                                      proposerName = B1.name,
                                      supervisorName = dci.name,
                                      managerName = eci.name
                                  };

                var pageQuery = scrapOrderQuery.OrderByDescending(p => p.scrapId).Skip(limit * (page - 1)).Take(limit).ToList();

                var result = new
                {
                    code = 0,
                    msg = "",
                    count = scrapOrderQuery.Count(),
                    data = pageQuery
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult GetScrapOrderListPending(int page, int limit)
        {
            using (TMSDbContext entities = new TMSDbContext())
            {
                scrapOrderQuery = from A in entities.scrap_order
                                  join B1 in entities.user on A.proposerId equals B1.id
                                  join B2 in entities.user on A.supervisorId equals B2.id into dc
                                  from dci in dc.DefaultIfEmpty()
                                  join B3 in entities.user on A.managerId equals B3.id into ec
                                  from eci in ec.DefaultIfEmpty()
                                  where A.state == -1
                                  select new ScrapOrderTableModel
                                  {
                                      scrapId = A.scrapId,
                                      proposerId = A.proposerId,
                                      supervisorId = A.supervisorId,
                                      managerId = A.managerId,
                                      toolId = A.toolId,
                                      lifeCount = A.lifeCount,
                                      description = A.description,
                                      state = A.state,
                                      proposerName = B1.name,
                                      supervisorName = dci.name,
                                      managerName = eci.name
                                  };

                var pageQuery = scrapOrderQuery.OrderByDescending(p => p.scrapId).Skip(limit * (page - 1)).Take(limit).ToList();

                var result = new
                {
                    code = 0,
                    msg = "",
                    count = scrapOrderQuery.Count(),
                    data = pageQuery
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult GetScrapOrderListResolved(int page, int limit)
        {
            using (TMSDbContext entities = new TMSDbContext())
            {
                scrapOrderQuery = from A in entities.scrap_order
                                  join B1 in entities.user on A.proposerId equals B1.id
                                  join B2 in entities.user on A.supervisorId equals B2.id into dc
                                  from dci in dc.DefaultIfEmpty()
                                  join B3 in entities.user on A.managerId equals B3.id into ec
                                  from eci in ec.DefaultIfEmpty()
                                  where A.state == 0
                                  select new ScrapOrderTableModel
                                  {
                                      scrapId = A.scrapId,
                                      proposerId = A.proposerId,
                                      supervisorId = A.supervisorId,
                                      managerId = A.managerId,
                                      toolId = A.toolId,
                                      lifeCount = A.lifeCount,
                                      description = A.description,
                                      state = A.state,
                                      proposerName = B1.name,
                                      supervisorName = dci.name,
                                      managerName = eci.name
                                  };

                var pageQuery = scrapOrderQuery.OrderByDescending(p => p.scrapId).Skip(limit * (page - 1)).Take(limit).ToList();

                var result = new
                {
                    code = 0,
                    msg = "",
                    count = scrapOrderQuery.Count(),
                    data = pageQuery
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult GetScrapOrderListRejected(int page, int limit)
        {
            using (TMSDbContext entities = new TMSDbContext())
            {
                scrapOrderQuery = from A in entities.scrap_order
                                  join B1 in entities.user on A.proposerId equals B1.id
                                  join B2 in entities.user on A.supervisorId equals B2.id into dc
                                  from dci in dc.DefaultIfEmpty()
                                  join B3 in entities.user on A.managerId equals B3.id into ec
                                  from eci in ec.DefaultIfEmpty()
                                  where A.state == 2
                                  select new ScrapOrderTableModel
                                  {
                                      scrapId = A.scrapId,
                                      proposerId = A.proposerId,
                                      supervisorId = A.supervisorId,
                                      managerId = A.managerId,
                                      toolId = A.toolId,
                                      lifeCount = A.lifeCount,
                                      description = A.description,
                                      state = A.state,
                                      proposerName = B1.name,
                                      supervisorName = dci.name,
                                      managerName = eci.name
                                  };

                var pageQuery = scrapOrderQuery.OrderByDescending(p => p.scrapId).Skip(limit * (page - 1)).Take(limit).ToList();

                var result = new
                {
                    code = 0,
                    msg = "",
                    count = scrapOrderQuery.Count(),
                    data = pageQuery
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult BatchDelScrapOrders(List<string> scrapIds)
        {
            using (TMSDbContext context = new TMSDbContext())
            {
                var delScrapOrderQuery = context.scrap_order.Where(t => scrapIds.Contains(t.scrapId.ToString()));
                context.scrap_order.RemoveRange(delScrapOrderQuery);

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

        public ActionResult BatchHandlePassScrapOrders(List<string> scrapIds)
        {
            using (TMSDbContext context = new TMSDbContext())
            {
                var scrap = context.scrap_order.Where(t => scrapIds.Contains(t.scrapId.ToString())).ToList();
                foreach (var t in scrap)
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

        public ActionResult BatchHandleFailScrapOrders(List<string> scrapIds)
        {
            using (TMSDbContext context = new TMSDbContext())
            {
                var scrap = context.scrap_order.Where(t => scrapIds.Contains(t.scrapId.ToString())).ToList();
                foreach (var t in scrap)
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


        public ActionResult GetScrapRatifyOrderListPending(int page, int limit)
        {
            using (TMSDbContext entities = new TMSDbContext())
            {
                scrapOrderQuery = from A in entities.scrap_order
                                  join B1 in entities.user on A.proposerId equals B1.id
                                  join B2 in entities.user on A.supervisorId equals B2.id into dc
                                  from dci in dc.DefaultIfEmpty()
                                  join B3 in entities.user on A.managerId equals B3.id into ec
                                  from eci in ec.DefaultIfEmpty()
                                  where A.state == 0
                                  select new ScrapOrderTableModel
                                  {
                                      scrapId = A.scrapId,
                                      proposerId = A.proposerId,
                                      supervisorId = A.supervisorId,
                                      managerId = A.managerId,
                                      toolId = A.toolId,
                                      lifeCount = A.lifeCount,
                                      description = A.description,
                                      state = A.state,
                                      proposerName = B1.name,
                                      supervisorName = dci.name,
                                      managerName = eci.name
                                  };

                var pageQuery = scrapOrderQuery.OrderByDescending(p => p.scrapId).Skip(limit * (page - 1)).Take(limit).ToList();

                var result = new
                {
                    code = 0,
                    msg = "",
                    count = scrapOrderQuery.Count(),
                    data = pageQuery
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult GetScrapRatifyOrderListResolved(int page, int limit)
        {
            using (TMSDbContext entities = new TMSDbContext())
            {
                scrapOrderQuery = from A in entities.scrap_order
                                  join B1 in entities.user on A.proposerId equals B1.id
                                  join B2 in entities.user on A.supervisorId equals B2.id into dc
                                  from dci in dc.DefaultIfEmpty()
                                  join B3 in entities.user on A.managerId equals B3.id into ec
                                  from eci in ec.DefaultIfEmpty()
                                  where A.state == 1
                                  select new ScrapOrderTableModel
                                  {
                                      scrapId = A.scrapId,
                                      proposerId = A.proposerId,
                                      supervisorId = A.supervisorId,
                                      managerId = A.managerId,
                                      toolId = A.toolId,
                                      lifeCount = A.lifeCount,
                                      description = A.description,
                                      state = A.state,
                                      proposerName = B1.name,
                                      supervisorName = dci.name,
                                      managerName =eci.name
                                  };

                var pageQuery = scrapOrderQuery.OrderByDescending(p => p.scrapId).Skip(limit * (page - 1)).Take(limit).ToList();

                var result = new
                {
                    code = 0,
                    msg = "",
                    count = scrapOrderQuery.Count(),
                    data = pageQuery
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult GetScrapRatifyOrderListRejected(int page, int limit)
        {
            using (TMSDbContext entities = new TMSDbContext())
            {
                scrapOrderQuery = from A in entities.scrap_order
                                  join B1 in entities.user on A.proposerId equals B1.id
                                  join B2 in entities.user on A.supervisorId equals B2.id into dc
                                  from dci in dc.DefaultIfEmpty()
                                  join B3 in entities.user on A.managerId equals B3.id into ec
                                  from eci in ec.DefaultIfEmpty()
                                  where A.state == 3
                                  select new ScrapOrderTableModel
                                  {
                                      scrapId = A.scrapId,
                                      proposerId = A.proposerId,
                                      supervisorId = A.supervisorId,
                                      managerId = A.managerId,
                                      toolId = A.toolId,
                                      lifeCount = A.lifeCount,
                                      description = A.description,
                                      state = A.state,
                                      proposerName = B1.name,
                                      supervisorName = dci.name,
                                      managerName = eci.name
                                  };

                var pageQuery = scrapOrderQuery.OrderByDescending(p => p.scrapId).Skip(limit * (page - 1)).Take(limit).ToList();

                var result = new
                {
                    code = 0,
                    msg = "",
                    count = scrapOrderQuery.Count(),
                    data = pageQuery
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult BatchRatifyPassScrapOrders(List<string> scrapIds)
        {
            using (TMSDbContext context = new TMSDbContext())
            {
                var scrap = context.scrap_order.Where(t => scrapIds.Contains(t.scrapId.ToString())).ToList();
                foreach (var t in scrap)
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

        public ActionResult BatchRatifyFailScrapOrders(List<string> scrapIds)
        {
            using (TMSDbContext context = new TMSDbContext())
            {
                var scrap = context.scrap_order.Where(t => scrapIds.Contains(t.scrapId.ToString())).ToList();
                foreach (var t in scrap)
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