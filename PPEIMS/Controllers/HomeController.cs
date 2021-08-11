using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PPEIMS.Models;
using DNTBreadCrumb.Core;
using Microsoft.AspNetCore.Mvc.Rendering;
using PPEIMS.Models.View_Model;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.IO;
using System.Text;
using Microsoft.Extensions.Logging;

namespace PPEIMS.Controllers
{
    [Authorize]
    [BreadCrumb(Title = "Home", UseDefaultRouteUrl = true, Order = 0, IgnoreAjaxRequests = true)]
    public class HomeController : Controller
    {

        //private readonly ILogger<HomeController> _logger;
        private readonly PPEIMSContext _context;

        public HomeController(PPEIMSContext context
            //, ILogger<HomeController> logger
            )
        {
            //_logger = logger;
            _context = context;
        }
        [BreadCrumb(Title = "Index", Order = 1, IgnoreAjaxRequests = true)]
        public IActionResult Index()
        {
            string companyAccess = User.Identity.GetCompanyAccess();
            int[] compId = companyAccess.Split(',').Select(n => Convert.ToInt32(n)).ToArray();


           
            return View();
        }



    }

}
