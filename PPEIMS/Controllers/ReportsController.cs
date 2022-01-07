using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using PPEIMS.Models;
using PPEIMS.Models.View_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace PPEIMS.Controllers
{
    public class ReportsController : Controller
    {
        private readonly PPEIMSContext _context;
        public ReportsController(PPEIMSContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {


            //string status = "Active,Default";
            //string[] stat = status.Split(',').Select(n => n).ToArray();
            //var lube = _context.LubeTrucks.Where(a => stat.Contains(a.Status)).Select(a => new
            //{
            //    a.Id,
            //    Text = a.No + " - " + a.Description
            //});
            //ViewData["LubeTruckId"] = new SelectList(lube.OrderBy(a => a.Id), "Id", "Text");



            var dept = _context.Departments.Where(a => a.Status == "Active").Select(a => new
            {
                Id = a.ID,
                Text = a.Name 
            });
            ViewData["DepartmentId"] = new SelectList(dept.OrderBy(a => a.Id), "Id", "Text");



            return View();
        }
        [HttpPost]
        public ActionResult getEmployeeSummary(DateTime strStart, DateTime end, int dept)
        {
            string status = "";

            string fstatus = "Active,Posted,Transferred";
            string[] fstat = fstatus.Split(',').Select(n => n).ToArray();


            try
            {




                var v =

                   _context.RequestDetailUsers
                                  .Where(b => b.Status == "Active")
                                  .Where(b => b.DocumentStatus == 1)
                                  //.Where(b => b.UserId == User.Identity.GetUserId())
                                  .Where(b => b.RequestDetails.Status == "Active")
                                  .Where(b => b.RequestDetails.Requests.DocumentStatus == "Approved")
                                  .Where(a => a.RequestDetails.Requests.ApprovedDate>= strStart && a.RequestDetails.Requests.ApprovedDate <= end)
                                  .Where(b => b.Users.DepartmentId == dept)
                                  .Select(a => new
                                  {
                                     
                                      Months = a.Users.Category == "OFFICE" ? a.RequestDetails.Items.PPEs.Office : a.RequestDetails.Items.PPEs.Field,
                                      EmployeeName = a.Users.Name,
                                      Department = a.Users.Departments.Name,
                                      Category = a.Users.Category,
                                      ItemNo = a.RequestDetails.Items.No,
                                      a.RequestDetails.Items.Description,
                                      a.RequestDetails.Items.Description2,
                                      DateIssued = a.RequestDetails.Requests.WarehouseApprovedDate,
                                      PPE = a.RequestDetails.Items.PPEs.Name,
                                      a.RequestDetails.Items.CompanyId

                                  });



                status = "success";

                var models = new
                {
                    status
                 ,
                    data = v
                };
                return Json(models);
            }
            catch (Exception ex)
            {
                var models = new
                {
                    status = "fail"
                 ,
                    message = ex.Message
                };
                return Json(models);
            }
        }
        public IActionResult printReport(ReportViewModel rvm)
        {

            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = new HttpResponseMessage();
                byte[] bytes = null;
                string xstring = JsonConvert.SerializeObject(rvm);



                string urilive = "http://californium/FODLApi/api/printreport?rvm=";
                string uridev = "http://sodium2/fodlapi/api/printreport?rvm=";
                string uridevminesite = "http://192.168.0.199/fodlapi/api/printreport?rvm=";
                string urilocal = "http://localhost:52441/api/printreport?rvm=";
                string urilocalhost1 = "http://192.168.102.104/fodlapi/api/printreport?rvm="; // jazel

                response = client.GetAsync(urilocal + xstring).Result;
                string byteToString = response.Content.ReadAsStringAsync().Result.Replace("\"", string.Empty);
                bytes = Convert.FromBase64String(byteToString);

                string rpttype = "";
                switch (rvm.rptType)
                {
                    case "PDF":
                        rpttype = "application/pdf";
                        break;
                    case "Excel":
                        rpttype = "application/vnd.ms-excel";
                        break;
                    default:
                        break;
                }


                return File(bytes, rpttype);
            }
            catch (Exception e)
            {

                throw;
            }

        }
    }
}