using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using DNTBreadCrumb.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PPEIMS.Models;
using PPEIMS.Models.View_Model;

namespace PPEIMS.Controllers
{
    public class RequestsController : Controller
    {
        private readonly PPEIMSContext _context;
        public RequestsController(PPEIMSContext context)
        {
            _context = context;
        }

        [BreadCrumb(Title = "Index", Order = 1, IgnoreAjaxRequests = true)]
        public IActionResult Index()
        {
           
            this.SetCurrentBreadCrumbTitle("Request");
            return View();
        }
        public JsonResult SearchItem(string q)
        {
            var model = _context.Items
                .Where(a => a.Status == "Active")
                .Where(a => a.Description.ToUpper().Contains(q.ToUpper())
                || a.No.ToUpper().Contains(q.ToUpper())).Select(b => new
                {
                    id = b.Id,
                    text = b.No + " | " + b.Description + " | " + b.PPEs.Name,

                });

            var modelItem = new
            {
                total_count = model.Count(),
                incomplete_results = false,
                items = model.ToList(),
            };
            return Json(modelItem);
        }
        [HttpPost]
        public ActionResult getData()
        {
            var roleName = User.Identity.GetRoleName();
            string docstatus = "";
            string compaccess = User.Identity.GetCompanyAccess();
            int[] _compaccess = compaccess.Split(',').Select(n => Convert.ToInt32(n)).ToArray();

            switch (roleName)
            {
                case "Admin":
                    break;
                case "User":

                    //docstatus = "Pending";
                   
                    //string[] stat = status.Split(',').Select(n => n).ToArray();

                    break;
                case "Dept Head":
                    docstatus = "For Approval Dept Head";
                    break;
                case "Safety":
                    docstatus = "For Approval Safety Head";
                    break;
                case "Warehouseman":
                    docstatus = "For Approval Warehouseman";
                    break;

            }



            



            string strFilter = "";
            try
            {


                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;


                for (int i = 0; i < 3; i++)
                {
                    string colval = Request.Form["columns[" + i + "][search][value]"];
                    if (colval != "")
                    {
                        colval = colval.ToUpper();
                        string colSearch = Request.Form["columns[" + i + "][name]"];



                        if (strFilter == "")
                        {

                            strFilter = colSearch + ".ToString().ToUpper().Contains(" + "\"" + colval + "\"" + ")";

                        }
                        else
                        {
                            strFilter = strFilter + " && " + colSearch + ".ToString().ToUpper().Contains(" + "\"" + colval + "\"" + ")";
                        }

                    }
                }


                if (strFilter == "")
                {
                    strFilter = "true";
                }

                int recCount = 0;

                if (roleName == "User")
                {
                    recCount =
                    _context.Requests
                        .Where(a=>a.CreatedByUserId == User.Identity.GetUserId())
                        .Where(a => a.Status == "Active")
                        .Where(strFilter)
                        .Count();
                }
                else if (roleName == "Warehouseman")
                {
                    recCount =
                    _context.Requests
                        .Where(a => _compaccess.Contains(a.CompanyId))
                        .Where(a => a.Status == "Active")
                        .Where(strFilter)
                        .Count();

                }
                else if (roleName == "Admin")
                {
                    recCount =
                    _context.Requests
                       
                        .Where(a => a.Status == "Active")
                        .Where(strFilter)
                        .Count();
                }
                else
                {
                    recCount =
                    _context.Requests
                        //.Where(a => a.DepartmentId == Convert.ToInt32(User.Identity.GetDepartmentID()))
                        .Where(a => a.DocumentStatus == docstatus)
                        .Where(a => a.Status == "Active")
                        .Where(strFilter)
                        .Count();
                }
                

                recordsTotal = recCount;
                int recFilter = recCount;

                var m = _context.Requests
                    .Where(a => a.Status == "Active")
                    .Where(strFilter);

                if (roleName == "User")
                {
                    m = m.Where(a => a.CreatedByUserId == User.Identity.GetUserId());
                }
                else if (roleName == "Warehouseman")
                {
                    m = m
                         .Where(a => _compaccess.Contains(a.CompanyId))
                        .Where(a => a.DocumentStatus == docstatus);
                }
                else if (roleName == "Admin")
                {
                    m = m.Where(a => a.DocumentStatus != "0");
                }
                else
                {
                    m = m.Where(a => a.DocumentStatus == docstatus);
                        //.Where(a => a.DepartmentId == Convert.ToInt32(User.Identity.GetDepartmentID()));
                }

                var v =
                    m.Skip(skip).Take(pageSize)
                      .Select(a => new
                      {
                          a.CreatedDate,
                          a.ReferenceNo,
                          a.CreatedBy,
                          a.DocumentStatus,
                          a.Id
                
                      });




                bool desc = false;
                if (sortColumnDirection == "desc")
                {
                    desc = true;
                }
                v = v.OrderBy(sortColumn + (desc ? " descending" : ""));



                if (pageSize < 0)
                {
                    pageSize = recordsTotal;
                }


                var data = v;
                var jsonData = new { draw = draw, recordsFiltered = recFilter, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }


        public IActionResult getEmployees(int RequestId, int itemid)
        {
            string status = "";

            int deptId = Convert.ToInt32(User.Identity.GetDepartmentID());
            DateTime dt = new DateTime(1900, 01, 01);

            //var v =

            //    _context.Users.Where(a => a.Status == "1")
            //    .Where(a => a.DepartmentId == deptId)
            //    .Select(a => new
            //    {
            //        EmployeeName = a.Name,
            //        a.Id,
            //        IsExisting = _context.RequestDetailUsers.Where(b => b.RequestDetailId == RequestId)
            //                    .Where(b => b.Status == "Active").Where(b => b.UserId == a.Id).Count() == 0 ? 0 : 1
            //        ,
            //        IssuedDate = _context.RequestDetailUsers.Where(b=>b.UserId == a.Id).Where
            //        ,
            //        Months = a.Category == "OFFICE" ? _context.RequestDetails.Where(b => b.ItemId == itemid).FirstOrDefault().Items.PPEs.Office : _context.RequestDetails.Where(b => b.ItemId == itemid).FirstOrDefault().Items.PPEs.Field

            //    });


           


            var v = _context.Users
                .Where(a => a.Status == "1")
                .Where(a => a.DepartmentId == deptId)
                     .GroupJoin(
                         _context.RequestDetailUsers
                                    
                                    .Where(b => b.Status == "Active")
                                    .Where(b => b.DocumentStatus == 1)
                                    .Where(b => b.RequestDetails.Status == "Active")
                                    .Where(b => b.RequestDetails.ItemId == itemid)
                                    .Where(b => b.RequestDetails.Requests.DocumentStatus == "Approved"),
                        i => i.Id,
                        p => p.UserId,
                        (i, g) =>
                           new
                           {
                               i,
                               g
                           }
                     )
                     .SelectMany(
                        temp => temp.g.Take(1).DefaultIfEmpty(),
                        (temp, p) =>
                           new
                           {
                               EmployeeName = temp.i.Name,
                               temp.i.Id,
                               IsExisting = _context.RequestDetailUsers
                                            .Where(b => b.RequestDetailId == RequestId)
                                            .Where(b => b.DocumentStatus == 1)
                                            .Where(b => b.Status == "Active")
                                            .Where(b => b.UserId == temp.i.Id).Count() == 0 ? 0 : 1,

                               RequestDetailId = p.RequestDetails.Id.ToString() == null ? 0 : p.RequestDetails.Id,
                               RequestId = p.RequestDetails.Requests.Id.ToString() == null ? 0 : p.RequestDetails.Requests.Id,

                               IssuedDate = p.RequestDetails.Requests.WarehouseApprovedDate == null ? dt : p.RequestDetails.Requests.WarehouseApprovedDate,
                               EmployeeType = p.Users.Category,
                               Months = p.Users.Category == "OFFICE" ? (p.RequestDetails.Items.PPEs.Office.ToString() == null ? 0  : p.RequestDetails.Items.PPEs.Office) : (p.RequestDetails.Items.PPEs.Field.ToString() == null ? 0 : p.RequestDetails.Items.PPEs.Field)
                           }
                     );


            status = "success";
            var x = v.ToList();





            var model = new
            {
                status
                ,
                data = v
            };
            return Json(model);
        }

        public IActionResult getEmployeesView(int RequestId)
        {
            string status = "";
            try
            {
                int deptId = Convert.ToInt32(User.Identity.GetDepartmentID());


                var v =

                     _context.RequestDetailUsers.Where(b => b.RequestDetailId == RequestId)
                                    .Where(b => b.Status == "Active")
                                    .Select(a => new
                                    {

                                        EmployeeName = a.Users.Name
                                    });




                status = "success";
                var model = new
                {
                    status
                ,
                    data = v
                };
                return Json(model);
            }
            catch (Exception e)
            {

                var model = new
                {
                    status = "fail"
                ,
                    data = e.Message
                };
                return Json(model);
            }
            






            
        }

        [HttpPost]
        public IActionResult saveRequest(RequestViewModel fvm)
        {
            string status = "";
            string message = "";
            string series = "";
            string refno = "";
            string refid = "0";
            string newrefno = "";

            int reqId = fvm.Id;

            var items = fvm.no;

            //_context.RequestDetails
            //.Where(a => a.Status == "Active")
            //.Where(a => !items.Contains(a.ItemId))
            //.ToList()
            //.ForEach(b => { b.Status = "Deleted"; });
            //_context.SaveChanges();

            try
            {
                if (fvm.Id == 0)
                {
                    int userid = User.Identity.GetUserId();
                    string series_code = "REQUEST";
                    series = new NoSeriesController(_context).GetNoSeries(series_code);
                    refno = "RQ" + series;
                    int _compid = _context.Users.Include(a=>a.Departments).Where(a => a.Id == userid).FirstOrDefault().Departments.CompanyId;
                    var req = new Request
                    {
                        ReferenceNo = refno,
                        CreatedBy = User.Identity.GetFullName(),
                        CreatedByUserId = User.Identity.GetUserId(),
                        CreatedDate = DateTime.Now.Date,
                        DocumentStatus = "Pending",
                        Status = "Active",
                        DepartmentId = Convert.ToInt32(User.Identity.GetDepartmentID())
                        ,CompanyId = _compid
                    };

                  
                    _context.Add(req);
                    _context.SaveChanges();
                    reqId = req.Id;
                    

                    string x = new NoSeriesController(_context).UpdateNoSeries(series, series_code);


                    
                }

                var d = _context.RequestDetails.Where(a => a.RequestId == fvm.Id).Count();
             
                if (d == 0)
                {
                    for (int i = 0; i < fvm.qty.Length; i++)
                    {
                        var sub = new RequestDetail
                        {
                            ItemId = Convert.ToInt32(fvm.no[i]),
                            RequestId = reqId,
                            Quantity = Convert.ToInt32(fvm.qty[i]),
                            CreatedDate = DateTime.Now.Date,
                            Type = fvm.type[i],
                            Remarks = fvm.remarks[i],
                            QuantityIssued = Convert.ToInt32(fvm.qty[i]),
                            Status = "Active"
                        };
                        _context.Add(sub);

                    }
                    _context.SaveChanges();
                   
                    status = "success";


                }
                else
                {

                    _context.RequestDetails
                          .Where(a => a.RequestId == fvm.Id)
                          .ToList().ForEach(a => a.Status = "Deleted");
                    _context.SaveChanges();
                    

                    for (int i = 0; i < fvm.qty.Length; i++)
                    {
                        var rd = _context.RequestDetails
                            //.Where(a => a.Status == "Active")
                            .Where(a => a.RequestId == reqId)
                            .Where(a => a.ItemId == fvm.no[i])
                            .Where(a => a.Type == fvm.type[i])
                            .FirstOrDefault();

                        if (rd == null)
                        {
                            var sub = new RequestDetail
                            {
                                ItemId = Convert.ToInt32(fvm.no[i]),
                                RequestId = reqId,
                                Quantity = Convert.ToInt32(fvm.qty[i]),
                                CreatedDate = DateTime.Now.Date,
                                Type = fvm.type[i],
                                Remarks = fvm.remarks[i],
                                Status = "Active",
                                QuantityIssued = Convert.ToInt32(fvm.qty[i])
                            };
                            _context.Add(sub);
                        }
                        else
                        {
                            rd.Status = "Active";
                            rd.Quantity = Convert.ToInt32(fvm.qty[i]);
                            rd.CreatedDate = DateTime.Now.Date;
                            rd.Type = fvm.type[i];
                            rd.Remarks = fvm.remarks[i];
                            _context.Update(rd);
                        }
                    };
                    _context.SaveChanges();
                    status = "success";




                }

              
            }
            catch (Exception ex)
            {

                status = "fail";
                message = ex.Message;
            }

            var modelItem = new
            {
                status,
                message,
                refid = reqId,
                refno
            };
            return Json(modelItem);
        }
        public IActionResult GetInventory(int itemid)
        {
            int inv = 0;
            string status = "";
            string message = "";
            

            try
            {
                
                inv = _context.ItemDetails.Where(b => b.ItemId == itemid)
                            .Where(b => b.Status == "Active")
                            .Sum(b => b.Quantity);
                status = "success";
              
            }
            catch (Exception e)
            {
                status = "fail";
                message = e.Message;
            }


            var model = new
            {
                status ,
                message ,
                inv
            };
            return Json(model);



        }
        public IActionResult submitRequest(int refid)
        {
            int inv = 0;
            string status = "";
            string message = "";


            try
            {
                var req = _context.Requests.Find(refid);
                string currentDocumentStatus = req.DocumentStatus;
                string newDocumentStatus = "";


                var roleName = User.Identity.GetRoleName();
                switch (roleName)
                {
                   
                    case "User":
                        switch (currentDocumentStatus)
                        {
                            case "Pending":
                            case "Return to Requestor":
                                newDocumentStatus = "For Approval Dept Head";
                                req.DateSubmitted = DateTime.Now;
                                break;
                        }

                        newDocumentStatus = currentDocumentStatus;

                        break;
                    case "Dept Head":
                        newDocumentStatus = "For Approval Safety Head";
                        req.DepartmentApprovedDate = DateTime.Now;
                        req.DepartmentHeadId = User.Identity.GetUserId();
                        break;
                    case "Safety":
                        newDocumentStatus = "For Approval Warehouseman";
                        req.SafetyApprovedDate = DateTime.Now;
                        req.SafetyId = User.Identity.GetUserId();
                        break;
                    case "Warehouseman":
                        newDocumentStatus = "Approved";
                        req.WarehouseApprovedDate = DateTime.Now;
                        req.ApprovedDate = DateTime.Now.Date;
                        req.WarehousemanId = User.Identity.GetUserId();
                        break;

                }



               

                req.DocumentStatus = newDocumentStatus;
                _context.Update(req);
                _context.SaveChanges();


                
                //if (newDocumentStatus == "Approved")
                //{
                //    _context.RequestDetailUsers
                //       .Where(a => a.Status == "Active")
                //       .Where(a => a.DocumentStatus == 1)
                //       .ToList()
                //       .ForEach(b => { b.DocumentStatus = 0; });
                //    _context.SaveChanges();

                //    _context.RequestDetailUsers
                //      .Where(a => a.Status == "Active")
                     
                //      .Where(a => a.RequestDetails.RequestId == refid)
                //      .ToList()
                //      .ForEach(b => { b.DocumentStatus = 1; });
                //    _context.SaveChanges();
                //}





                status = "success";

                Log log = new Log
                {
                    Descriptions = "Update Record ID:" + refid + ". Set [Status] equals to " + newDocumentStatus + " from " + currentDocumentStatus,
                    UserId = User.Identity.GetUserName(),
                    Status = status
                };
                _context.Add(log);
                _context.SaveChanges();


            }
            catch (Exception e)
            {
                status = "fail";
                message = e.Message;
            }


            var model = new
            {
                status,
                message
            };
            return Json(model);



        }
        public IActionResult disapproveRequest(int refid)
        {
            int inv = 0;
            string status = "";
            string message = "";


            try
            {
                var req = _context.Requests.Find(refid);
                string currentDocumentStatus = req.DocumentStatus;
                //string newDocumentStatus = "";

                //switch (currentDocumentStatus)
                //{
                //    case "Pending":
                //        newDocumentStatus = "For Approval Dept Head";
                //        req.DateSubmitted = DateTime.Now;

                //        break;
                //    case "For Approval Dept Head":
                //        newDocumentStatus = "For Approval Safety Head";
                //        req.DepartmentApprovedDate = DateTime.Now;
                //        req.DepartmentHeadId = User.Identity.GetUserId();
                //        break;
                //    case "For Approval Safety Head":
                //        newDocumentStatus = "For Approval Warehouseman";
                //        req.SafetyApprovedDate = DateTime.Now;
                //        req.SafetyId = User.Identity.GetUserId();
                //        break;
                //    case "For Approval Warehouseman":
                //        newDocumentStatus = "Approved";
                //        req.WarehouseApprovedDate = DateTime.Now;
                //        req.WarehousemanId = User.Identity.GetUserId();
                //        break;
                //}

                req.DocumentStatus = "Return to Requestor";
                _context.Update(req);
                _context.SaveChanges();


                status = "success";

                Log log = new Log
                {
                    Descriptions = "Update Record. ID:" + refid + ". Set [Status] equals to Return to Requestor from " + currentDocumentStatus,
                    UserId = User.Identity.GetUserName(),
                    Status = status
                };
                _context.Add(log);
                _context.SaveChanges();


            }
            catch (Exception e)
            {
                status = "fail";
                message = e.Message;
            }


            var model = new
            {
                status,
                message
            };
            return Json(model);



        }
        public IActionResult getDataDetails(int id)
        {
            string strFilter = "";
            string refno = "";

            try
            {

                var v =

               _context.RequestDetails
              .Where(a => a.RequestId == id)
              .Where(a => a.Status != "Deleted")

              .Select(a => new
              {
                  ItemId = a.Items.Id,
                  ItemName = a.Items.No + " | " + a.Items.Description + " | " + a.Items.PPEs.Name,
                  a.Quantity,
                  a.Remarks,
                  a.Type,
                 
                  a.Id
                  ,a.QuantityIssued
                  ,a.Requests.ReferenceNo
                  ,a.Comments

              });


                var model = new
                {
                    data = v.ToList()
                   
                };




                return Json(model);
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }
        public IActionResult getDataDetailsView(int id)
        {
            string status = "";
            try
            {

                var v =

               _context.RequestDetails
              .Where(a => a.RequestId == id)
              .Where(a => a.Status != "Deleted")

              .Select(a => new
              {
                  ItemId = a.Items.Id,
                  ItemName = a.Items.No + " | " + a.Items.Description,
                  a.Quantity,
                  a.Remarks,
                  a.Type,
                  Inventory = _context.ItemDetails.Where(b => b.ItemId == a.ItemId)
                            .Where(b => b.Status == "Active")
                            .Sum(b => b.Quantity),

                  a.QuantityIssued,
                  a.Id,
                  a.Requests.ReferenceNo
                  ,a.Requests.DocumentStatus
                  ,a.Comments

              });

                status = "success";

                var model = new
                {
                    status
                    ,
                    data = v.ToList()

                };




                return Json(model);

            }
            catch (Exception ex)
            {
                var model = new
                {
                    status = "fail"
                    ,
                    message = ex.Message
                };
                return Json(model);
            }
        }
        public JsonResult SearchEmployee(string q)
        {

            var model = _context.Users.Where(e => e.Status == "1").Select(b => new
            {
                id = b.Id,
                text = b.LastName + ", " + b.FirstName,
            });

            if (!string.IsNullOrEmpty(q))
            {
                model = model.Where(b => b.text.Contains(q));
            }

            var modelEmp = new
            {
                total_count = model.Count(),
                incomplete_results = false,
                items = model.ToList(),
            };
            return Json(modelEmp);
        }
        [HttpPost]
        public ActionResult SaveEmployee(RequestDetailUser[] member, string userid)
        {
            
            string status = "";
           
            int[] userlist = userid.Split(',').Select(n => Convert.ToInt32(n)).ToArray();

            string message = "";
            try
            {
               
                _context.RequestDetailUsers
                        .Where(a => a.Status == "Active")
                        .Where(a => a.RequestDetailId == member[0].RequestDetailId)
                        .Where(a=>!userlist.Contains(a.UserId))
                        .ToList()
                        .ForEach(b => { b.Status = "Deleted"; });
                _context.SaveChanges();

                foreach (var item in member)
                {



                    int _cnt = _context.RequestDetailUsers
                        .Where(a => a.Status == "Active")
                        .Where(a => a.UserId == item.UserId)
                        .Where(a => a.RequestDetailId == item.RequestDetailId)
                        .Count();

                    if (_cnt == 0)
                    {
                        item.Status = "Active";
                        item.CreatedDate = DateTime.Now;
                        item.DocumentStatus = 1;
                        _context.RequestDetailUsers.Add(item);
                    }


                }
                _context.SaveChanges();

                status = "success";
            }
            catch (Exception e)
            {
                status = e.ToString();
                message = e.InnerException.Message;

            }
            var model = new
            {
                status,
                message
            };
            return Json(model);
        }
        public JsonResult saveQtyIssued(int id,int qty,string type,string comment)
        {
            string message = "";
            string status = "";


            try
            {
                var req = _context.RequestDetails.Find(id);
                if (type == "qtyissued")
                {
                    req.QuantityIssued = qty;
                }
                else
                {
                    req.Comments = comment;
                }
                

                _context.Update(req);
                _context.SaveChanges();

                status = "success";

                
            }
            catch (Exception e)
            {
                status = "fail";
                message = e.Message;
                throw;
            }

            var model = new
            {
                status,
                message
            };



            Log log = new Log
            {
                Descriptions = "Update Record. ID:" + id,
                UserId = User.Identity.GetUserName(),
                Status = status
            };
            _context.Add(log);
            _context.SaveChanges();

            return Json(model);
        }
        
    }
}