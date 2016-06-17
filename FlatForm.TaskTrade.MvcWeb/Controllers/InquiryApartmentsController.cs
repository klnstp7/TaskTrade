using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Peacock.InWork4.Voucher.MvcWebSite.Controllers
{
    public class InquiryApartmentsController : Controller
    {
        //
        // GET: /InquiryApartments/

        public ActionResult InquiryList()
        {
            return View();
        }
        public ActionResult AddInquiry()
        {
            return View();
        }
    }
}
