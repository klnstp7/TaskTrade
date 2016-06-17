using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Peacock.PEP.Model.Condition;
using Peacock.PEP.Model.DTO;
using System.IO;
using ResourceLibraryExtension.Helper;
using Newtonsoft.Json;
using Peacock.PEP.Model;

namespace Peacock.PEP.MvcWebSite.Controllers
{
    public class CommonController : BaseController
    {
        /// <summary>
        /// 获取省份
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public JsonResult GetProvince()
        {
            IList<RegionModel> list= RegionService.GetAll().Where(x=> x.ParentId == null).ToList();
            var result = list.Select(x => new { Id = x.Id, Name = x.Name});
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据父ID获取子项
        /// </summary>
        /// <param name="parentid"></param>
        /// <returns></returns>
        public JsonResult GetRegionByParentId(long parentid)
        {
            IList<RegionModel> list = RegionService.GetAll().Where(x => x.ParentId == parentid).ToList();
            var result = list.Select(x => new
            {
                Id = x.Id,
                Name = x.Name
            });
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据省份查找城市
        /// </summary>
        /// <param name="parentid"></param>
        /// <returns></returns>
        public JsonResult GetCityByParentName(string parentname)
        {
            IList<RegionModel> list = RegionService.GetAll();
            RegionModel parent = list.SingleOrDefault(x => x.Name == parentname && x.RegionType == "province");
            long parentId = -1;
            if (parent != null) parentId = parent.Id;
            var result = list.Where(x => x.ParentId == parentId).Select(x => new
            {
                Id = x.Id,
                Name = x.Name
            });
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据城市查找县区
        /// </summary>
        /// <param name="parentid"></param>
        /// <returns></returns>
        public JsonResult GetRegionByParentName(string parentname)
        {
            IList<RegionModel> list = RegionService.GetAll();
            RegionModel parent = list.SingleOrDefault(x => x.Name == parentname && x.RegionType == "city");
            long parentId = -1;
            if (parent != null) parentId = parent.Id;
            var result = list.Where(x => x.ParentId == parentId).Select(x => new
            {
                Id = x.Id,
                Name = x.Name
            });
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取分类参数列表
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public JsonResult GetParamJsonList(string name)
        {
            IList<ParameterModel> paramList = new List<ParameterModel>();
            var parentParameter = ParameterAdapter.GetParameterTree(name);
            if (parentParameter != null)
            {
                paramList = ParameterAdapter.GetListByParentId(parentParameter.Id);
            }
            return Json(paramList, JsonRequestBehavior.AllowGet);
        }
    }
}
