using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using DNTBreadCrumb.Core;
using PPEIMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PPEIMS.Controllers
{
    public class ItemsController : Controller
    {
        private readonly PPEIMSContext _context;

        public ItemsController(PPEIMSContext context)
        {
            _context = context;
        }

        [BreadCrumb(Title = "Index", Order = 1, IgnoreAjaxRequests = true)]
        public IActionResult Index()
        {
           


            this.SetCurrentBreadCrumbTitle("Item");
            ViewData["PPEId"] = new SelectList(_context.PPEs.Where(a => a.Status == "Active"), "Id", "Name");
            return View();
        }
        
        [HttpPost]
        public ActionResult SaveItem(int id,int PPEId)
        {
            string status = "";
            string message = "";
            try
            {
                var item = _context.Items.Find(id);
                item.PPEId = PPEId;
                //item.Office = Office;
                //item.Field = Field;
                _context.Entry(item).State = EntityState.Modified;
                _context.SaveChanges();

                status = "success";
            }
            catch (Exception ex)
            {
                status = "fail";
                message = ex.Message;
            }

            var model = new
            {
                status,
                message
            };

            return Json(model);
        }

        [HttpPost]
        public ActionResult getData()
        {
            string strFilter = "";

            string compaccess = User.Identity.GetCompanyAccess();
            int[] _compaccess = compaccess.Split(',').Select(n => Convert.ToInt32(n)).ToArray();


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


                for (int i = 0; i < 4; i++)
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

                _context.Items
                 .Where(a => a.Status == "Active")
                 .Where(a => _compaccess.Contains(a.CompanyId))
                .Select(a => new
                {
                    a.No,
                    a.Description,
                    a.Description2,
                    Inventory = _context.ItemDetails.Where(b => b.ItemId == a.Id)
                             .Where(b => b.Status == "Active")
                             .Sum(b => b.Quantity),
                    a.Id
                  ,
                    a.PPEId
                  ,
                    PPE = a.PPEId.ToString() == "" ? "" : a.PPEs.Name,
                    Company = _context.Companies.Where(b=>b.ID == a.CompanyId).FirstOrDefault().Name
        
                    //,a.Field
                    //,a.Office
                  
                })
               
               
                .Where(strFilter)

                .Count();

                recordsTotal = recCount;
                int recFilter = recCount;



                var v =
               _context.Items
              .Where(a => a.Status == "Active")
             .Where(a => _compaccess.Contains(a.CompanyId))
              .Select(a => new
              {
                  a.No,
                  a.Description,
                  a.Description2,
                  Inventory = _context.ItemDetails.Where(b => b.ItemId == a.Id) 
                             .Where(b => b.Status == "Active")
                             .Sum(b => b.Quantity),
                  a.Id
                  ,
                  a.PPEId
                  ,PPE = a.PPEId.ToString() == "" ? "" : a.PPEs.Name,
                  Company = _context.Companies.Where(b => b.ID == a.CompanyId).FirstOrDefault().Name
                  //,a.Field
                  //,a.Office
              }).Where(strFilter)
              .Skip(skip).Take(pageSize);

              




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
                var jsonData = new { draw = draw, recordsFiltered = recFilter, recordsTotal = recordsTotal, data = data};
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }
        public IActionResult getDataDetails(int itemid)
        {
            string status = "";
            var v =
                _context.ItemDetails.Where(a => a.Status != "Deleted")
                .Where(a => a.ItemId == itemid)
                .Select(a => new
                {
                     a.Id
                    ,a.CreatedDate
                    ,a.LineNo
                    ,a.Quantity
                    ,a.Remarks
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
    }
    
}