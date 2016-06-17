using Peacock.PEP.Model;
using Peacock.PEP.Model.Condition;
using Peacock.PEP.MvcWebSite.Controllers;
using ResourceLibraryExtension.Helper;
using ResourceLibraryExtension.Untity.Enum;
using System.Linq;
using System.Web.Mvc;

namespace Peacock.InWork4.Voucher.MvcWebSite.Controllers
{
    public class CompanyCheckController : BaseController
    {
        //
        // GET: /CompanyCheck/

        public ActionResult CompanyCheck()
        {
            return View();
        }

        public JsonResult GetCompanyData(CompanyCondition condition, int index, int rows)
        {
            int total;
            var result = CompanyService.GetCompanyList(condition, index, rows, out total);
            return Json(new
            {
                rows = result.Select(x => new
                {
                    x.Id,
                    x.CompanyName,
                    x.Address,
                    x.UserName,
                    x.Tel,
                    x.Contact,
                    x.City,
                    x.Reson,
                    x.IsApprove
                }),
                total
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCompany(long id)
        {
            ResultInfo result = new ResultInfo();
            result = CompanyService.GetCompany(id);
            if(result.Others != null)
            {
                long resource = result.Others;
                result.Others = SingleFileManager.GetFileUrl(resource, ResourceImageSize.中图);
            }            
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ApproveCompany(long id)
        {
            ResultInfo result = new ResultInfo();
            result = CompanyService.ApproveCompany(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteCompany(long id)
        {
            ResultInfo result = new ResultInfo();
            result = CompanyService.DeleteCompany(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveReson(long id,string reson)
        {
            ResultInfo result = CompanyService.SaveReson(id, reson);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
