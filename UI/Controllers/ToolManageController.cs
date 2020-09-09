using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMS.EF;
using UI.Models;
using System.Net.Mail;
using System.Data;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Text;

namespace UI.Controllers
{
    public class ToolManageController : Controller
    {
        // GET: ToolManage

        private IQueryable<tool_type> query;
        private IQueryable<inoutListModel> manuList;
        private IQueryable<RepairOrderTableModel> repairOrderQuery;
        private IQueryable<PurchaseOrderTableModel> purchaseOrderQuery;
        private IQueryable<ScrapOrderTableModel> scrapOrderQuery;

        //private IQueryable<scrap_order> scrapOrderQuery;

        //private IQueryable<ScrapOrderTableModel> scrapOrderQuery;

        //*******************************************夹具类管理class********************************************************//
        public ActionResult GetToolTypeList(int page, int limit)
        {
            using (TMSDbContext entities = new TMSDbContext())
            {

                if (Request.QueryString.Get("keyword").Equals(""))
                {
                    query = entities.tool_type.AsQueryable();
                }
                else
                {
                    String keyword = Request.QueryString.Get("keyword");
                    query = entities.tool_type.Where(u => (u.code.Contains(keyword) || (u.model.Contains(keyword)) || (u.name.Contains(keyword)) ||
                    (u.partNo.Contains(keyword)) || (u.usedFor.Contains(keyword)))).AsQueryable();

                }
                var pageQuery = query.OrderByDescending(a => a.typeId).Skip(limit * (page - 1)).Take(limit).ToList();
                //layui的特定格式
                var result = new
                {
                    code = 0,
                    msg = "",
                    count = query.Count(),
                    data = pageQuery
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        //已经用GetToolTypeList代替
        //public ActionResult SearchToolType(int page, int limit)
        //{
        //    using(ToolManageDbContext context = new ToolManageDbContext())
        //    {
        //        //获得的keyword前有一个","，用substring去除
        //        String keyword = Request.QueryString.Get("keyword").Substring(1);
        //        var query = context.tool_type.Where(u => u.code.Contains(keyword)).OrderBy(u =>u.typeId).Skip(limit * (page - 1)).Take(limit).ToList();
        //        var result = new
        //        {
        //            code = 0,
        //            msg = "",
        //            count = query.Count(),
        //            data = query
        //        };
        //        return Json(result, JsonRequestBehavior.AllowGet);
        //    }
        //}
        public ActionResult ToolTypeDetail(Guid? typeId)
        {
            if (typeId == null)
            {
                return View();
            }
            using (TMSDbContext entities = new TMSDbContext())
            {
                tool_type tool_Type = entities.tool_type.FirstOrDefault(u => u.typeId == typeId);
                ViewBag.tool_Type = Newtonsoft.Json.JsonConvert.SerializeObject(tool_Type);
                return View();
            }
        }

        public ActionResult SubToolTypeDetail(ToolTypeDetailModel toolType)
        {
            using (TMSDbContext context = new TMSDbContext())
            {
                tool_type editToolType = context.tool_type.FirstOrDefault(t => t.typeId == toolType.typeId);
                if (editToolType == null)
                {
                    tool_type addTool_Type = new tool_type()
                    {
                        typeId = Guid.NewGuid(),
                        code = toolType.code,
                        name = toolType.name,
                        model = toolType.model,
                        partNo = toolType.partNo,
                        usedFor = toolType.usedFor,
                        ulp = toolType.ulp,
                        pmPeriod = toolType.pmPeriod
                    };
                    context.tool_type.Add(addTool_Type);
                }
                else
                {
                    editToolType.code = toolType.code;
                    editToolType.name = toolType.name;
                    editToolType.model = toolType.model;
                    editToolType.partNo = toolType.partNo;
                    editToolType.usedFor = toolType.usedFor;
                    editToolType.ulp = toolType.ulp;
                    editToolType.pmPeriod = toolType.pmPeriod;
                }
                int flg = context.SaveChanges();
                if (flg > 0)
                {
                    return Json(new
                    {
                        Success = true,
                        Message = "操作成功"
                    });
                }
                return Json(new
                {
                    Success = false,
                    Message = "操作失败"
                });
            }
        }

        public ActionResult DelToolType(Guid? typeId)
        {
            using (TMSDbContext context = new TMSDbContext())
            {
                tool_type tool_Type = context.tool_type.FirstOrDefault(u => u.typeId == typeId);
                context.tool_type.Remove(tool_Type);
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

        public ActionResult BatchDelToolTypes(List<Guid> typeIds)
        {
            using (TMSDbContext context = new TMSDbContext())
            {
                var delToolTypeQuery = context.tool_type.Where(t => typeIds.Contains(t.typeId));
                context.tool_type.RemoveRange(delToolTypeQuery);
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

        //*********************************************夹具类出入库inout******************************************************//

        public ActionResult GetToolList(int page, int limit)
        {
            using (TMSDbContext entities = new TMSDbContext())
            {
                if (Request.QueryString.Get("keyword").Equals(""))
                {
                    manuList = from A in entities.tool_type
                               join B in entities.tool on A.typeId equals B.typeId
                               select new inoutListModel
                               {
                                   toolId = B.toolId,
                                   typeId = B.typeId,
                                   code = A.code,
                                   seqId = B.seqId,
                                   billNo = B.billNo,
                                   operatorId = B.operatorId,
                                   regDate = B.regDate,
                                   usedCount = B.usedCount,
                                   location = B.location,
                                   proDate = B.proDate,
                                   picUrl = B.picUrl,
                                   _out = B._out
                               };
                }
                else
                {
                    string keyword = Request.QueryString.Get("keyword");
                    manuList = from A in entities.tool_type
                               join B in entities.tool on A.typeId equals B.typeId
                               where A.code.Contains(keyword)
                               select new inoutListModel
                               {
                                   toolId = B.toolId,
                                   typeId = B.typeId,
                                   code = A.code,
                                   seqId = B.seqId,
                                   billNo = B.billNo,
                                   operatorId = B.operatorId,
                                   regDate = B.regDate,
                                   usedCount = B.usedCount,
                                   location = B.location,
                                   proDate = B.proDate,
                                   picUrl = B.picUrl,
                                   _out = B._out
                               };
                }
                var pageQuery = manuList.OrderByDescending(a => a.toolId).Skip(limit * (page - 1)).Take(limit).ToList();
                //layui的特定格式
                var result = new
                {
                    code = 0,
                    msg = "",
                    count = manuList.Count(),
                    data = pageQuery
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        //此条用于新增和编辑Basicinfo
        public ActionResult newToolDetail(Guid? toolId)
        {
            return View();
        }


        public ActionResult editToolDetail(Guid? toolId)
        {
            if (toolId == null)
            {
                return View();
            }
            using (TMSDbContext entities = new TMSDbContext())
            {
                tool tool = entities.tool.FirstOrDefault(u => u.toolId == toolId);
                tool_type tool_Type = entities.tool_type.FirstOrDefault(t => t.typeId == tool.typeId);
                inoutListModel list = new inoutListModel()
                {
                    toolId = tool.toolId,
                    typeId = tool.typeId,
                    code = tool_Type.code,
                    seqId = tool.seqId,
                    billNo = tool.billNo,
                    operatorId = tool.operatorId,
                    regDate = tool.regDate,
                    usedCount = tool.usedCount,
                    location = tool.location,
                    proDate = tool.proDate,
                    picUrl = tool.picUrl,
                    _out = tool._out
                };
                ViewBag.tool = Newtonsoft.Json.JsonConvert.SerializeObject(list);
                return View();
            }
        }

        //以上为本条

        public ActionResult SubToolDetail(inoutListModel toolDetail, String picUrl)
        {
            using (TMSDbContext context = new TMSDbContext())
            {
                tool tool = context.tool.FirstOrDefault(t => t.toolId == toolDetail.toolId);
                //从夹具类表中根据code得到typeId
                tool_type tool_Type = context.tool_type.FirstOrDefault(u => u.typeId == toolDetail.toolType);
                if (tool == null)
                {

                    if (tool_Type == null)
                    {
                        return Json(new
                        {
                            Success = false,
                            Message = "不存在该夹具类"
                        });
                    }
                    else
                    {
                        tool addTool = new tool()
                        {
                            toolId = Guid.NewGuid(),
                            typeId = tool_Type.typeId,
                            seqId = toolDetail.seqId,
                            billNo = toolDetail.billNo,
                            operatorId = toolDetail.operatorId,
                            regDate = toolDetail.regDate,
                            usedCount = toolDetail.usedCount,
                            location = toolDetail.location,
                            proDate = toolDetail.proDate,
                            picUrl = picUrl,
                            _out = false,
                        };
                        context.tool.Add(addTool);
                    }
                }
                else//进行编辑
                {
                    tool tool1 = context.tool.FirstOrDefault(t => t.toolId == toolDetail.toolId);
                    //tool.typeId = tool_Type.typeId;//编辑时不应当修改工夹具种类
                    tool.seqId = toolDetail.seqId;
                    tool.billNo = toolDetail.billNo;
                    tool.operatorId = toolDetail.operatorId;
                    tool.regDate = toolDetail.regDate;
                    tool.usedCount = toolDetail.usedCount;
                    tool.location = toolDetail.location;
                    tool.proDate = toolDetail.proDate;
                    tool.picUrl = picUrl;
                    tool._out = toolDetail._out;
                }
                int flg = context.SaveChanges();
                if (flg > 0)
                {
                    return Json(new
                    {
                        Success = true,
                        Message = "操作成功"
                    });
                }
                return Json(new
                {
                    Success = false,
                    Message = "操作失败"
                });
            }
        }

        //上传图片
        
        
        public ActionResult UploadToolPic()
        {
            try
            {
                HttpFileCollectionBase files = Request.Files;
                HttpPostedFileBase file = files[0];
                //获取文件名后缀
                string extName = Path.GetExtension(file.FileName).ToLower();
                //获取保存目录的物理路径
                if (System.IO.Directory.Exists(Server.MapPath("/Images/Tool/")) == false)//如果不存在就创建images文件夹
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath("/Images/Tool/"));
                }
                string path = Server.MapPath("/Images/Tool/"); //path为某个文件夹的绝对路径，不要直接保存到数据库
                                                                 //    string path = "F:\\TgeoSmart\\Image\\";
                                                                 //生成新文件的名称，guid保证某一时刻内图片名唯一（文件不会被覆盖）
                string fileNewName = Guid.NewGuid().ToString();
                string ImageUrl = path + fileNewName + extName;
                //SaveAs将文件保存到指定文件夹中
                file.SaveAs(ImageUrl);
                //此路径为相对路径，只有把相对路径保存到数据库中图片才能正确显示（不加~为相对路径）
                string url = "\\Images\\Tool\\" + fileNewName + extName;
                return Json(new
                {
                    Result = true,
                    Data = url
                });
            }
            catch (Exception exception)
            {
                return Json(new
                {
                    Result = false,
                    exception.Message
                });
            }
        }

        public ActionResult Out(Guid? toolId)
        {
            using (TMSDbContext context = new TMSDbContext())
            {
                tool tool = context.tool.FirstOrDefault(u => u.toolId == toolId);
                tool._out = true;
                tool.usedCount += 1;
                if (context.SaveChanges() > 0)
                {
                    return Json(new
                    {

                        Success = true,
                        Message = "操作成功"
                    });
                }
                return Json(new
                {
                    Success = false,
                    Message = "操作失败"
                });
            }
        }

        public ActionResult In(Guid? toolId)
        {
            using (TMSDbContext context = new TMSDbContext())
            {
                tool tool = context.tool.FirstOrDefault(u => u.toolId == toolId);
                tool._out = false;
                if (context.SaveChanges() > 0)
                {
                    return Json(new
                    {

                        Success = true,
                        Message = "操作成功"
                    });
                }
                return Json(new
                {
                    Success = false,
                    Message = "操作失败"
                });
            }
        }

        public ActionResult BatchInTools(List<Guid> toolIds)
        {
            using (TMSDbContext context = new TMSDbContext())
            {
                int flag = 1;
                //检查是否全部可以执行入库操作，即全部都是true
                foreach (Guid t in toolIds)
                {
                    tool tool = context.tool.FirstOrDefault(u => u.toolId == t);
                    if (tool._out == false) flag = 0;
                }
                if (flag == 0)
                {
                    return Json(new
                    {
                        Success = false,
                        Message = "所选的工夹具不全为已出库状态"
                    });
                }
                else
                {
                    foreach (var t in toolIds)
                    {
                        tool tool = context.tool.FirstOrDefault(u => u.toolId == t);
                        tool._out = false;
                    }
                    if (context.SaveChanges() > 0)
                    {
                        return Json(new
                        {
                            Success = true,
                            Message = "批量出库成功"
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            Success = false,
                            Message = "批量出库失败"
                        });

                    }

                }

            }
        }

        public ActionResult BatchOutTools(List<Guid> toolIds)
        {
            using (TMSDbContext context = new TMSDbContext())
            {
                int flag = 1;
                //检查是否全部可以执行出库操作，即全部都是false
                foreach (Guid t in toolIds)
                {
                    tool tool = context.tool.FirstOrDefault(u => u.toolId == t);
                    if (tool._out == true) flag = 0;
                }
                if (flag == 0)
                {
                    return Json(new
                    {
                        Success = false,
                        Message = "所选的工夹具不全为在库中状态"
                    });
                }
                else
                {
                    foreach (var t in toolIds)
                    {
                        tool tool = context.tool.FirstOrDefault(u => u.toolId == t);
                        tool._out = true;
                    }
                    if (context.SaveChanges() > 0)
                    {
                        return Json(new
                        {
                            Success = true,
                            Message = "批量入库成功"
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            Success = false,
                            Message = "批量入库失败"
                        });

                    }

                }

            }
        }

        public ActionResult BatchDeleteTools(List<Guid> toolIds)
        {
            using (TMSDbContext context = new TMSDbContext())
            {
                var delToolTypeQuery = context.tool.Where(t => toolIds.Contains(t.toolId));
                context.tool.RemoveRange(delToolTypeQuery);
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


        //*********************************************采购管理purchase******************************************************//

        public ActionResult GetPurchaseOrderList(int page, int limit)
        {
            using (TMSDbContext entities = new TMSDbContext())
            {
                if (Request.QueryString.Get("keyword").Equals(""))
                {
                    //purchaseOrderQuery = entities.purchase_order.AsQueryable();

                    purchaseOrderQuery = from A in entities.purchase_order
                                         join B in entities.user on A.proposerId equals B.id
                                         select new PurchaseOrderTableModel
                                         {
                                             proposerName = B.name,
                                             purchaseId = A.purchaseId,
                                             proposerId = A.proposerId,
                                             typeId = A.typeId,
                                             toolId = A.toolId,
                                             supervisorId = A.supervisorId,
                                             managerId = A.managerId,
                                             state = A.state,
                                             code = A.code
                                         };

                }
                else
                {
                    string keyword = Request.QueryString.Get("keyword");

                    //purchaseOrderQuery = entities.purchase_order.Where(p => p.code.Contains(keyword)).AsQueryable();
                    purchaseOrderQuery = from A in entities.purchase_order
                                         join B in entities.user on A.proposerId equals B.id
                                         where A.code.Contains(keyword)
                                         select new PurchaseOrderTableModel
                                         {
                                             proposerName = B.name,
                                             purchaseId = A.purchaseId,
                                             proposerId = A.proposerId,
                                             typeId = A.typeId,
                                             toolId = A.toolId,
                                             supervisorId = A.supervisorId,
                                             managerId = A.managerId,
                                             state = A.state,
                                             code = A.code
                                         };
                }
                var pageQuery = purchaseOrderQuery.OrderByDescending(p => p.purchaseId).Skip(limit * (page - 1)).Take(limit).ToList();
                //layui的特定格式
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

        public ActionResult PurchaseOrderDetail(Guid? purchaseId)
        {
            if (purchaseId == null)
            {
                return View();
            }
            using (TMSDbContext entities = new TMSDbContext())
            {
                purchase_order purchase = entities.purchase_order.FirstOrDefault(u => u.purchaseId == purchaseId);
                ViewBag.purchase = Newtonsoft.Json.JsonConvert.SerializeObject(purchase);
                return View();
            }
        }

        //public ActionResult SearchPurchaseOrder()
        //{
        //    using (ToolManageDbContext entities = new ToolManageDbContext())
        //    {
        //        var keyword = Request.QueryString.Get("code");
        //        //************************************************************************************************
        //        //String s = "select * from purchase_order where purchaseId like '%@keyword%';";
        //        //var query = entities.purchase_order.SqlQuery(s,new SqlParameter("keyword", keyword));
        //        //String s = "select * from purchase_order where purchaseId like '%" + keyword + "%';";
        //        //var query = entities.purchase_order.SqlQuery(s);
        //        //entities.tool.Where(u=)
        //        //var query = entities.purchase_order.Where(u => u.state != 1);
        //        //query = query.Where(u => u.managerId.Con)
        //        var pageQuery = query.OrderByDescending(a => a.purchaseId).ToList();
        //        //layui的特定格式
        //        var result = new
        //        {
        //            code = 0,
        //            msg = "",
        //            count = query.Count(),
        //            data = pageQuery
        //        };
        //        return Json(result, JsonRequestBehavior.AllowGet);
        //    }
        //}

        public ActionResult SubPurchaseOrderDetail(PurchaseOrderDetailModel purchase)
        {
            using (TMSDbContext context = new TMSDbContext())
            {
                purchase_order purchaseOrder = context.purchase_order.FirstOrDefault(t => t.purchaseId == purchase.purchaseId);
                if (purchaseOrder == null)
                {
                    //前端传回的是typeId，用typeId在夹具类表中查询出typeId对应的code
                    tool_type type = context.tool_type.FirstOrDefault(u => u.typeId == purchase.toolType);
                    int num = purchase.purchaseNum;
                    user userentity = (user)HttpContext.Session["userInfo"];
                    for (int i = 0; i < num; i++)
                    {
                        purchase_order addPurchaseOrder = new purchase_order()
                        {
                            purchaseId = Guid.NewGuid(),
                            code = type.code,
                            typeId = purchase.toolType,
                            //proposerId = Guid.NewGuid(),//有管理员后应该传入当前用户的id
                            proposerId = userentity.id,
                            state = -1//初始赋值为-1
                        };
                        context.purchase_order.Add(addPurchaseOrder);
                    }
                }
                else
                {
                    tool_type type = context.tool_type.FirstOrDefault(u => u.code == purchase.code);
                    purchaseOrder.typeId = purchase.toolType;
                    purchaseOrder.code = type.code;
                }
                int flg = context.SaveChanges();
                if (flg > 0)
                {
                    return Json(new
                    {
                        Success = true,
                        Message = "操作成功"
                    });
                }
                return Json(new
                {
                    Success = false,
                    Message = "操作失败"
                });
            }
        }

        public ActionResult BatchDelPurchaseOrders(List<Guid> purchaseIds)
        {
            using (TMSDbContext context = new TMSDbContext())
            {
                var delToolTypeQuery = context.purchase_order.Where(t => purchaseIds.Contains(t.purchaseId));
                context.purchase_order.RemoveRange(delToolTypeQuery);
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

        public ActionResult DelPurchaseOrder(Guid? purchaseId)
        {
            using (TMSDbContext context = new TMSDbContext())
            {
                purchase_order purchase_Order = context.purchase_order.FirstOrDefault(u => u.purchaseId == purchaseId);
                context.purchase_order.Remove(purchase_Order);
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

        public ActionResult SelectToolTypeCode()
        {
            using (TMSDbContext context = new TMSDbContext())
            {
                var query = context.tool_type.AsQueryable().OrderBy(t => t.typeId).ToList();
                var result = new
                {
                    code = 0,
                    msg = "",
                    count = query.Count(),
                    data = query
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SelectToolTypeCodeList()
        {
            using (TMSDbContext context = new TMSDbContext())
            {
                List<tool_type> toolTypes = context.tool_type.ToList();
                return Json(toolTypes, JsonRequestBehavior.AllowGet);
            }
        }

        //*******************************************报修申请repair************************************//
        public ActionResult GetRepairOrderList(int page, int limit)
        {
            using (TMSDbContext entities = new TMSDbContext())
            {
                if (Request.QueryString.Get("keyword").Equals(""))
                {
                    //purchaseOrderQuery = entities.purchase_order.AsQueryable();
                    repairOrderQuery = from A in entities.repair_order
                                       join B in entities.tool on A.toolId equals B.toolId
                                       join C in entities.tool_type on B.typeId equals C.typeId
                                       join D in entities.user on A.proposerId equals D.id
                                       select new RepairOrderTableModel
                                       {
                                           proposerName = D.name,
                                           repairOrderId = A.repairOrderId,
                                           proposerId = A.proposerId,
                                           toolId = A.toolId,
                                           code = C.code,
                                           seqId = B.seqId,
                                           dealerId = A.dealerId,
                                           faultPicUrl = A.faultPicUrl,
                                           description = A.description,
                                           state = A.state
                                       };
                }
                else
                {
                    string keyword = Request.QueryString.Get("keyword");
                    repairOrderQuery = from A in entities.repair_order
                                       join B in entities.tool on A.toolId equals B.toolId
                                       join C in entities.tool_type on B.typeId equals C.typeId
                                       join D in entities.user on A.proposerId equals D.id
                                       where C.code.Contains(keyword)
                                       select new RepairOrderTableModel
                                       {
                                           proposerName = D.name,
                                           repairOrderId = A.repairOrderId,
                                           proposerId = A.proposerId,
                                           toolId = A.toolId,
                                           code = C.code,
                                           seqId = B.seqId,
                                           dealerId = A.dealerId,
                                           faultPicUrl = A.faultPicUrl,
                                           description = A.description,
                                           state = A.state
                                       };
                }
                var pageQuery = repairOrderQuery.OrderByDescending(p => p.repairOrderId).Skip(limit * (page - 1)).Take(limit).ToList();
                //layui的特定格式
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

        public ActionResult RepairOrderDetail(Guid? repairId)
        {
            if (repairId == null)
            {
                return View();
            }
            using (TMSDbContext entities = new TMSDbContext())
            {
                repair_order repair = entities.repair_order.FirstOrDefault(u => u.repairOrderId == repairId);
                ViewBag.repair = Newtonsoft.Json.JsonConvert.SerializeObject(repair);
                return View();
            }
        }

        public ActionResult SubRepairOrderDetail(RepairOrderDetailModel repair, String faultPicUrl)
        {
            using (TMSDbContext context = new TMSDbContext())
            {
                repair_order repair_Order = context.repair_order.FirstOrDefault(t => t.repairOrderId == repair.repairOrderId);
                if (repair_Order == null)
                {
                    //前端传回的是typeId，用typeId在夹具类表中查询出typeId对应的code
                    //tool_type type = context.tool_type.FirstOrDefault(u => u.typeId == purchase.toolType);
                    user userentity = (user)HttpContext.Session["userInfo"];
                    repair_order addRepairOrder = new repair_order()
                    {
                        repairOrderId = Guid.NewGuid(),
                        description = repair.description,
                        //前端选项显示为seqId，但value为toolId
                        toolId = repair.seqId,
                        faultPicUrl = faultPicUrl,
                        proposerId = userentity.id,
                        state = -1
                    };
                    context.repair_order.Add(addRepairOrder);
                }
                else
                {
                    //tool_type type = context.tool_type.FirstOrDefault(u => u.code == purchase.code);
                    //purchaseOrder.typeId = purchase.toolType;
                    //purchaseOrder.code = type.code;
                }
                int flg = context.SaveChanges();
                if (flg > 0)
                {
                    return Json(new
                    {
                        Success = true,
                        Message = "操作成功"
                    });
                }
                return Json(new
                {
                    Success = false,
                    Message = "操作失败"
                });
            }
        }

        public ActionResult SelectToolSeqId(Guid? toolType)
        {
            using (TMSDbContext context = new TMSDbContext())
            {
                //Guid toolType = new Guid(Request.QueryString.Get("toolType"));
                //Guid toolType = new Guid(repair.state.ToString());
                var query = context.tool.Where(t => t.typeId == toolType).AsQueryable().OrderBy(t => t.typeId).ToList();
                var result = new
                {
                    code = 0,
                    msg = "",
                    count = query.Count(),
                    data = query
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult SelectToolSeqIdList(Guid? toolType)
        {

            using (TMSDbContext context = new TMSDbContext())
            {
                List<tool> tool = context.tool.Where(t => t.typeId == toolType).AsQueryable().OrderBy(t => t.seqId).ToList();
                return Json(tool, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult BatchDelRepairOrders(List<Guid> repairIds)
        {
            using (TMSDbContext context = new TMSDbContext())
            {
                var delRepairOrderQuery = context.repair_order.Where(t => repairIds.Contains(t.repairOrderId));
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

        public ActionResult DelRepairOrder(Guid? repairId)
        {
            using (TMSDbContext context = new TMSDbContext())
            {
                repair_order repair = context.repair_order.FirstOrDefault(u => u.repairOrderId == repairId);
                String imgPath = repair.faultPicUrl.Remove(0,1);
                String path = System.AppDomain.CurrentDomain.BaseDirectory;
                String deletePath = path + imgPath;
                if (System.IO.File.Exists(deletePath))
                {
                    System.IO.File.Delete(deletePath);
                }
                context.repair_order.Remove(repair);
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

        //上传图片
        public ActionResult UploadRepairPic()
        {
            try
            {
                HttpFileCollectionBase files = Request.Files;
                HttpPostedFileBase file = files[0];
                //获取文件名后缀
                string extName = Path.GetExtension(file.FileName).ToLower();
                //获取保存目录的物理路径
                if (System.IO.Directory.Exists(Server.MapPath("/Images/Repair/")) == false)//如果不存在就创建images文件夹
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath("/Images/Repair/"));
                }
                string path = Server.MapPath("/Images/Repair/"); //path为某个文件夹的绝对路径，不要直接保存到数据库
                                                          //    string path = "F:\\TgeoSmart\\Image\\";
                                                          //生成新文件的名称，guid保证某一时刻内图片名唯一（文件不会被覆盖）
                string fileNewName = Guid.NewGuid().ToString();
                string ImageUrl = path + fileNewName + extName;
                //SaveAs将文件保存到指定文件夹中
                file.SaveAs(ImageUrl);
                //此路径为相对路径，只有把相对路径保存到数据库中图片才能正确显示（不加~为相对路径）
                string url = "\\Images\\Repair\\" + fileNewName + extName;
                return Json(new
                {
                    Result = true,
                    Data = url
                });
            }
            catch (Exception exception)
            {
                return Json(new
                {
                    Result = false,
                    exception.Message
                });
            }
        }



        //*******************************************报废申请scrap************************************//

        public ActionResult GetScrapOrderList(int page, int limit)
        {
                using (TMSDbContext entities = new TMSDbContext())
                {
                if (Request.QueryString.Get("keyword").Equals(""))
                {
                    scrapOrderQuery = from A in entities.scrap_order
                                      join B in entities.user on A.proposerId equals B.id
                                      join C in entities.tool on A.toolId equals C.toolId
                                      join D in entities.tool_type on C.typeId equals D.typeId
                                      select new ScrapOrderTableModel
                                      {
                                          scrapId = A.scrapId,
                                          proposerName = B.name,
                                          proposerId = A.proposerId,
                                          code = D.code,
                                          seqId = C.seqId,
                                          supervisorId = A.supervisorId,
                                          managerId = A.managerId,
                                          toolId = A.toolId,
                                          lifeCount = A.lifeCount,
                                          description = A.description,
                                          state = A.state
                                      };
                }
                else
                {
                    string keyword = Request.QueryString.Get("keyword");
                    scrapOrderQuery = from A in entities.scrap_order
                                      join B in entities.user on A.proposerId equals B.id
                                      join C in entities.tool on A.toolId equals C.toolId
                                      join D in entities.tool_type on C.typeId equals D.typeId
                                      where D.code.Contains(keyword)
                                      select new ScrapOrderTableModel
                                      {
                                          scrapId = A.scrapId,
                                          proposerName = B.name,
                                          proposerId = A.proposerId,
                                          code = D.code,
                                          seqId = C.seqId,
                                          supervisorId = A.supervisorId,
                                          managerId = A.managerId,
                                          toolId = A.toolId,
                                          lifeCount = A.lifeCount,
                                          description = A.description,
                                          state = A.state
                                      };
                }

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

        public ActionResult ScrapOrderDetail(Guid? scrapId)
        {
            if (scrapId == null)
            {
                return View();
            }
            using (TMSDbContext entities = new TMSDbContext())
            {
                scrap_order scrap_Order = entities.scrap_order.FirstOrDefault(u => u.scrapId == scrapId);
                ViewBag.scrap = Newtonsoft.Json.JsonConvert.SerializeObject(scrapId);
                return View();
            }
        }

        public ActionResult SubScrapOrderDetail(ScrapOrderDetailModel scrap)
        {
            using (TMSDbContext context = new TMSDbContext())
            {
                scrap_order scrap_Order = context.scrap_order.FirstOrDefault(t => t.scrapId == scrap.scrapId);
                if (scrap_Order == null)
                {
                    //前端传回的是typeId，用typeId在夹具类表中查询出typeId对应的code
                    //tool_type type = context.tool_type.FirstOrDefault(u => u.typeId == purchase.toolType);
                    user userentity = (user)HttpContext.Session["userInfo"];
                    scrap_order addScrapOrder = new scrap_order()
                    {
                        scrapId = Guid.NewGuid(),
                        description = scrap.description,
                        //前端选项显示为seqId，但value为toolId
                        lifeCount = scrap.lifeCount,
                        toolId = scrap.seqId,
                        proposerId = userentity.id,
                        state = -1
                    };
                    context.scrap_order.Add(addScrapOrder);
                }
                else
                {
                    //tool_type type = context.tool_type.FirstOrDefault(u => u.code == purchase.code);
                    //purchaseOrder.typeId = purchase.toolType;
                    //purchaseOrder.code = type.code;
                }
                int flg = context.SaveChanges();
                if (flg > 0)
                {
                    return Json(new
                    {
                        Success = true,
                        Message = "操作成功"
                    });
                }
                return Json(new
                {
                    Success = false,
                    Message = "操作失败"
                });
            }
        }

        
        public ActionResult BatchDelScrapOrders(List<Guid> scrapIds)
        {
            using (TMSDbContext context = new TMSDbContext())
            {
                var delScrapOrderQuery = context.scrap_order.Where(s => scrapIds.Contains(s.scrapId));
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

        public ActionResult DelScrapOrder(Guid? scrapId)
        {
            using (TMSDbContext context = new TMSDbContext())
            {
                scrap_order scrap = context.scrap_order.FirstOrDefault(s => s.scrapId == scrapId);
                context.scrap_order.Remove(scrap);

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


        //*****************************************预警******************************//
        /******新增代码*******/
        //class SendEmail
        //{
        //    public static void SendInfo(string receivingMailBox, string subject, string mailContent)
        //    {
        //        //SendMailbox:发送信息的邮箱
        //        //SMTPServiceCode:邮箱smtp服务密码，确保邮箱已经开启了SMTP服务，开启后会给出一串编码就是smtp服务密码，后台填入编码
        //        var client = new SmtpClient
        //        {
        //            Host = "smtp.163.com",
        //            Port = 25,
        //            EnableSsl = true,
        //            DeliveryMethod = SmtpDeliveryMethod.Network,
        //            UseDefaultCredentials = true,
        //            Credentials = new System.Net.NetworkCredential("17770985759@163.com", "KXXFTWDQAHXPBWXD")
        //        };
        //        MailMessage msg = new MailMessage("17770985759@163.com", receivingMailBox, subject, mailContent)
        //        {
        //            SubjectEncoding = System.Text.Encoding.UTF8,
        //            BodyEncoding = System.Text.Encoding.UTF8,
        //        };
        //        try
        //        {
        //            client.Send(msg);
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine(e.Message);
        //        }
        //    }

        //}
        //class Program
        //{

        //    System.Threading.Timer timer;
        //    private static string MySqlCon = "Data Source=DESKTOP-E3D2N1N;Initial Catalog=TMSDatabase;Integrated Security=True";
        //    private static string toolid;
        //    public Program()
        //    {
        //        //3秒执行一次
        //        // timer = new System.Threading.Timer(SetCensusURL,null, 0, 1000 * 3);
        //    }
        //    public SqlDataReader ExecuteQuery(string sqlStr)
        //    {
        //        SqlConnection con = new SqlConnection(@MySqlCon);
        //        con.Open();
        //        SqlCommand cmd = new SqlCommand
        //        {
        //            Connection = con,
        //            CommandType = CommandType.Text,
        //            CommandText = sqlStr
        //        };
        //        SqlDataReader reader = cmd.ExecuteReader();
        //        return reader;


        //    }
        //    public static void RunPython(string argName, string args = "", params string[] teps)
        //    {
        //        Process p = new Process();
        //        //获得python的绝对路径
        //        //string path = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + argName;
        //        string path = @"C:\Users\DELL\PycharmProjects\pca\venv\" + argName;
        //        p.StartInfo.FileName = @"C:\Users\DELL\PycharmProjects\pca\venv\Scripts\python.exe";
        //        string sArguments = path;
        //        foreach (string sigstr in teps)
        //        {
        //            sArguments += " " + sigstr;
        //        }
        //        sArguments += " " + args;
        //        p.StartInfo.Arguments = sArguments;

        //        p.StartInfo.UseShellExecute = false;

        //        p.StartInfo.RedirectStandardOutput = true;

        //        p.StartInfo.RedirectStandardInput = true;

        //        p.StartInfo.RedirectStandardError = true;

        //        p.StartInfo.CreateNoWindow = true;

        //        p.Start();
        //        p.BeginOutputReadLine();
        //        p.OutputDataReceived += new DataReceivedEventHandler(p_OutputDataReceived);

        //        p.WaitForExit();
        //    }
        //    //若预测为高危状态，则发邮件给仓管员
        //    static void p_OutputDataReceived(object sender, DataReceivedEventArgs e)
        //    {
        //        if (!string.IsNullOrEmpty(e.Data))
        //        {
        //            if (e.Data.IndexOf("1") > -1)
        //            {
        //                SendEmail.SendInfo("1404955596@qq.com", "预警报告", "工夹具" + toolid + "正处于高危状态，请及时报修");
        //            }
        //        }
        //    }
        //    public static string ReadTxtContent(string Path)
        //    {
        //        StreamReader sr = new StreamReader(Path, Encoding.Default);
        //        string content, cont = null;
        //        while ((content = sr.ReadLine()) != null)
        //        {
        //            cont = content;
        //        }
        //        return cont;
        //    }
        //    public delegate void AppendTextCallback(string text);
        //    public static void AppendText(string text)
        //    {
        //        Console.WriteLine(text);     //此处在控制台输出.py文件print的结果

        //    }
        //    public string getData(SqlDataReader read, String para)
        //    {
        //        return read.GetInt32(read.GetOrdinal(para)).ToString();
        //    }
        //    public static void sqlToTxt(string txtPath)
        //    {
        //        //保存数据库每一列数据
        //        List<string> idList = new List<string>();
        //        string str3 = "SELECT * FROM dbo.tool";
        //        Program r1 = new Program();
        //        StreamWriter sw = new StreamWriter(txtPath, true, Encoding.Default);
        //        SqlConnection con = new SqlConnection(@MySqlCon);
        //        con.Open();
        //        SqlCommand cmd = new SqlCommand
        //        {
        //            Connection = con,
        //            CommandType = CommandType.Text,
        //            CommandText = str3
        //        };
        //        SqlDataReader reader = cmd.ExecuteReader();
        //        try
        //        {
        //            SqlDataReader read = r1.ExecuteQuery(str3);
        //            while (read.Read())
        //            {
        //                string proDate = read["proDate"].ToString();
        //                DateTime pt = Convert.ToDateTime(proDate);
        //                DateTime now = DateTime.Now;
        //                string regDate = read["regDate"].ToString();
        //                DateTime rt = Convert.ToDateTime(regDate);
        //                int info3 = read.GetInt32(read.GetOrdinal("usedCount"));
        //                toolid = read["toolId"].ToString();
        //                TimeSpan np = now - pt;
        //                TimeSpan nr = now - rt;
        //                string c = info3.ToString();
        //                sw.WriteLine(np.Days.ToString() + '\t' + nr.Days.ToString() + '\t' + c + '\t' + nr.Days.ToString() + '\t' + nr.Days.ToString());
        //            }
        //            sw.Flush();
        //            sw.Close();

        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine(e.Message);

        //        }
        //        con.Close();

        //    }
        //    public static void predict()
        //    {
        //        string filePath = @"D:\\test.txt";
        //        sqlToTxt(filePath);
        //        string sArgum = filePath;
        //        string strArr = @"b.py";
        //        RunPython(strArr, "-u", sArgum);
        //    }
        //}
    }
}