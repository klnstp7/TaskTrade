using Peacock.PEP.DataAdapter.Interface;
using Peacock.PEP.Model.ApiModel;
using Peacock.PEP.Service;
using GHSoft.DTO2;
using Peacock.PMS.Service.Services;

namespace Peacock.PEP.DataAdapter.Implement
{
    public class BaseAPIAdapter:IBaseAPIAdapter
    {

        /// <summary>
        /// 获取房屋评估案例 
        /// </summary>
        /// <param name="residentialAreaName">小区名称</param>
        /// <param name="address">地址</param>
        /// <param name="cityName">城市名称</param>
        /// <returns></returns>
        public HouseAppraisalCaseDTO[] GetHouseAppraisalCaseList(string residentialAreaName, string address,
            string residentialAreaId, string cityName, out string msg)
        {
            return BaseAPIService.Instance.GetHouseAppraisalCaseList(residentialAreaName, address, residentialAreaId,
                cityName, out msg);
        }

        /// <summary>
        /// 获取二手房出售案例
        /// </summary>
        /// <param name="residentialAreaName">小区名称</param>
        /// <param name="address">地址</param>
        /// <param name="address">城市名称</param>
        /// <returns></returns>
        public HouseResoldCaseDTO[] GetHouseResoldCaseList(string residentialAreaName, string address,
            string residentialAreaId, string cityName, out string msg)
        {
            return BaseAPIService.Instance.GetHouseResoldCaseList(residentialAreaName, address, residentialAreaId,
                cityName, out msg);
        }

        /// <summary>
        /// 获取报盘数据
        /// </summary>
        /// <param name="residentialAreaName">小区名称</param>
        /// <param name="address">地址</param>
        /// <param name="CtiyName">城市名称</param>
        /// <returns></returns>
        public ApiModelBaoPan[] GetOfferForSaleList(string residentialAreaName, string address, string residentialAreaId,
            string cityName, out string msg)
        {
            return BaseAPIService.Instance.GetOfferForSaleList(residentialAreaName, address, residentialAreaId, cityName,
                out msg);
        }

        /// <summary>
        /// 根据给出的提示(拼音、楼盘名简写等)返回楼盘名称集合
        /// </summary>
        /// <param name="cue">部分小区信息</param>
        /// <param name="cityName">城市名称</param>
        /// <param name="count">指定数量</param>
        /// <returns></returns>
        public ApiModelResidentialArea[] GetResidentialArea(string cue, string cityName, int count)
        {
            return BaseAPIService.Instance.GetResidentialArea(cue, cityName, count);
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
            return BaseAPIService.Instance.GetHouseBuildings(cityName, residentialName, districtName);
        }

        /// <summary>
        /// 获取某个楼幢下所有单元的信息
        /// </summary>
        /// <param name="cityName">城市信息</param>
        /// <param name="buildingId">楼幢ID号</param>
        /// <returns></returns>
        public HousePurposeUnitDTO[] GetHouseUnits(string cityName, long buildingId)
        {
            return BaseAPIService.Instance.GetHouseUnits(cityName, buildingId);
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
            return BaseAPIService.Instance.GetHouseNames(cityName, unitId, floor);
        }

        /// <summary>
        /// 根据枚举值类型获取枚举值集合
        /// </summary>
        /// <param name="cityName">城市名称</param>
        /// <param name="enumType">枚举类型</param>
        /// <returns></returns>
        public EnumsListDTO[] GetEnumInfoByType(string cityName, string enumType)
        {
            return BaseAPIService.Instance.GetEnumInfoByType(cityName, enumType);
        }
    }
}
