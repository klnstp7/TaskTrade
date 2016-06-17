using System;
using System.Collections;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Linq;
using Peacock.PEP.Model;
using Peacock.PEP.MvcWebSite.Help;

namespace Peacock.PEP.MvcWebSite.Controllers
{
    public class ParamsController : BaseController
    {
        //
        // GET: /Params/

        public ActionResult Index()
        {
            //var TreeList = ParameterAdapter.GetParameterList().Select(x => new
            //{
            //    id = x.Id,
            //    pId = x.ParentId,
            //    name = x.Name
            //});
            ////ViewBag.Tree = Json(TreeList.Select(x => new
            ////{
            ////    id = x.Id,
            ////    pId = x.ParentId,
            ////    name = x.Name
            ////}), JsonRequestBehavior.AllowGet);
            //string aa = JsonConvert.SerializeObject(TreeList);
            //ViewBag.Tree = HttpUtility.HtmlEncode(aa);
            //ViewBag.Tree2 = HttpUtility.UrlEncode(aa);
            return View();
        }

        public JsonResult GetParameterList()
        {
            var treeList = ParameterAdapter.GetParameterList().Select(x => new
            {
                id = x.Id,
                pId = x.ParentId,
                name = x.Name
            });
            return Json(treeList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetListByParentId(long parentId, bool needRequest)
        {
            if (!needRequest)
                return Json(new ArrayList(), JsonRequestBehavior.AllowGet);
            var result = ParameterAdapter.GetListByParentId(parentId);
            return Json(result.Select(x => new
            {
                x.Id,
                x.Name,
                x.Value,
                CreatedTime = x.CreatedTime.ToString("yyyy-MM-dd HH:mm:ss")
            }), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveParameter(long parentId, string name, string value, long id = 0)
        {
            var param = new ParameterModel()
            {
                Id = id,
                ParentId = parentId,
                Name = name,
                Value = value,
                Title = name
            };
            return ExceptionCatch.Invoke(() =>
            {
                ParameterAdapter.SaveParameter(param);
            });
        }

        [HttpPost]
        public JsonResult DeleteParameter(long id)
        {
            return ExceptionCatch.Invoke(() =>
            {
                ParameterAdapter.Delete(id);
            });
        }

        public JsonResult GetParameterById(long id)
        {
            return Json(ParameterAdapter.GetParameterById(id), JsonRequestBehavior.AllowGet);
        }
    }
}
