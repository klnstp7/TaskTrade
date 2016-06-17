using Peacock.PEP.Model;
using PermissionsMiddle.Dto;
using System.Collections.Generic;
using Peacock.PEP.Model.ApiModel;

namespace Peacock.PEP.DataAdapter.Interface
{
    public interface IUserAdapter
    {
        IList<UserPowerApiDto> GetUserPermissions(string userAccount = null);

        ResultInfo SaveUserInfo(UserApiDto dto);

        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <returns></returns>
        UserApiDto GetCurUserApiDto();

        /// <summary>
        /// 保存用户信息到权限系统
        /// </summary>        
        /// <returns></returns>
        ResultInfo GetCRMCompanys();

        /// <summary>
        /// 获取当前用户公司下属的部门列表，包括当前公司(tag为空则加载下属公司和部门，填“公司”加载下属公司，填“部门”加载下属部门)
        /// </summary>
        /// <returns></returns>
        IList<CompanyApiDto> GetDepartmentsByCompany(string tag = null, string userAccount = null);

        UserApiDto GetUser(long userId = 0, string userAccount = null);

        List<UserApiDto> GetUserByIds(List<long> ids);

        List<CompanyApiDto> GetDepartmentByIds(List<long> ids);
        /// <summary>
        /// 自动刷新缓存

        CompanyApiDto GetDepartmentById(long id);
        /// </summary>
        void RefreshCached();

        void StartMqReceiveData();

        IDictionary<string, object> GetAllCompany(string cityName, string bank = "");
    }
}
