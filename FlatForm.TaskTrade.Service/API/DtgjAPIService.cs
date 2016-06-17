using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using Aliyun.OpenServices.OpenStorageService;
using Peacock.Common.Helper;
using Peacock.PEP.Service.Base;

namespace Peacock.PEP.Service
{
    /// <summary>
    /// 调用询价API进行数据获取
    /// </summary>
    public class DtgjAPIService : SingModel<DtgjAPIService>
    {
        private readonly string APIUrl;
        private readonly string UserName;
        private readonly string PassWord;
        private static readonly string cityname = "beijing";
        private static readonly string cachekey = "xunjiatoken";
        private object locklogin = new object();
        private readonly NameValueCollection Citys;
        private string token
        {
            get
            {
                string _token = HttpContext.Current.Cache.Get(cachekey) as string;
                if (_token == null)
                {
                    lock (locklogin)
                    {
                        if (_token == null)
                        {
                            _token = Login();
                            HttpContext.Current.Cache[cachekey] = _token;
                        }
                    }
                }
                _token = HttpContext.Current.Cache.Get(cachekey) as string;
                return _token;
            }
        }
        private void ReloadToken()
        {
            HttpContext.Current.Cache.Remove(cachekey);
        }
        private string Login()
        {
            WebClient client = new WebClient();
            NameValueCollection collection = new NameValueCollection();
            collection.Add("username", UserName);
            collection.Add("password", PassWord);
            client.QueryString = collection;
            Dictionary<string, object> data = null;
            try
            {
                string tagurl = APIUrl + "/" + cityname + "/Login";
                var postresult = client.DownloadData(tagurl);
                var jdata = Encoding.UTF8.GetString(postresult);
                data = new JavaScriptSerializer().DeserializeObject(jdata) as Dictionary<string, object>;

            }
            catch
            {
                throw new Exception("询价服务方接口调用异常");
            }
            var success = (bool)data["Success"];
            if (success == false)
                throw new Exception("询价接口获取令牌失败");
            var token = data["Data"] as string;
            return token;
        }

        private DtgjAPIService()
        {
            Hashtable ApiData = (Hashtable)ConfigurationManager.GetSection("DtgjApiData");
            if (ApiData == null)
            {
                LogHelper.Error("DtgjAPIService:请先配置DtgjApiData节点", new Exception("配置错误"));
            }

            if (!ApiData.Contains("APIUrl"))
            {
                LogHelper.Error("DtgjAPIService:请先配置DtgjApiData下节点:APIUrl", new Exception("配置错误"));
            }
            APIUrl = (string) ApiData["APIUrl"].ToString();

            if (!ApiData.Contains("UserName"))
            {
                LogHelper.Error("DtgjAPIService:请先配置DtgjApiData下节点:UserName", new Exception("配置错误"));
            }
            UserName = (string) ApiData["UserName"].ToString();

            if (!ApiData.Contains("PassWord"))
            {
                LogHelper.Error("DtgjAPIService:请先配置DtgjApiData下节点:PassWord", new Exception("配置错误"));
            }
            PassWord = (string) ApiData["PassWord"].ToString();
            Citys = ConfigurationManager.GetSection("xunjiaCitys") as NameValueCollection;
        }

        public List<string> GetHouseTypeByArea(string address, int area)
        {
            int trycount = 0;
        begin:
            if (string.IsNullOrEmpty(address))
                throw new ServiceException("地址不能为空");
            List<object> urllist = new List<object>()
            {
                cityname,
                address,
                area,
                "UnitShape"
            };
            string tagurl = APIUrl + "/" + string.Join("/", urllist);
            WebClient client = new WebClient();
            NameValueCollection collection = new NameValueCollection();
            collection.Add("token", token);
            client.QueryString = collection;
            byte[] postresult = null;
            try
            {
                postresult = client.DownloadData(tagurl);
            }
            catch
            {
                throw new ServiceException("询价端接口调用异常");
            }
            string jdata = Encoding.UTF8.GetString(postresult);
            var dict = new JavaScriptSerializer().DeserializeObject(jdata) as Dictionary<string, object>;
            bool success = (bool)dict["Success"];
            if (!success)
            {
                //如果token过期，则重新登录
                ReloadToken();
                if (trycount > 2)//如果继续登录3次后还是非法用户，则抛出异常
                    throw new ServiceException("询价接口登录失败");
                trycount++;
                goto begin;
            }
            List<string> result = null;
            try
            {
                result = (dict["Data"] as object[]).Select(x =>
                {
                    var item = x as Dictionary<string, object>;
                    return item["Value"].ToString();
                }).ToList();
            }
            catch
            {
                throw new ServiceException("询价接口返回信息异常");
            }
            return result;
        }

        public List<string> GetSpecialInfo(string address)
        {
            int trycount = 0;
        begin:
            if (string.IsNullOrEmpty(address))
                throw new ServiceException("地址不能为空");
            List<object> urllist = new List<object>()
            {
                cityname,
                address,
                "SpecialFactors"
            };
            string tagurl = APIUrl + "/" + string.Join("/", urllist);
            WebClient client = new WebClient();
            NameValueCollection collection = new NameValueCollection();
            collection.Add("token", token);
            client.QueryString = collection;
            byte[] postresult = null;
            try
            {
                postresult = client.DownloadData(tagurl);
            }
            catch
            {
                throw new ServiceException("询价端接口调用异常");
            }
            string jdata = Encoding.UTF8.GetString(postresult);
            var dict = new JavaScriptSerializer().DeserializeObject(jdata) as Dictionary<string, object>;
            bool success = (bool)dict["Success"];
            if (!success)
            {
                //如果token过期，则重新登录
                ReloadToken();
                if (trycount > 2)//如果继续登录3次后还是非法用户，则抛出异常
                    throw new ServiceException("询价接口登录失败");
                trycount++;
                goto begin;
            }
            List<string> result = null;
            try
            {
                result = (dict["Data"] as object[]).Select(x => x.ToString()).ToList();
            }
            catch
            {
                throw new ServiceException("询价接口返回信息异常");
            }
            return result;
        }

        public string GetPriceDiff(string address)
        {
            int trycount = 0;
            begin:
            if (string.IsNullOrEmpty(address))
                throw new ServiceException("地址不能为空");
            List<object> urllist = new List<object>()
            {
                address,
                "Instruction"
            };
            string tagurl = APIUrl + "/" + string.Join("/", urllist);
            WebClient client = new WebClient();
            NameValueCollection collection = new NameValueCollection();
            collection.Add("token", token);
            collection.Add("count", "1");
            client.QueryString = collection;
            byte[] postresult = null;
            try
            {
                postresult = client.DownloadData(tagurl);
            }
            catch
            {
                throw new ServiceException("询价端接口调用异常");
            }
            string jdata = Encoding.UTF8.GetString(postresult);
            var dict = new JavaScriptSerializer().DeserializeObject(jdata) as Dictionary<string, object>;
            bool success = (bool) dict["Success"];
            if (!success)
            {
                //如果token过期，则重新登录
                ReloadToken();
                if (trycount > 2) //如果继续登录3次后还是非法用户，则抛出异常
                    throw new ServiceException("询价接口登录失败");
                trycount++;
                goto begin;
            }
            string result = string.Empty;
            try
            {
                var infos = dict["Data"] as object[];
                result = string.Join("/", infos);
            }
            catch
            {
                throw new ServiceException("询价接口返回信息异常");
            }
            return result;
        }

        public Tuple<double, int, string> GetFindPriceByMoHu(string propertytype, int housetype, double area, string filter, string cityname,
            string floor_building = null, int? buildedyear = null, int? floor = null, int? maxfloor = null,
            string toward = null, string special_factors = null, int? hall = null, int? toilet = null, string house_number = null, string renovation = null)
        {
            int trycount = 0;
        begin:
            if (Citys.AllKeys.Contains(cityname))
                cityname = Citys[cityname];
            if (string.IsNullOrEmpty(propertytype))
                throw new ServiceException("住宅类型不能为空");
            if (area == default(double))
                throw new ServiceException("面积不能为空");
            if (string.IsNullOrEmpty(filter))
                throw new ServiceException("小区名称或地址不能为空");
            var urllist = new List<object>()
            {
                cityname,
                propertytype,
                housetype,
                area,
                filter,
                "InquiryResults"
            };
            var tagurl = APIUrl + "/" + string.Join("/", urllist);
            var collection = new NameValueCollection();
            if (!string.IsNullOrEmpty(floor_building))
                collection["floor_building"] = floor_building;
            if (buildedyear.HasValue)
                collection["builted_time"] = buildedyear.Value.ToString();
            if (floor.HasValue)
                collection["floor"] = floor.Value.ToString();
            if (maxfloor.HasValue)
                collection["totalfloor"] = maxfloor.Value.ToString();
            if (!string.IsNullOrEmpty(toward) && toward.Length <= 8)
                collection["toward"] = HttpUtility.UrlEncode(toward);
            if (!string.IsNullOrEmpty(special_factors))
                collection["special_factors"] = HttpUtility.UrlEncode(special_factors);
            if (hall.HasValue)
                collection["hall"] = hall.Value.ToString();
            if (toilet.HasValue)
                collection["toilet"] = toilet.Value.ToString();
            if (!string.IsNullOrEmpty(house_number))
                collection["house_number"] = house_number;
            if (!string.IsNullOrEmpty(renovation))
                collection["renovation"] = HttpUtility.UrlEncode(renovation);
            collection["token"] = token;

            var client = new WebClient();
            client.QueryString = collection;
            byte[] postresult = null;
            try
            {
                postresult = client.DownloadData(tagurl);
            }
            catch
            {
                throw new ServiceException("询价服务方接口调用异常");
            }
            string jdata = Encoding.UTF8.GetString(postresult);
            var dict = new JavaScriptSerializer().DeserializeObject(jdata) as Dictionary<string, object>;
            var success = (bool)dict["Success"];
            object data = dict["Data"];
            if (!success)
            {
                //如果token过期，则重新登录
                ReloadToken();
                if (trycount > 2)//如果继续登录3次后还是非法用户，则抛出异常
                    throw new ServiceException("询价接口登录失败");
                trycount++;
                goto begin;
            }
            var priceresult = data as Dictionary<string, object>;
            if (priceresult.ContainsKey("price") && priceresult.ContainsKey("totalprice"))
            {
                double price;//单价
                int totalprice;//总价
                string guid = priceresult["guid"].ToString();
                if (double.TryParse(priceresult["price"].ToString(), out price) && int.TryParse(priceresult["totalprice"].ToString(), out totalprice))
                {
                    return Tuple.Create(price, totalprice, guid);
                }
            }
            throw new ServiceException("询价接口数据异常");
        }

        public Tuple<double, double, double, double> GetPriceBetween(string cityname, string address, int area, string propertytype)
        {
            int trycount = 0;
        begin:
            if (Citys.AllKeys.Contains(cityname))
                cityname = Citys[cityname];
            if (string.IsNullOrEmpty(address))
                throw new ServiceException("地址不能为空");
            if (area == default(int))
                throw new ServiceException("面积不能为0");
            if (string.IsNullOrEmpty(propertytype))
                throw new ServiceException("住宅类型不能为空");
            var urllist = new List<object>()
            {
                cityname,
                propertytype,
                area,
                address,
                "InquiryResultsWithMortgage"
            };
            string tagurl = APIUrl + "/" + string.Join("/", urllist);
            var client = new WebClient();
            var collection = new NameValueCollection();
            collection.Add("token", token);
            client.QueryString = collection;
            byte[] postresult = null;
            try
            {
                postresult = client.DownloadData(tagurl);
            }
            catch
            {
                throw new ServiceException("询价端接口调用异常");
            }
            string jdata = Encoding.UTF8.GetString(postresult);
            var dict = new JavaScriptSerializer().DeserializeObject(jdata) as Dictionary<string, object>;
            var success = (bool)dict["Success"];
            if (!success)
            {
                ReloadToken();
                if (trycount > 2)//如果继续登录3次后还是非法用户，则抛出异常
                    throw new ServiceException("询价接口登录失败");
                trycount++;
                goto begin;
            }
            Tuple<double, double, double, double> result = null;
            try
            {
                dynamic data = dict["Data"];
                double max = Convert.ToDouble(data["mortgageUnitPrice"]["maxPrice"]);
                double min = Convert.ToDouble(data["mortgageUnitPrice"]["minPrice"]);
                double totalmax = Convert.ToDouble(data["mortgageTotalPrice"]["maxPrice"]);
                double totalmin = Convert.ToDouble(data["mortgageTotalPrice"]["minPrice"]);
                result = Tuple.Create(max, min, totalmax, totalmin);
            }
            catch
            {
                throw new ServiceException("询价端返回结果异常");
            }
            return result;
        }

        public List<Tuple<string, string, double, double, string>> GetResidential(string cityname, string address, int count)
        {
            int trycount = 0;
        begin:
            if (string.IsNullOrEmpty(address))
                throw new ServiceException("请填写查询地址");
            if (Citys.AllKeys.Contains(cityname))
                cityname = Citys[cityname];
            var urllist = new List<object>()
            {
                cityname,
                address,
                "VillageInfo"
            };
            string tagurl = APIUrl + "/" + string.Join("/", urllist);
            var client = new WebClient();
            var collection = new NameValueCollection();
            collection.Add("token", token);
            collection.Add("count", count.ToString());
            client.QueryString = collection;
            byte[] postresult = null;
            try
            {
                postresult = client.DownloadData(tagurl);
            }
            catch
            {
                throw new ServiceException("询价端接口调用异常");
            }
            string jdata = Encoding.UTF8.GetString(postresult);
            dynamic dict = new JavaScriptSerializer().DeserializeObject(jdata);
            bool success = dict["Success"];
            if (!success)
            {
                //如果token过期，则重新登录
                ReloadToken();
                if (trycount > 2)//如果继续登录3次后还是非法用户，则抛出异常
                    throw new ServiceException("询价接口登录失败");
                trycount++;
                goto begin;
            }
            var result = new List<Tuple<string, string, double, double, string>>();
            try
            {
                foreach (dynamic item in dict["Data"])
                {
                    var cName = item["cName"].ToString();
                    var cAddress = item["cAddress"].ToString();
                    var pointx = Convert.ToDouble(item["XLongitude"]);
                    var pointy = Convert.ToDouble(item["Ylatitude"]);
                    var districtname = item["DistrictName"].ToString();
                    var temp = Tuple.Create(cName, cAddress, pointx, pointy, districtname);
                    result.Add(temp);
                }
            }
            catch
            {
                throw new ServiceException("询价端接返回数据异常");
            }
            return result;
        }

        public bool FeedbackMessage(string message)
        {
            Hashtable ApiData = (Hashtable)ConfigurationManager.GetSection("FeedbackApiData");
            if (ApiData == null)
            {
                LogHelper.Error("DtgjAPIService:请先配置FeedbackApiData节点", new Exception("配置错误"));
            }

            if (!ApiData.Contains("APIUrl"))
            {
                LogHelper.Error("DtgjAPIService:请先配置FeedbackApiData下节点:APIUrl", new Exception("配置错误"));
            }
            var APIUrl = (string)ApiData["APIUrl"].ToString();

            if (!ApiData.Contains("UserKeyId"))
            {
                LogHelper.Error("DtgjAPIService:请先配置FeedbackApiData下节点:UserKeyId", new Exception("配置错误"));
            }
            var UserKeyId = (string)ApiData["UserKeyId"].ToString();

            if (!ApiData.Contains("UserAccessKey"))
            {
                LogHelper.Error("DtgjAPIService:请先配置FeedbackApiData下节点:UserAccessKey", new Exception("配置错误"));
            }
            var UserAccessKey = (string)ApiData["UserAccessKey"].ToString();

            if (!ApiData.Contains("CityName"))
            {
                LogHelper.Error("DtgjAPIService:请先配置FeedbackApiData下节点:CityName", new Exception("配置错误"));
            }
            var CityName = (string)ApiData["CityName"].ToString();
            WebClient client = new WebClient();
            NameValueCollection collection = new NameValueCollection();
            collection.Add("CityName", CityName);
            collection.Add("SubmitSystemName", "估值系统");
            collection.Add("FeedbackMessage", message);
            collection.Add("UserKeyId", UserKeyId);
            collection.Add("UserAccessKey", UserAccessKey);
            byte[] postresult = null;
            try
            {
                postresult = client.UploadValues(APIUrl, "POST", collection);
            }
            catch
            {
                throw new ServiceException("反馈接口调用失败");
            }
            bool success = false;
            try
            {
                var result = Encoding.UTF8.GetString(postresult);
                var jdata = new JavaScriptSerializer().DeserializeObject(result);
                var temp = (jdata as Dictionary<string, object>)["result"];
                success = Convert.ToBoolean(temp);
            }
            catch
            {
                throw new ServiceException("反馈接口返回信息异常");
            }
            return success;
        }
    }
}
