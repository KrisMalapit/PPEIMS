using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
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
        [HttpPost]
        public IActionResult saveRequest(RequestViewModel fvm)
        {
            string status = "";
            string message = "";
            string series = "";
            string refno = "";
            string refid = "0";
            int reqId = fvm.Id;
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
                        var sub = new RequestDetail();
                        sub.ItemId = Convert.ToInt32(fvm.no[i]);
                        sub.RequestId = reqId;
                        sub.Quantity = Convert.ToInt32(fvm.qty[i]);
                        sub.CreatedDate = DateTime.Now.Date;
                        sub.Status = "Active";
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

                    for (int i = 0; i < fvm.qty.Length; i++)
                    {
                        var sub = new RequestDetail
                        {
                            ItemId = Convert.ToInt32(fvm.no[i]),
                            RequestId = reqId,
                            Quantity = Convert.ToInt32(fvm.qty[i]),
                            CreatedDate = DateTime.Now.Date,
                            Status = "Active"
                        };
                        _context.Add(sub);

                    }
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
                  a.Id

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
        public ActionResult SaveEmployee(RequestDetailUser member)
        {
            string status = "";
            string message = "";
            try
            {
                member.Status = "Active";
                _context.RequestDetailUsers.Add(member);
                _context.SaveChanges();
                status = "success";
            }
            catch (Exception e)
            {
                status = e.ToString();
                message = e.InnerException.InnerException.Message;

            }
            var model = new
            {
                status,
                message
            };
            return Json(model);
        }
    }
}