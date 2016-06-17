using Peacock.PEP.Model;
using Peacock.PEP.Model.DTO;
using Peacock.PEP.Services.API;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacock.PEP.Service.API
{
    /// <summary>
    /// 外采api调用
    /// </summary>
    public class EiasAPIClient : BaseClientService
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public EiasAPIClient()
            : base(ConfigurationManager.AppSettings["EIASURL"])
        {

        }

        /// <summary>
        /// 获取外业勘察人员
        /// </summary>
        /// <param name="DepartId">部门id</param>
        /// <returns></returns>
        public List<OutTaskUserModel> GetOuterTaskUsers(long departId)
        {
            return this.Get<string, List<OutTaskUserModel>>(null, string.Format("/apis/InworkGetUsersList?Id={0}", departId));
        }

        /// <summary>
        /// 外业任务分配
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public ResultInfo AssignedOutTask(string json)
        {
            var client = new RestClient(this.ApiBaseUrl);
            var request = new RestRequest("/apis/InworkPushTask", Method.POST);
            request.AddParameter("jsonConcent", json);
            var data = client.Execute<ResultInfo>(request);
            return data.Data;
        }

        /// <summary>
        /// 根据权限系统用户id获取勘察表
        /// </summary>
        /// <param name="UserId">权限系统用户id</param>
        /// <returns></returns>
        public DataDefinesResult GetGetDataDefines(long UserId)
        {
            return this.Get<string, DataDefinesResult>(null, string.Format("/apis/InworkGetDatadefineNames?userId={0}", UserId));
        }
    }
}
