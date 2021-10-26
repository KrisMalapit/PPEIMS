using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using DNTBreadCrumb.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PPEIMS.Models;
using PPEIMS.Models.View_Model;


namespace PPEIMS.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly PPEIMSContext _context;

        public DepartmentsController(PPEIMSContext context)
        {
            _context = context;
        }
        [BreadCrumb(Title = "Index", Order = 1, IgnoreAjaxRequests = true)]
        // GET: Departments
        public IActionResult Index()
        {
            this.SetCurrentBreadCrumbTitle("Departments");
           
            //ViewData["PPEId"] = new SelectList(_context.PPEs.Where(a => a.Status == "Active"), "Id", "Name");

            return View();
        }
        public IActionResult getData()
        {
            string status = "";
            var v =

                _context.Departments.Where(a => a.Status != "Deleted").Select(a => new {


                    a.Code
                      ,
                    a.Name
                    ,
                    CompanyName = a.Companies.Name,
                    a.ID




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
        public JsonResult SearchPPE(string q)
        {
            var model = _context.PPEs
                .Where(a => a.Status == "Active")
                //.Where(a => a.Name.ToUpper().Contains(q.ToUpper())
                //|| a.Code.ToUpper().Contains(q.ToUpper()))
                .Select(b => new
                {
                    id = b.Id,
                    text = b.Name,

                });
            var x = model.ToList();
            var modelItem = new
            {
                total_count = model.Count(),
                incomplete_results = false,
                items = model.ToList(),
            };
            return Json(modelItem);
        }
        // GET: Departments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .Include(d => d.Companies)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        [BreadCrumb(Title = "Create", Order = 2, IgnoreAjaxRequests = true)]
        // GET: Departments/Create
        public IActionResult Create()
        {
            this.AddBreadCrumb(new BreadCrumb
            {
                Title = "Department",
                Url = string.Format(Url.Action("Index", "Departments")),
                Order = 1
            });

            ViewData["CompanyId"] = new SelectList(_context.Companies, "ID", "Name");
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Code,Name,Status,CompanyId")] Department department)
        {
            if (ModelState.IsValid)
            {
                _context.Add(department);
                await _context.SaveChangesAsync();


                Log log = new Log
                {
                    Descriptions = "Create Department - " + department.ID,
                    Action = "Create",
                    Status = "success",
                    UserId = User.Identity.GetUserName()
                };
                _context.Add(log);

                _context.SaveChanges();



                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "ID", "Name", department.CompanyId);
            return View(department);
        }
        [BreadCrumb(Title = "Edit", Order = 2, IgnoreAjaxRequests = true)]
        // GET: Departments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            this.AddBreadCrumb(new BreadCrumb
            {
                Title = "Department",
                Url = string.Format(Url.Action("Index", "Departments")),
                Order = 1
            });

            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "ID", "Name", department.CompanyId);
            return View(department);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Code,Name,Status,CompanyId")] Department department)
        {
            if (id != department.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(department);
                    await _context.SaveChangesAsync();

                    Log log = new Log
                    {
                        Descriptions = "Update Department - " + department.ID,
                        Action = "Edit",
                        Status = "success",
                        UserId = User.Identity.GetUserName()
                    };
                    _context.Add(log);

                    _context.SaveChanges();


                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentExists(department.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "ID", "Name", department.CompanyId);
            return View(department);
        }

        [BreadCrumb(Title = "Delete", Order = 2, IgnoreAjaxRequests = true)]
        // GET: Departments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            this.AddBreadCrumb(new BreadCrumb
            {
                Title = "Department",
                Url = string.Format(Url.Action("Index", "Departments")),
                Order = 1
            });
            var department = await _context.Departments
                .Include(d => d.Companies)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var model = await _context.Departments.FindAsync(id);
            model.Status = "Deleted";
            _context.Update(model);
            await _context.SaveChangesAsync();


            Log log = new Log
            {
                Descriptions = "Delete Department - " + id,
                Action = "Delete",
                Status = "success",
                UserId = User.Identity.GetUserName()
            };
            _context.Add(log);
            _context.SaveChanges();


            return RedirectToAction(nameof(Index));
        }

        private bool DepartmentExists(int id)
        {
            return _context.Departments.Any(e => e.ID == id);
        }
        [HttpPost]
        public IActionResult saveRequest(DepartmentPPE[] member, string ppeid)
        {
            int deptid = member[0].DepartmentId;
            string status = "";
            
            string message = "";
            status = "success";
            try
            {
                int[] ppelist = ppeid.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
                
                _context.DepartmentPPEs
                        .Where(a => a.Status == "Active")
                        .Where(a => a.DepartmentId == member[0].DepartmentId)
                        .Where(a => !ppelist.Contains(a.PPEId))
                        .ToList()
                        .ForEach(b => { b.Status = "Deleted"; });
                _context.SaveChanges();

                foreach (var item in member)
                {
                    int _cnt = _context.DepartmentPPEs
                        .Where(a => a.Status == "Active")
                        .Where(a => a.DepartmentId == item.DepartmentId)
                        .Where(a => a.PPEId == item.PPEId).Count();

                    if (_cnt == 0)
                    {
                        item.Status = "Active";
                        item.CreatedDate = DateTime.Now;
                        _context.DepartmentPPEs.Add(item);
                    }
                    else
                    {
                        
                      
                        item.Status = "Active";
                        
                        _context.Update(item);
                    }


                }
                _context.SaveChanges();

                status = "success";
            }
            catch (Exception e)
            {
                status = e.ToString();
                message = e.Message;
               

            }
            var model = new
            {
                status,
                message,
                refid = deptid
            };
            return Json(model);
        }
        public IActionResult getDataDetails(int id)
        {
            string strFilter = "";
            try
            {

                var v =

               _context.DepartmentPPEs
              .Where(a => a.DepartmentId == id)
              .Where(a => a.Status != "Deleted")

              .Select(a => new
              {
                  a.PPEId,
                  PPEName = a.PPEs.Name,
                  a.Field,
                  a.Office,
                  a.DepartmentId,
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
    }
}
