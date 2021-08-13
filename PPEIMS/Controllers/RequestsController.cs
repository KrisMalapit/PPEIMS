using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DNTBreadCrumb.Core;
using Microsoft.AspNetCore.Mvc;
using PPEIMS.Models;

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
    }
}