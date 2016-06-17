using EIAS.Models;
using Newtonsoft.Json;
using Peacock.Common.Exceptions;
using Peacock.Common.Helper;
using Peacock.PEP.Data.Entities;
using Peacock.PEP.Model;
using Peacock.PEP.Model.DTO;
using Peacock.PEP.Service.API;
using Peacock.PEP.Service.Base;
using Peacock.PEP.Services.API;
using PermissionsMiddle.Dto;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacock.PEP.Service
{
    /// <summary>
    /// 外业任务
    /// </summary>
    public class OutTaskService : SingModel<OutTaskService>
    {
        private OutTaskService()
        {

        }

        /// <summary>
        /// 根据公司获取外业用户
        /// </summary>
        /// <returns></returns>
        public Dictionary<long, string> GetOuterTaskUsers()
        {
            LogHelper.Ilog("GetOuterTaskUsers", "根据公司获取外业用户-" + Instance.ToString());
            UserApiDto user = UserService.Instance.GetUser();
            return GetOuterTaskUsers(user.CompanyId);
        }

        /// <summary>
        /// 根据公司获取外业用户
        /// </summary>
        /// <param name="departmentid"></param>
        /// <returns></returns>
        public Dictionary<long, string> GetOuterTaskUsers(long departmentid)
        {
            Dictionary<long, string> result = new Dictionary<long, string>();
            try
            {
                List<OutTaskUserModel> value = new EiasAPIClient().GetOuterTaskUsers(departmentid);
                LogHelper.Ilog("GetOuterTaskUsers?departmentid=" + departmentid, "根据公司获取外业用户-" + Instance.ToString());
                //特殊处理个人用户
                string company = ConfigurationManager.AppSettings["PersonalUserCompany"];
                UserApiDto user = UserService.Instance.GetUser();
                if (company == user.CompanyName) {
                    result.Add(user.Id, user.UserAccount);
                }
                else
                {
                    foreach (var item in value)
                    {
                        if (!result.ContainsKey(item.CRMID))
                        {
                            result.Add(item.CRMID, item.UserAccount);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("调用外采系统获取外采用户列表出错", ex);
            }

            return result;
        }

        /// <summary>
        /// 外业任务分配
        /// </summary>
        /// <param name="entity">项目信息</param>
        /// <param name="cust">客户信息</param>
        /// <param name="listExplorationContacts">看房联系人信息</param>
        /// <returns></returns>
        public void AssignedOutTask(Project entity, Customer cust, List<ExplorationContacts> listExplorationContacts)
        {
            List<OriginalDataExDTO> datas = new List<OriginalDataExDTO>();

            OriginalDataExDTO outTask = new OriginalDataExDTO();
            outTask.IsPad = true;
            outTask.TaskNum = entity.ProjectNo;
            outTask.OperationType = "派发";
            outTask.CompanyName = entity.DepartmentId.ToString();
            outTask.InquirerName = entity.Investigator;
            outTask.AdjustFee = 0;
            outTask.QuickPrice = 0;
            outTask.EmergencyLevel = entity.EmergencyLevel == "普通" ? "常规" : "紧急";
            outTask.TargetName = entity.PropertyType;
            outTask.Remark = entity.Remark;
            outTask.DataDefineId = entity.OutSurveyTableId;
            outTask.CityName = entity.City;
            outTask.TargetAddress = entity.ProjectAddress ?? string.Empty;
            outTask.ResidentialArea = entity.ResidentialAddress ?? string.Empty;

            outTask.ClientName = cust.CustomerName;
            outTask.ClientTelephone = cust.Tel;
            outTask.CustomerSource = entity.ProjectSource;
            outTask.LiveSearchCharge = 0;
            ExplorationContacts DefaultExplorationContacts = listExplorationContacts.Where(x => x.IsDefault == true).FirstOrDefault();
            if (DefaultExplorationContacts == null && listExplorationContacts.Count > 0)
            {
                DefaultExplorationContacts = listExplorationContacts[0];
            }
            if(DefaultExplorationContacts != null)
            {
                outTask.ContactPerson = DefaultExplorationContacts.Contacts;//看房联系人
                outTask.ContactTel = DefaultExplorationContacts.Phone;//看房联系人电话
            }            
            datas.Add(outTask);

            try
            {
                string json = JsonConvert.SerializeObject(datas);
                LogHelper.Ilog("AssignedOutTask?outTask=" + json, "外业任务分配-" + Instance.ToString());
                ResultInfo data = new EiasAPIClient().AssignedOutTask(json);
                if (!data.Success)
                {
                    LogHelper.Error("外业任务分配", new Exception(data.Message));
                    throw new ServiceException("外业任务分配失败!");
                }
            }
            catch (Exception ex)
            {
                throw new ServiceException("外业任务分配失败!" + ex.ToString());
            }
        }

        /// <summary>
        /// 获取当前登陆用户的勘察表
        /// </summary>
        /// <returns></returns>
        public Dictionary<long, string> GetDataDefines()
        {
            UserApiDto user = UserService.Instance.GetUser();
            Dictionary<long, string> result = new Dictionary<long, string>();
            DataDefinesResult value = new EiasAPIClient().GetGetDataDefines(user.Id);
            if (value.Success && value.Data != null && value.Data.Count() > 0)
            {
                foreach (var item in value.Data)
                {
                    if (!result.ContainsKey(item.ID))
                    {
                        result.Add(item.ID, item.Name);
                    }
                }
            }
            return result;
        }
    }
}