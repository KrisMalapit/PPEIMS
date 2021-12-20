using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PPEIMS.Models;

namespace PPEIMS.Controllers
{
    public class ProfileController : Controller
    {
        private readonly PPEIMSContext _context;
        public ProfileController(PPEIMSContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            ViewData["EmployeeName"] = User.Identity.GetFullName();
            ViewData["DepartmentName"] = User.Identity.GetDepartmentName();
            var reqppe = _context.DepartmentPPEs.Where(a => a.DepartmentId == Convert.ToInt32(User.Identity.GetDepartmentID())).Select(a => a.PPEs.Name).ToArray();
            string rppe = "";
            foreach (var item in reqppe)
            {
                rppe += item + ",";
            }

            if (rppe.Trim() != "")
            {
                rppe = rppe.Remove(rppe.Length - 1);
            }
            ViewData["RequiredPPE"] = rppe;

            return View();
        }
        public IActionResult getData()
        {
            string status = "";
            try
            {
                //int deptId = Convert.ToInt32(User.Identity.GetDepartmentID());


                var v =

                     _context.RequestDetailUsers
                                    .Where(b => b.Status == "Active")
                                    .Where(b => b.DocumentStatus == 1)
                                    .Where(b => b.UserId == User.Identity.GetUserId())
                                    .Where(b=>b.RequestDetails.Status == "Active")
                                    .Where(b => b.RequestDetails.Requests.DocumentStatus == "Approved")
                                    .Select(a => new
                                    {
                                        PPE = a.RequestDetails.Items.PPEs.Name,
                                        ItemNo = a.RequestDetails.Items.No,
                                        a.RequestDetails.Items.Description,
                                        IssuedDate = a.RequestDetails.Requests.WarehouseApprovedDate,
                                        EmployeeType = a.Users.Category,
                                        Months = a.Users.Category == "OFFICE" ? a.RequestDetails.Items.PPEs.Office : a.RequestDetails.Items.PPEs.Field,


                                        EmployeeName = a.Users.Name
                                    });


                int cnt = v.Count();

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

    }
}