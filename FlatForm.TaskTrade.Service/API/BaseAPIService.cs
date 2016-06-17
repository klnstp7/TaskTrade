using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Web;
using GHSoft.Helper;
using Peacock.Common.Helper;
using Peacock.PEP.Model.ApiModel;
using GHSoft.DTO2;
using Peacock.PEP.Service.Base;

namespace Peacock.PEP.Service
{
    /// <summary>
    /// 调用基础库API进行数据获取
    /// </summary>
    public class BaseAPIService : SingModel<BaseAPIService>
    {
        private readonly string APIUrl;
        private readonly string UserName;
        private readonly string PassWord;
        private readonly NameValueCollection Citys;

        private BaseAPIService()
        {
            Hashtable ApiData = (Hashtable) ConfigurationManager.GetSection("BaseApiData");
            if (ApiData == null)
            {
                LogHelper.Error("BaseAPIService:请先配置BaseApiData节点", new Exception("配置错误"));
            }

            if (!ApiData.Contains("APIUrl"))
            {
                LogHelper.Error("BaseAPIService:请先配置BaseApiData下节点:APIUrl", new Exception("配置错误"));
            }
            APIUrl = (string) ApiData["APIUrl"].ToString();

            if (!ApiData.Contains("UserName"))
            {
                LogHelper.Error("BaseAPIService:请先配置BaseApiData下节点:UserName", new Exception("配置错误"));
            }
            UserName = (string) ApiData["UserName"].ToString();

            if (!ApiData.Contains("PassWord"))
            {
                LogHelper.Error("BaseAPIService:请先配置BaseApiData下节点:PassWord", new Exception("配置错误"));
            }
            PassWord = (string) ApiData["PassWord"].ToString();
            Citys = ConfigurationManager.GetSection("xunjiaCitys") as NameValueCollection;
        }

        #region 案例信息
        /// <summary>
        /// 获取房屋评估案例 
        /// </summary>
        /// <param name="residentialAreaName">小区名称</param>
        /// <param name="address">地址</param>
        /// <param name="address">城市名称</param>
        /// <returns></returns>
        public HouseAppraisalCaseDTO[] GetHouseAppraisalCaseList(string residentialAreaName, string address, string residentialAreaId, string cityName, out string msg)
        {
            LogHelper.Ilog("GetHouseAppraisalCaseList?residentialAreaName=" + residentialAreaName + "&address=" + address + "&residentialAreaId=" + residentialAreaId + "&cityName=" + cityName, "报告案例- " + Instance.ToString());
            msg = string.Empty;
            HouseAppraisalCaseDTO[] result = null;
            if (Citys == null || !Citys.AllKeys.Contains(cityName))
            {
                LogHelper.Error("GetHouseSoldCaseList:请先配置xunjiaCitys节点:" + cityName, new Exception("配置错误"));
                return result;
            }
            var APIUrl = this.APIUrl + Citys[cityName] + "/api";
            IDictionary<string, string> paramsList = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(residentialAreaId))
                paramsList.Add("residentialAreaId", residentialAreaId.Trim());
            paramsList.Add("residentialAreaName", residentialAreaName.Trim());
            //paramsList.Add("address", address.Trim());
            paramsList.Add("pagesize", "20");
            string ApiUrl = WebApiHelper.WebApiAddress(APIUrl, "houseAppraisalCase", paramsList);
            string Token = GetPeacockBaseToken(cityName, APIUrl, UserName, PassWord);
            WebOutputResult<List<HouseAppraisalCaseDTO>> resultValue = WebApiHelper.GetDataForJson<WebOutputResult<List<HouseAppraisalCaseDTO>>>(ApiUrl, Token);
            if (!resultValue.Status.Success)
            {
                msg = resultValue.Status.Message;
            }
            if (resultValue.ResultData != null)
            {
                result = resultValue.ResultData.ToArray();
            }
            return result;
        }

        /// <summary>
        /// 获取二手房出售案例
        /// </summary>
        /// <param name="residentialAreaName">小区名称</param>
        /// <param name="address">地址</param>
        /// <param name="address">城市名称</param>
        /// <returns></returns>
        public HouseResoldCaseDTO[] GetHouseResoldCaseList(string residentialAreaName, string address, string residentialAreaId, string cityName, out string msg)
        {
            LogHelper.Ilog("GetHouseResoldCaseList?residentialAreaName=" + residentialAreaName + "&address=" + address + "&residentialAreaId=" + residentialAreaId + "&cityName=" + cityName, "成交案例-" + Instance.ToString());
            msg = string.Empty;
            HouseResoldCaseDTO[] result = null;
            if (Citys == null || !Citys.AllKeys.Contains(cityName))
            {
                LogHelper.Error("GetHouseSoldCaseList:请先配置xunjiaCitys节点:" + cityName, new Exception("配置错误"));
                return result;
            }
            var APIUrl = this.APIUrl + Citys[cityName] + "/api";
            IDictionary<string, string> paramsList = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(residentialAreaId))
                paramsList.Add("residentialAreaId", residentialAreaId.Trim());
            paramsList.Add("residentialAreaName", residentialAreaName.Trim());
            //paramsList.Add("address", address.Trim());
            paramsList.Add("pagesize", "20");
            string ApiUrl = WebApiHelper.WebApiAddress(APIUrl, "houseResoldCase", paramsList);
            string Token = GetPeacockBaseToken(cityName, APIUrl, UserName, PassWord);
            WebOutputResult<List<HouseResoldCaseDTO>> resultValue = WebApiHelper.GetDataForJson<WebOutputResult<List<HouseResoldCaseDTO>>>(ApiUrl, Token);
            if (!resultValue.Status.Success)
            {
                msg = resultValue.Status.Message;
            }
            if (resultValue.ResultData != null)
            {
                result = resultValue.ResultData.ToArray();
            }
            return result;
        }


        /// <summary>
        /// 获取报盘数据
        /// </summary>
        /// <param name="residentialAreaName">小区名称</param>
        /// <param name="address">地址</param>
        /// <param name="CtiyName">城市名称</param>
        /// <returns></returns>
        public ApiModelBaoPan[] GetOfferForSaleList(string residentialAreaName, string address, string residentialAreaId, string cityName, out string msg)
        {
            LogHelper.Ilog("GetOfferForSaleList?residentialAreaName=" + residentialAreaName + "&address=" + address + "&residentialAreaId=" + residentialAreaId + "&cityName=" + cityName, "报盘案例-" + Instance.ToString());
            msg = string.Empty;
            ApiModelBaoPan[] result = null;
            if (Citys == null || !Citys.AllKeys.Contains(cityName))
            {
                LogHelper.Error("GetOfferForSaleList:请先配置xunjiaCitys节点:" + cityName, new Exception("配置错误"));
                return result;
            }
            var APIUrl = this.APIUrl + Citys[cityName] + "/api";
            IDictionary<string, string> paramsList = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(residentialAreaId))
                paramsList.Add("residentialAreaId", residentialAreaId.Trim());
            paramsList.Add("residentialAreaName", residentialAreaName.Trim());
            //paramsList.Add("address", address.Trim());
            paramsList.Add("pagesize", "20");
            string ApiUrl = WebApiHelper.WebApiAddress(APIUrl, "houseOfferForSale", paramsList);
            string Token = GetPeacockBaseToken(cityName, APIUrl, UserName, PassWord);
            WebOutputResult<List<ApiModelBaoPan>> ResultData = WebApiHelper.GetDataForJson<WebOutputResult<List<ApiModelBaoPan>>>(ApiUrl, Token);

            if (!ResultData.Status.Success)
            {
                msg = ResultData.Status.Message;
            }
            return ResultData.ResultData == null ? null : ResultData.ResultData.ToArray();
        }
        #endregion

        #region 住宅数据
        /// <summary>
        /// 根据给出的提示(拼音、楼盘名简写等)返回楼盘名称集合
        /// </summary>
        /// <param name="cue">部分小区信息</param>
        /// <param name="cityName">城市名称</param>
        /// <param name="count">指定数量</param>
        /// <returns></returns>
        public ApiModelResidentialArea[] GetResidentialArea(string cue, string cityName, int count)
        {
            ApiModelResidentialArea[] result = null;
            if (string.IsNullOrEmpty(cue) || string.IsNullOrEmpty(cityName))
                return result;
            if (Citys == null || !Citys.AllKeys.Contains(cityName))
            {
                LogHelper.Error("GetResidentialArea:请先配置xunjiaCitys节点:" + cityName, new Exception("配置错误"));
                return result;
            }
            var APIUrl = this.APIUrl + Citys[cityName] + "/api";
            IDictionary<string, string> paramsList = new Dictionary<string, string>();
            paramsList.Add("name", cue.Trim());
            var query = WebApiHelper.GetDataForJson<WebOutputResult<List<ApiModelResidentialArea>>>(
                WebApiHelper.WebApiAddress(
                APIUrl,
                "selectFuzzySearchResidentialarea", paramsList),
                GetPeacockBaseToken(cityName, APIUrl, UserName, PassWord)).ResultData;
            if (query != null)
                result = query.Count > count ? query.Take(count).ToArray() : query.ToArray();
            return result;
        }

        /// <summary>
        /// 获取某个小区下的所有楼幢信息
        /// </summary>
        /// <param name="cityName">城市名称</param>
        /// <param name="residentialName">小区名称</param>
        /// <param name="districtName">小区所在的行政区域信息</param>
        /// <returns></returns>
        public HouseBuildingDTO[] GetHouseBuildings(string cityName, string residentialName, string districtName)
        {
            HouseBuildingDTO[] result = null;
            if (Citys == null || !Citys.AllKeys.Contains(cityName))
            {
                LogHelper.Error("GetHouseBuildings:请先配置xunjiaCitys节点:" + cityName, new Exception("配置错误"));
                return result;
            }
            var APIUrl = this.APIUrl + Citys[cityName] + "/api";
            string Token = this.GetPeacockBaseToken(cityName, APIUrl, UserName, PassWord);
            var address = WebApiHelper.WebApiAddress(APIUrl, string.Format("ResidentialArea/{0}/building", residentialName.Trim()));
            result = (WebApiHelper.GetDataForJson<WebOutputResult<List<HouseBuildingDTO>>>(address, Token).ResultData).ToArray();
            return result;
        }

        /// <summary>
        /// 获取某个楼幢下所有单元的信息
        /// </summary>
        /// <param name="cityName">城市信息</param>
        /// <param name="buildingId">楼幢ID号</param>
        /// <returns></returns>
        public HousePurposeUnitDTO[] GetHouseUnits(string cityName, long buildingId)
        {
            HousePurposeUnitDTO[] result = null;
            if (Citys == null || !Citys.AllKeys.Contains(cityName))
            {
                LogHelper.Error("GetHouseUnits:请先配置xunjiaCitys节点:" + cityName, new Exception("配置错误"));
                return result;
            }
            var APIUrl = this.APIUrl + Citys[cityName] + "/api";
            string Token = GetPeacockBaseToken(cityName, APIUrl, UserName, PassWord);
            result = (WebApiHelper.GetDataForJson<WebOutputResult<List<HousePurposeUnitDTO>>>(
                WebApiHelper.WebApiAddress(APIUrl, string.Format("building/{0}/units", buildingId.ToString())), Token).ResultData).ToArray();
            return result;
        }

        /// <summary>
        ///  获取某个单元下的所有户信息
        /// </summary>
        /// <param name="cityName">城市名称</param>
        /// <param name="unitId">所在单元的ID</param>
        /// <param name="floor">所在层</param>
        /// <returns></returns>
        public HouseDTO[] GetHouseNames(string cityName, long unitId, int floor)
        {
            HouseDTO[] result = null;
            if (Citys == null || !Citys.AllKeys.Contains(cityName))
            {
                LogHelper.Error("GetHouseNames:请先配置xunjiaCitys节点:" + cityName, new Exception("配置错误"));
                return result;
            }
            var APIUrl = this.APIUrl + Citys[cityName] + "/api";
            string Token = GetPeacockBaseToken(cityName, APIUrl, UserName, PassWord);
            result = (WebApiHelper.GetDataForJson<WebOutputResult<List<HouseDTO>>>(
                WebApiHelper.WebApiAddress(APIUrl, string.Format("houseUnit/{0}/houseList", unitId.ToString())), Token).ResultData).ToArray();
            return result;
        }

        /// <summary>
        /// 根据枚举值类型获取枚举值集合
        /// </summary>
        /// <param name="cityName">城市名称</param>
        /// <param name="enumType">枚举类型</param>
        /// <returns></returns>
        public EnumsListDTO[] GetEnumInfoByType(string cityName, string enumType)
        {
            EnumsListDTO[] result = null;
            EnumsListDTO[] fullEnums = GetEnumsList(cityName);
            string[] types = enumType.Split(',');
            var selectEnums = from p in fullEnums where types.Contains(p.Desc) select p;
            result = selectEnums.ToArray<EnumsListDTO>();
            return result;
        }

        /// <summary>
        /// 获取基础库中所有的枚举值的信息，缓存到Application中
        /// </summary>
        /// <param name="cityName">城市名称</param>
        /// <returns></returns>
        private EnumsListDTO[] GetEnumsList(string cityName)
        {
            EnumsListDTO[] result = null;
            if (HttpContext.Current.Application["EnumsList"] == null)
            {
                if (Citys == null || !Citys.AllKeys.Contains(cityName))
                {
                    LogHelper.Error("GetEnumsList:请先配置xunjiaCitys节点:" + cityName, new Exception("配置错误"));
                    return result;
                }
                var APIUrl = this.APIUrl + Citys[cityName] + "/api";
                string Token = GetPeacockBaseToken(cityName, APIUrl, UserName, PassWord);
                result = (WebApiHelper.GetDataForJson<WebOutputResult<List<EnumsListDTO>>>(
                    WebApiHelper.WebApiAddress(APIUrl, string.Format("enums/list/")), Token).ResultData).ToArray();
                HttpContext.Current.Application["EnumsList"] = result;
            }
            else
            {
                result = HttpContext.Current.Application["EnumsList"] as EnumsListDTO[];
            }
            return result;
        }
        #endregion


        /// <summary>
        /// 获取基础库登陆的令牌
        /// </summary>
        /// <param name="cityName">城市名称</param>
        /// <param name="API">API地址</param>
        /// <param name="userName">用户名</param>
        /// <param name="userPassword">用户密码</param>
        /// <returns></returns>
        private string GetPeacockBaseToken(string cityName, string API, string userName, string userPassword)
        {
            string result = string.Empty;
            string SessionKey = cityName + "BaseToken";
            ApiModelDataBaseToken TokenDTO = null;
            if (HttpContext.Current.Session[SessionKey] == null)
            {
                TokenDTO = new ApiModelDataBaseToken();
                TokenDTO.BaseTokenExpirationTime = DateTime.Now;
            }
            else
            {
                TokenDTO = HttpContext.Current.Session[SessionKey] as ApiModelDataBaseToken;
            }

            TimeSpan timeSpan = TokenDTO.BaseTokenExpirationTime.Subtract(DateTime.Now);
            //判断令牌是否失效或者没有登陆，要是已经失效或者没有登陆，则重新登陆
            string loginUrl = API + "/member/login";
            if (timeSpan.Milliseconds < 0)
            {
                WebOutputResult<TokenDTO> loginReulst = WebApiHelper.Login<WebOutputResult<TokenDTO>>(loginUrl, userName, userPassword);
                TokenDTO.BaseToken = loginReulst.ResultData.Token;
                TokenDTO.BaseTokenExpirationTime = loginReulst.ResultData.ExpirationTime;
                HttpContext.Current.Session[SessionKey] = TokenDTO;
            }
            if (string.IsNullOrWhiteSpace(TokenDTO.BaseToken))
            {
                WebOutputResult<TokenDTO> loginReulst = WebApiHelper.Login<WebOutputResult<TokenDTO>>(loginUrl, userName, userPassword);
                result = loginReulst.ResultData.Token;
                TokenDTO.BaseToken = loginReulst.ResultData.Token;
                TokenDTO.BaseTokenExpirationTime = loginReulst.ResultData.ExpirationTime;
                HttpContext.Current.Session[SessionKey] = TokenDTO;
            }
            else
            {
                result = TokenDTO.BaseToken;
            }
            return result;
        }
    }
}
