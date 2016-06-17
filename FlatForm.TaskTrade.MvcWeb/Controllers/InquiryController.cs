using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using NPOI.HSSF.Record.Chart;
using Peacock.PEP.Model;
using Peacock.PEP.Model.Condition;
using Microsoft.International.Converters.PinYinConverter;
using Peacock.PEP.MvcWebSite.Help;
using Peacock.PMS.Service.Services;
using Peacock.Common.Helper;

namespace Peacock.PEP.MvcWebSite.Controllers
{
    public class InquiryController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddInquiry()
        {
            var list = BaseAPIService.GetEnumInfoByType("北京市", "户型,主体朝向,装修");
            ViewBag.HouseTypeList = list.First(x => x.Desc == "户型").EnumItem;
            ViewBag.TowordList = list.First(x => x.Desc == "主体朝向").EnumItem;
            ViewBag.DecorationList = list.First(x => x.Desc == "装修").EnumItem;
            var citys = ConfigurationManager.GetSection("xunjiaCitys") as NameValueCollection;
            ViewBag.Citys = citys;
            var user = UserService.GetUser();
            ViewBag.Company = user.CompanyName;
            var usercity = CurrentCity;
            ViewBag.UserCity = citys.AllKeys.Contains(usercity) ? usercity : "北京市";
            ViewBag.PropertyTypeList = GetSelectList("物业类型");
            ViewBag.InquirySourceList = GetSelectList("询价来源");
            ViewBag.SpecialInfoList = GetSelectList("特殊因素");

            return View();
        }

        /// <summary>
        /// 获取询价信息列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetInquiryList(InquiryCondition condition, int index, int rows)
        {
            int total;
            var result = InquiryService.GetInquiryList(condition, index, rows, out total);
            //var creatorIds = result.Select(x => x.CreatorId).ToList();
            //var creators = UserService.GetUserByIds(creatorIds);
            return Json(new
            {
                rows = result.Select(x => new
                {
                    x.Id,
                    x.ResidentialAreaName,
                    x.Address,
                    MortgageResult = x.MortgageResult.HasValue ? x.MortgageResult.Value.ToString("F2") : "",
                    InquiryPrice = x.InquiryPrice.HasValue?x.InquiryPrice.Value.ToString("F2"):"",
                    InquiryResult = x.InquiryResult.HasValue ? x.InquiryResult.Value.ToString("F2") : "",
                    BuildingArea = x.BuildingArea.HasValue ? x.BuildingArea.Value.ToString("F2") : "",
                    x.Floor,
                    x.MaxFloor,
                    x.InquirySource,
                    //Bank = x.Customer != null ? x.Customer.Bank : "",
                    CustomerName = x.Customer != null ? x.Customer.CustomerName : "",
                    x.CreateTime,
                    x.CreatorName, //UserService.GetUser(x.CreatorId).UserName,
                    IsToProject = x.IsToProject ? "是" : "否"
                }),
                total
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取询价信息列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetInquiry(int inquiryId)
        {
            var result = InquiryService.GetInquiry(inquiryId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRegion(string cityName)
        {
            var result = InquiryService.GetRegion(cityName);
            return Json(result);
        }

        public JsonResult GetCityByRegion(string regionName)
        {
            var cityName = InquiryService.GetCityByRegion(regionName);
            return Json(cityName, JsonRequestBehavior.AllowGet);
        }

        public ActionResult InquriyPrint()
        {
            return View();
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <returns></returns>
        public ActionResult Print()
        {
            return View();
        }

        /// <summary>
        /// 询价单保存
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="inquiry"></param>
        /// <param name="OnlineBusinessId"></param>
        /// <param name="isToProject"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveInquiry(CustomerModel customer, InquiryModel inquiry, long? OnlineBusinessId,bool isToProject = false)
        {
            //if (inquiry.Id == 0 && !string.IsNullOrEmpty(inquiry.FeedbackMessage))
            //{
            //    System.Threading.Tasks.Task.Factory.StartNew(() =>
            //    {
            //        DtgjAPIService.FeedbackMessage(inquiry.FeedbackMessage);
            //    });
            //}
            return ExceptionCatch.Invoke(() =>
            {
                var result = InquiryService.SaveInquiry(inquiry, customer);
                if (isToProject)
                {
                    LogHelper.Ilog("Inquiry?CustomerModel=" + customer.ToJson() + "&InquiryModel=" + inquiry.ToJson(), "询价直接转立项");
                }
                if (OnlineBusinessId.HasValue && OnlineBusinessId > 0)
                {
                    InquiryService.SaveInquiryFromOnline(OnlineBusinessId.Value, result);
                }
                return result;
            }, x => "操作成功!");
        }

      

        #region 基础库案例信息
        /// <summary>
        /// 评估报告案例
        /// </summary>
        /// <returns></returns>
        public JsonResult GetHouseAppraisalCaseList(string residentialAreaName, string address, string residentialAreaId,
            string cityName, decimal? begin, decimal? end, bool isDefault)
        {
            string message;
            //记录数
            int resultCount = 0;
            var result =
                BaseAPIService.GetHouseAppraisalCaseList(residentialAreaName, address, residentialAreaId, cityName,
                    out message);
            if (result == null)
                return Json(new { resultCount = 0 }, JsonRequestBehavior.AllowGet);
            resultCount = result.Length;
            var temp = result = result.OrderByDescending(x => x.CaseTime).ToArray();
            if (begin.HasValue && end.HasValue)
            {
                result =
                    result.OrderByDescending(x => x.CaseTime)
                        .Where(x => x.BuildingArea >= begin.Value && x.BuildingArea <= end.Value)
                        .ToArray();
                if (isDefault && !result.Any())
                    result = temp;
            } 
            if (result.Count() > 5)
                result = result.Take(5).ToArray();
            return Json(new{
                rows=result.Select(x => new
                {
                    x.Address,
                    x.DistrictName,
                    x.UnitShape,
                    x.BuildingCompletedYear,
                    DateofValue = x.DateofValue.HasValue ? x.DateofValue.Value.ToString("yyyy-MM-dd") : string.Empty,
                    x.ResidentialAreaName,
                    BuildingArea = x.BuildingArea.ToString("F2"),
                    x.Floors,
                    x.Floor,
                    x.Toward,
                    UnitPrice = x.UnitPrice.ToString("0"),
                    TotalPrice = (x.TotalPrice/10000).ToString("0"),
                    CaseTime = x.CaseTime.HasValue ? x.CaseTime.Value.ToString("yyyy-MM-dd") : "",
                
                }),
                resultCount
            });
        }

        /// <summary>
        /// 二手房交易案例
        /// </summary>
        /// <returns></returns>
        public JsonResult GetHouseResoldCaseList(string residentialAreaName, string address, string residentialAreaId,
            string cityName, decimal? begin, decimal? end, string caseFrom, bool isDefault)
        {
            caseFrom = Server.UrlDecode(caseFrom);
            //记录数
            int resultCount = 0;
            string message;
            var data = BaseAPIService.GetHouseResoldCaseList(residentialAreaName, address, residentialAreaId, cityName,
                out message);
            if (data == null)
                return Json(new { resultCount = 0 }, JsonRequestBehavior.AllowGet);
            resultCount = data.Length;
            var result = data.OrderByDescending(x => x.CaseTime).ToArray();
            if (begin.HasValue && end.HasValue)
            {
                result = result.Where(x => x.BuildingArea >= begin.Value && x.BuildingArea <= end.Value).ToArray();
                if (isDefault && !result.Any())
                    result = data.OrderByDescending(x => x.CaseTime).ToArray();
            }
            if (!string.IsNullOrEmpty(caseFrom) && caseFrom != "undefined" && caseFrom != "全部")
                result = result.Where(x => x.CaseFrom == caseFrom).ToArray();
            var caseFromData = data.Select(x => x.CaseFrom).Distinct().Select(x => new {value = x}).ToList();
            caseFromData.Insert(0, new {value = "全部"});
            if (result.Count() > 5)
                result = result.Take(5).ToArray();
            return Json(new
            {
                rows = result.Select(x => new
                {
                    x.DistrictName,
                    x.UnitShape,
                    x.BuildingCompletedYear,
                    x.CaseFrom,
                    x.ResidentialAreaName,
                    BuildingArea = x.BuildingArea.ToString("F2"),
                    x.Floors,
                    x.Floor,
                    x.Toward,
                    UnitPrice = x.UnitPrice.ToString("0"),
                    TotalPrice = (x.TotalPrice/10000).ToString("0"),
                    CaseTime = x.CaseTime.HasValue ? x.CaseTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
                }),
                begin,
                end,
                caseFromData,
                caseFrom,
                resultCount  
            });
        }

        /// <summary>
        /// 二手房报盘案例
        /// </summary>
        /// <returns></returns>
        public JsonResult GetOfferForSaleList(string residentialAreaName, string address, string residentialAreaId,
            string cityName, string order, decimal? begin, decimal? end, string caseFrom, bool isDefault)
        {
            caseFrom = Server.UrlDecode(caseFrom);
            //记录数
            int resultCount = 0;
            string message;
            var data = BaseAPIService.GetOfferForSaleList(residentialAreaName, address, residentialAreaId, cityName,
                out message);
            if (data == null)
                return Json(new { resultCount = 0 }, JsonRequestBehavior.AllowGet);
            resultCount = data.Length;
            var result = data.OrderByDescending(x => x.CaseTime).ToArray();
            if (begin.HasValue && end.HasValue)
            {
                result = result.Where(x => x.BuildingArea >= begin.Value && x.BuildingArea <= end.Value).ToArray();
                if (isDefault && !result.Any())
                    result = data.OrderByDescending(x => x.CaseTime).ToArray();
            }
            if (!string.IsNullOrEmpty(caseFrom) && caseFrom != "undefined" && caseFrom != "全部")
                result = result.Where(x => x.CaseFrom == caseFrom).ToArray();
            if (order == "desc")
                result = result.OrderByDescending(x => x.CaseFrom).ToArray();
            if (order == "asc")
                result = result.OrderBy(x => x.CaseFrom).ToArray();
            var caseFromData = data.Select(x => x.CaseFrom).Distinct().Select(x => new {value = x}).ToList();
            caseFromData.Insert(0, new {value = "全部"});
            if (result.Count() > 5)
                result = result.Take(5).ToArray();
            return Json(new
            {
                rows = result.Select(x => new
                {
                    x.CaseFrom,
                    x.ResidentialAreaName,
                    BuildingArea = x.BuildingArea.ToString("F2"),
                    x.Floors,
                    x.Floor,
                    x.Toward,
                    UnitPrice = x.UnitPrice.ToString("0"),
                    TotalPrice = (x.TotalPrice/10000).ToString("0"),
                    CaseTime = x.CaseTime.HasValue ? x.CaseTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
                    x.DistrictName,
                    x.UnitShape,
                    x.BuildingCompletedYear,
                    x.SourceLink
                }),
                begin,
                end,
                caseFromData,
                caseFrom,
                resultCount
            });
        }
        #endregion

        #region 基础库住宅数据
        /// <summary>
        /// 根据给出的提示(拼音、楼盘名简写等)返回楼盘名称集合
        /// </summary>
        public JsonResult GetXiaoquList(string data, string cityname)
        {
            data = HttpUtility.UrlDecode(data);
            cityname = HttpUtility.UrlDecode(cityname);
            var result = BaseAPIService.GetResidentialArea(data, cityname, 10);
            if (result == null)
                return Json(null, JsonRequestBehavior.AllowGet);
            return Json(result.Select(x => new
            {
                x.ResidentialAreaName,
                x.Address,
                xingzhengqu = x.DistrictFullName,
                residentialAreaId = x.ResidentialAreaID
            }), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetLoudong(string address, string cityname)
        {
            var bllsource = BaseAPIService.GetHouseBuildings(cityname, address, cityname);
            var result = bllsource.Select(x =>
            {
                long Key = x.TID;
                string Value = x.BuildingName;
                List<string> list = new List<string>()
                {
                    string.Empty
                };
                if (!string.IsNullOrEmpty(Value))
                {
                    foreach (var item in Value)
                    {
                        List<string> templist = new List<string>();
                        ChineseChar chinese = null;
                        try
                        {
                            chinese = new ChineseChar(item);
                            var source = chinese.Pinyins.Where(y => y != null);
                            templist = source.Select(y => Regex.Replace(y, "[\\d]", "").ToUpper()).Concat
                            (
                                source.Select(y => y.First().ToString().ToUpper())
                            ).GroupBy(y => y).Select(y => y.Key).ToList();
                        }
                        catch
                        {
                            templist.Add(item.ToString());
                        }
                        list = list.SelectMany(y => templist.Select(z => y + z)).ToList();
                    }
                }
                return new
                {
                    Key,
                    Value,
                    Pingyin = list
                };
            });
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDanyuanHao(long buildid, string cityname)
        {
            var result = BaseAPIService.GetHouseUnits(cityname, buildid);
            return Json(result.Select(x => new { Key = x.TID, Value = x.UnitName }), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetHuming(long buildid, string cityname)
        {
            var result = BaseAPIService.GetHouseNames(cityname, buildid, 0).Select(x =>
            {
                var item = new
                {
                    floor = x.Floor,
                    maxfloor = x.FloorCount
                };
                var aaa = Json(item).ToString();
                return new
                {
                    Key = new JavaScriptSerializer().Serialize(item),
                    Value = x.HouseName
                };
            });
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 询价接口数据
        public JsonResult GetHouseTypeByArea(string address, int area)
        {
            var result = DtgjAPIService.GetHouseTypeByArea(address, area);
            return Json(result.Select(x => new { Key = x }), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSpecialInfo(string address)
        {
            var result = DtgjAPIService.GetSpecialInfo(address);
            return Json(result.Select(x => new { Key = x }), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPriceDiff(string address)
        {
            var result = DtgjAPIService.GetPriceDiff(address);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 调用询价接口进行询价
        /// </summary>
        /// <returns></returns>
        public JsonResult GetFindPriceByMoHu(string propertytype, string housetype, double buildingarea, string residentialareaname, string city, int? floor, int? maxfloor, int? buildedyear, string toward, string specialinfo, string renovation)
        {
            if (!string.IsNullOrEmpty(housetype))
            {
                var houseType = (Hashtable)ConfigurationSettings.GetConfig("HouseType");
                if (houseType == null)
                    throw new Exception("请先配置HouseType节点");
                if (houseType.Contains(housetype.Trim()))
                    housetype = (string)houseType[housetype.Trim()].ToString();
            }
            var priceResult = DtgjAPIService.GetFindPriceByMoHu(propertytype, housetype == "" ? 0 : Convert.ToInt32(housetype), buildingarea, residentialareaname, city, buildedyear: buildedyear, floor: floor, maxfloor: maxfloor, toward: toward, special_factors: specialinfo, renovation: renovation);
            var priceBetweenResult = DtgjAPIService.GetPriceBetween(city, residentialareaname, Convert.ToInt32(buildingarea), propertytype);
            var priceDiffResult = DtgjAPIService.GetPriceDiff(residentialareaname);
            var price = priceResult.Item1;//单价
            var totalPrice = priceResult.Item2;//总价(�
            var id = priceResult.Item3;//询价ID
            var maxPrice = Convert.ToInt32(priceBetweenResult.Item1);//最高价
            var minPrice = Convert.ToInt32(priceBetweenResult.Item2);//最低价
            var totalMaxPrice = priceBetweenResult.Item3.ToString("f2");//总价最大
            var totalMinPrice = priceBetweenResult.Item4.ToString("f2");//总价最小
            var infos = priceDiffResult;//价格差异说明;
            double x = 0;
            double y = 0;
            if (totalPrice != 0 | price != 0 | maxPrice != 0 | minPrice != 0 | !string.IsNullOrEmpty(infos))
            {
                var site = DtgjAPIService.GetResidential(city, residentialareaname, 1).FirstOrDefault();
                if (site != null)
                {
                    x = site.Item3;
                    y = site.Item4;
                }
            }
            return Json(new
            {
                price,
                totalPrice,
                maxPrice,
                minPrice,
                infos,
                totalMaxPrice,
                totalMinPrice,
                x,
                y,
                id
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}

