using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using DNTBreadCrumb.Core;
using Microsoft.AspNetCore.Mvc;
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
                    text = b.No + " | " + b.Description,

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



                int recCount =

                _context.Requests
                .Where(a => a.Status == "Active")

                .Where(strFilter)
                .Count();

                recordsTotal = recCount;
                int recFilter = recCount;



                var v =

               _context.Requests
              .Where(a => a.Status == "Active")
              .Where(strFilter)
              .Skip(skip).Take(pageSize)
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


        public IActionResult getEmployees(int RequestId)
        {
            string status = "";

            int deptId = Convert.ToInt32(User.Identity.GetDepartmentID());


            var v =

                _context.Users.Where(e => e.Status == "1")
                .Where(a => a.DepartmentId == deptId)
                .Select(a => new {
                    EmployeeName = a.LastName + ", " + a.FirstName,
                    a.Id,
                    IsExisting = _context.RequestDetailUsers.Where(b => b.RequestDetailId == RequestId)
                                .Where(b => b.Status == "Active").Where(b => b.UserId == a.Id).Count() == 0 ? 0 : 1
                   
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
                    string series_code = "REQUEST";
                    series = new NoSeriesController(_context).GetNoSeries(series_code);
                    refno = "RQ" + series;

                    var req = new Request
                    {
                        ReferenceNo = refno,
                        CreatedBy = User.Identity.GetFullName(),
                        CreatedDate = DateTime.Now.Date,
                        DocumentStatus = "Pending",
                        Status = "Active"
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
                refid = reqId
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
        public IActionResult getDataDetails(int id)
        {
            string strFilter = "";
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
                 
                  a.Id
                  ,a.QuantityIssued

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
                  a.Id

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
        public JsonResult saveQtyIssued(int id,int qty)
        {
            string message = "";
            string status = "";


            try
            {
                var req = _context.RequestDetails.Find(id);
                //var emp = _context.Employees.Find(att.EmployeeId);
                //string empno = emp.EmployeeNo;
                //string empname = emp.LastName + ", " + emp.FirstName;
                //string attdate = att.CreatedDate.Date.ToString("MM/dd/yyyy");

                req.QuantityIssued = qty;
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