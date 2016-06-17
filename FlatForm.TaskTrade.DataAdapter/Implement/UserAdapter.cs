using Peacock.PEP.DataAdapter.Interface;
using Peacock.PEP.Model;
using Peacock.PEP.Service;
using PermissionsMiddle.Dto;
using System.Collections.Generic;
//using Peacock.PEP.Model.ApiModel;

namespace Peacock.PEP.DataAdapter.Implement
{
    public class UserAdapter : IUserAdapter
    {

        public IList<UserPowerApiDto> GetUserPermissions(string userAccount = null)
        {
            return UserService.Instance.GetUserPermissions(userAccount);
        }


        public ResultInfo SaveUserInfo(UserApiDto dto)
        {
            return UserService.Instance.SaveUserInfo(dto);
        }

        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <returns></returns>
        public UserApiDto GetCurUserApiDto()
        {
            return UserService.Instance.GetCrmUser();
        }

        /// <summary>
        /// 保存用户信息到权限系统
        /// </summary>        
        /// <returns></returns>
        public ResultInfo GetCRMCompanys()
        {
            return UserService.Instance.GetCRMCompanys();
        }

        /// <summary>
        /// 获取当前用户公司下属的部门列表，包括当前公司(tag为空则加载下属公司和部门，填“公司”加载下属公司，填“部门”加载下属部门)
        /// </summary>
        /// <returns></returns>
        public IList<CompanyApiDto> GetDepartmentsByCompany(string tag = null, string userAccount = null)
        {
            return UserService.Instance.GetDepartmentsByCompany(tag, userAccount);
        }

        public UserApiDto GetUser(long userId = 0, string userAccount = null)
        {
            return UserService.Instance.GetUser(userId, userAccount);
        }

        public List<UserApiDto> GetUserByIds(List<long> ids)
        {
            return UserService.Instance.GetUserByIds(ids);
        }

        public List<CompanyApiDto> GetDepartmentByIds(List<long> ids)
        {
            return UserService.Instance.GetDepartmentByIds(ids);
        }

        public void RefreshCached()
        {
            UserService.Instance.RefreshCached();
        }

        public CompanyApiDto GetDepartmentById(long id)
        {
            return UserService.Instance.GetDepartmentById(id);
        }

        public IDictionary<string, object> GetAllCompany(string cityName, string bank = "")
        {
            return UserService.Instance.GetAllCompany(cityName, bank);
        }
        public void StartMqReceiveData()
        {
            KafkaMQService.Instance.StartMqReceiveData();
        }
    }
}