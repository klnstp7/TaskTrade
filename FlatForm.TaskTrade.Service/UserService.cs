using System.Linq;
using Newtonsoft.Json;
using Peacock.PEP.Data.Entities;
using Peacock.PEP.Model.ApiEnum;
using Peacock.PEP.Repository.Repositories;
using Peacock.PEP.Service.Base;
using Peacock.PMS.Service.Enum;
using Peacock.PMS.Service.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using PermissionsMiddle.Dto;
using Peacock.Common.Helper;
using Peacock.PEP.Model.ApiModel;
using Peacock.PEP.Model;
using System.IO;
using log4net;

namespace Peacock.PEP.Service
{
    public class UserService : SingModel<UserService>
    {
        private static readonly string AppCode = ConfigurationManager.AppSettings["YewuAppCode"];
        private static readonly int CacheTime = Convert.ToInt32(ConfigurationManager.AppSettings["CrmCacheTime"]);
        //个人用户组公司
        private static readonly string PersonalUserCompany = ConfigurationManager.AppSettings["PersonalUserCompany"];
        //需要过滤的公司
        private static readonly string[] FilterCompanys = ConfigurationManager.AppSettings["FilterCompanys"].Split(',');
        private static readonly PmsService PmsService = new PmsService(AppCode, CacheTime);

        private UserService()
        {

        }

        #region 用户权限


        public void RefreshCached()
        {

            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                PmsService.RefreshCached();
            }).ContinueWith((t) =>
            {
                if (t.Exception == null)
                    LogManager.GetLogger("RefreshCached").Error("刷新缓存成功", null);
                else
                    LogManager.GetLogger("RefreshCached").Error("刷新缓存成功失败", t.Exception);
            });
        }

        /// <summary>
        /// 根据AppCode获取用户权限
        /// </summary>
        /// <returns></returns>
        public IList<UserPowerApiDto> GetUserPermissions(string userAccount = null)
        {
            return PmsService.GetUserPermissions(GetCrmUser(userAccount).Id);
        }

        /// <summary>
        /// 获取CRM用户信息
        /// </summary>
        /// <returns></returns>
        public UserApiDto GetCrmUser(string userAccount = null)
        {
            if (!string.IsNullOrEmpty(userAccount))
                return PmsService.GetUser(userAccount);
            return PmsService.GetUser(CookieHelper.GetCookie(CookieHelper.UserStateKey));
        }

        #endregion 用户权限


        #region 部门

        /// <summary>
        /// 获取当前用户所在的部门
        /// </summary>
        /// <returns></returns>
        internal CompanyApiDto GetUserDepartment(string userAccount = null)
        {
            var account = string.IsNullOrEmpty(userAccount) ? GetCrmUser(userAccount).UserAccount : userAccount;
            return PmsService.GetUserDepartment(account);
        }

        /// <summary>
        /// 获取当前用户所在的公司
        /// </summary>
        /// <returns></returns>
        internal CompanyApiDto GetUserCompany(string userAccount = null)
        {
            var account = string.IsNullOrEmpty(userAccount) ? GetCrmUser(userAccount).UserAccount : userAccount;
            return PmsService.GetUserCompany(account);
        }

        /// <summary>
        /// 获取当前用户部门下属的子部门列表
        /// </summary>
        /// <returns></returns>
        internal IList<CompanyApiDto> GetUserChildDeparments(string useraccount = null)
        {
            var userDepartment = GetUserDepartment(useraccount);
            var departmentList = GetChildList(userDepartment.Id, GetAllStructures());
            departmentList.Add(userDepartment);

            return departmentList;
        }

        /// <summary>
        /// 获取当前用户公司下属的部门列表，包括当前公司(tag为空则加载下属公司和部门，填“公司”加载下属公司，填“部门”加载下属部门)
        /// </summary>
        /// <returns></returns>
        public IList<CompanyApiDto> GetDepartmentsByCompany(string tag = null, string userAccount = null)
        {
            var company = GetUserCompany(userAccount);
            var departmentList = GetChildList(company.Id, GetAllStructures());


            switch (tag)
            {
                case StructureTag.Company:
                    departmentList = departmentList.Where(x => x.Tag == StructureTag.Company).ToList();
                    break;
                case StructureTag.Department:
                    departmentList = departmentList.Where(x => x.Tag == StructureTag.Department).ToList();
                    break;
            }

            departmentList.Add(company);

            return departmentList;
        }

        /// <summary>
        /// 获取所有组织架构（公司和部门）
        /// </summary>
        /// <returns></returns>
        internal IList<CompanyApiDto> GetAllStructures()
        {
            return PmsService.GetAllStructures();
        }

        /// <summary>
        /// 根据名称获取部门信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal CompanyApiDto GetDepartmentByName(string name)
        {
            var list = GetAllStructures();
            return list.FirstOrDefault(x => x.CompanyName == name) ?? new CompanyApiDto();
        }

        public CompanyApiDto GetDepartmentById(long id)
        {
            var list = GetAllStructures();
            return list.FirstOrDefault(x => x.Id == id) ?? new CompanyApiDto();
        }

        public List<CompanyApiDto> GetDepartmentByIds(List<long> ids)
        {
            ids = ids.Distinct().ToList();
            var list = GetAllStructures();
            var result = list.Where(x => ids.Contains(x.Id)).ToList();// new List<CompanyApiDto>();
            return result;
        }

        /// <summary>
        /// 获取所有公司  
        /// </summary>
        /// <returns></returns>
        internal IList<ApiModelCrmDepartment> GetAllCompanys()
        {
            return GetAllStructures()
                .Where(x => x.Tag == StructureTag.Company)
                .Select(x => new ApiModelCrmDepartment
                {
                    CrmDepartmentId = x.Id,
                    CrmDepartmentName = x.CompanyName
                }).ToList();
        }

        /// <summary>
        /// 根据公司ID获取下属所有部门列表
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        internal IList<CompanyApiDto> GetDepartmentsByCompanyId(long companyId)
        {
            return GetChildList(companyId, GetAllStructures());
        }

        /// <summary>
        /// 获取部门所属的公司
        /// </summary>
        /// <returns></returns>
        internal CompanyApiDto GetBaseCompany(long departmentId)
        {
            var result = GetAllStructures().FirstOrDefault(x => x.Id == departmentId);
            if (result.Tag != StructureTag.Company)
            {
                result = GetBaseCompany(result.BaseCompanyId.Value);
            }

            return result;
        }

        /// <summary>
        /// 根据登录名获取用户所在的公司
        /// </summary>
        internal CompanyApiDto GetCompanyByUserAccount(string userAccount)
        {
            return PmsService.GetUserDepartment(userAccount);
        }

        /// <summary>
        /// 根据登录名获取用户所在的组织架构（属于部门则获取部门，属于公司则获取公司）
        /// </summary>
        internal CompanyApiDto GetDepartmentByUserAccount(string userAccount)
        {
            return PmsService.GetUserCompany(userAccount);
        }

        /// <summary>
        /// 获取当前公司或部门所有下属列表
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        private IList<CompanyApiDto> GetChildList(long parentId, IEnumerable<CompanyApiDto> list)
        {
            var childList = list.Where(x => x.BaseCompanyId == parentId).ToList();
            if (childList.Count > 0)
            {
                foreach (var d in childList)
                {
                    childList = childList.Union(GetChildList(d.Id, GetAllStructures())).ToList();
                }
            }

            return childList;
        }

        /// <summary>
        /// 根据用户数据查看权限获取部门ID
        /// </summary>
        /// <returns></returns>
        public List<long> GetCrmDepartmentIds(string useraccount = null)
        {
            List<long> ids = new List<long>();
            var permissions = Instance.GetUserPermissions(useraccount);

            if (permissions.Any(x => x.PowerCode == DataPermission.Company.ToString()))
            {
                var departmentList = Instance.GetDepartmentsByCompany(userAccount: useraccount);
                ids.AddRange(departmentList.Select(x => x.Id));

            }
            else if (permissions.Any(x => x.PowerCode == DataPermission.AllDepartment.ToString()))
            {
                var departmentList = Instance.GetUserChildDeparments(useraccount);
                ids.AddRange(departmentList.Select(x => x.Id));

            }
            else if (permissions.Any(x => x.PowerCode == DataPermission.Department.ToString()))
            {
                var department = Instance.GetUserDepartment(useraccount);
                ids.Add(department.Id); //查询本部门的sql
            }

            return ids;
        }

        /// <summary>
        /// 项目列表查询判断（根据用户权限),返回：CrmDepartmentID、 ProjectCreator_TID
        /// </summary>
        /// <param name="CrmDepartmentID">部门ID集合</param>
        /// <param name="ProjectCreator_TID">项目创建者ID</param>
        /// <returns></returns>
        public void GetLoadProject_power(out List<long> CrmDepartmentIDs, out long ProjectCreator_TID)
        {
            ProjectCreator_TID = 0;
            CrmDepartmentIDs = new List<long>();
            var permissions = Instance.GetUserPermissions();

            if (permissions.Any(x => x.PowerCode == DataPermission.Company.ToString()))
            {
                var departmentList = Instance.GetDepartmentsByCompany();
                CrmDepartmentIDs.AddRange(departmentList.Select(x => x.Id));
            }
            else if (permissions.Any(x => x.PowerCode == DataPermission.AllDepartment.ToString()))
            {
                var departmentList = Instance.GetUserChildDeparments();
                CrmDepartmentIDs.AddRange(departmentList.Select(x => x.Id));
            }
            else if (permissions.Any(x => x.PowerCode == DataPermission.Department.ToString()))
            {
                var department = Instance.GetUserDepartment();
                CrmDepartmentIDs.Add(department.Id);
            }
            else
            {
                ProjectCreator_TID = Instance.GetCrmUser().Id;
            }
        }
        #endregion 部门

        #region 新增用户

        /// <summary>
        /// 保存用户信息到权限系统
        /// </summary>
        /// <param name="dto">收集的用户信息</param>
        /// <returns></returns>
        public ResultInfo SaveUserInfo(UserApiDto dto)
        {
            LogHelper.Ilog("SaveUserInfo?dto=" + dto.ToJson(), "新增用户-" + Instance.ToString());
            ResultInfo result = new ResultInfo();
            try
            {
                var exist = PmsService.GetUser(dto.UserAccount);
                if (exist == null || exist.Id <= 0)
                {
                    if (!string.IsNullOrEmpty(dto.CompanyName))     //存在公司名的先暂存
                    {
                        var userCompany = new UserCompany()
                        {
                            UserName = dto.UserAccount,
                            CompanyName = dto.CompanyName
                        };
                        UserCompanyRepository.Instance.Insert(userCompany);
                    }
                    dto.CompanyName = ConfigurationManager.AppSettings["PersonalUserCompany"];  //将所有用户都作为个人用户
                    CompanyApiDto company = PmsService.GetAllStructures().FirstOrDefault(t => t.CompanyName == dto.CompanyName);
                    dto.CompanyId = company.Id;
                    var data = PmsService.CreatePepSimpleUser(dto);
                    if (data)
                    {
                        //发送消息给外采,通知创建用户
                        KafkaMQService.Instance.SendMessageWaicaiQueue(new WaicaiMessage { bussinessType = "添加用户", userAccount = dto.UserAccount });
                        result.Message = "注册成功";
                        result.Success = true;
                    }
                }
                else
                {
                    result.Message = string.Format("用户[{0}]已经存在", dto.UserAccount);
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        #endregion

        #region 获取权限系统有的公司列表

        /// <summary>
        /// 保存用户信息到权限系统
        /// </summary>        
        /// <returns></returns>
        public ResultInfo GetCRMCompanys()
        {
            ResultInfo result = new ResultInfo();
            IList<CompanyApiDto> datas = PmsService.GetAllStructures().Where(x => x.CompanyName != PersonalUserCompany && x.Tag == "公司" && !FilterCompanys.Contains(x.CompanyName)).ToList();
            string[] formatCompany = new string[datas.Count];
            for (int i = 0; i < datas.Count; i++)
            {
                formatCompany[i] = datas[i].CompanyName;
            }
            result.Message = "操作成功";
            result.Success = true;
            result.Data = formatCompany;
            return result;
        }

        #endregion

        /// <summary>
        /// 获取CRM用户信息
        /// </summary>
        /// <returns></returns>
        //public UserApiDto GetCrmUser()
        //{
        //    //return GetCrmUser(GetCurrentUser().EmployeeName);
        //    return GetCrmUser("评E评管理员");
        //}

        /// <summary>
        /// 获取CRM用户信息
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="userAccount">用户登录名</param>
        /// <returns></returns>
        public UserApiDto GetUser(long userId = 0, string userAccount = null)
        {
            if (userId > 0)
                return PmsService.GetUser(userId);
            if (!string.IsNullOrEmpty(userAccount))
                return PmsService.GetUser(userAccount);
            return PmsService.GetUser(CookieHelper.GetCookie(CookieHelper.UserStateKey));
        }

        public List<UserApiDto> GetUserByIds(List<long> ids)
        {
            ids = ids.Distinct().ToList();
            var users = PmsService.GetAllUsers();
            var result = users.Where(x => ids.Contains(x.Id)).ToList();
            return result;
        }


        /// <summary>
        /// 获取城市可选评估机构列表
        /// 作者：BOBO
        /// 时间：2016-4-27
        /// </summary>
        /// <param name="cityName"></param>
        /// <param name="bank"></param>
        /// <returns></returns>
        public IDictionary<string, object> GetAllCompany(string cityName, string bank = "")
        {
            var PersonalUserCompany = System.Web.Configuration.WebConfigurationManager.AppSettings["PersonalUserCompany"];
            var result = GetAllStructures().Where(x => x.Tag == StructureTag.Company && x.CompanyName != PersonalUserCompany);
            if (!string.IsNullOrEmpty(cityName))
                result = result.Where(x => x.CityName == cityName);
            if (!string.IsNullOrEmpty(bank))
            {
                var bankInfo = result.Where(x => x.CompanyName == bank).FirstOrDefault();
                if (bankInfo != null)
                    result = GetSonID(bankInfo.Id);
            }
            var list = result.Select(x => new
            {
                OrganizationId = x.Id,
                OrganizationName = x.CompanyName
            }).ToList();
            IDictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("code", "1");
            dic.Add("msg", "请求成功!");
            dic.Add("data", list);
            return dic;
        }

        /// <summary>
        /// 递归查询所有子公司
        /// </summary>
        /// <param name="p_id"></param>
        /// <returns></returns>
        public IEnumerable<CompanyApiDto> GetSonID(long? p_id)
        {
            var query = GetAllStructures().Where(x => x.Tag == StructureTag.Company).Where(x => x.BaseCompanyId == p_id);
            return query.ToList().Concat(query.ToList().SelectMany(t => GetSonID(t.Id)));
        }
    }
}
