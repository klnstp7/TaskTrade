using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Peacock.PEP.Service.Base;
using Peacock.PEP.Data.Entities;
using Peacock.PEP.Repository.Repositories;
using Peacock.Common.Helper;

namespace Peacock.PEP.Service
{
    public class ProjectStateInfoService : SingModel<ProjectStateInfoService>
    {
        private ProjectStateInfoService()
        {
        }

        /// <summary>
        /// 根据项目Id获取流程状态记录列表
        /// 作者：BOBO
        /// 时间：2016-4-7
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public IList<ProjectStateInfo> GetProjectStateInfoList(long projectId)
        {
            LogHelper.Ilog("GetProjectStateInfoList?projectId=" + projectId, "获取流程状态记录列表-" + Instance.ToString());
            if (projectId==0)
                return null;
            var query = ProjectStateInfoRepository.Instance.Source;
            query = query.Where(p => p.ProjectId == projectId);
            query = query.OrderByDescending(p => p.OperationTime);
            return query.ToList();            
        }

        /// <summary>
        /// 写入流程记录
        /// 作者：BOBO
        /// 时间：2016-4-13
        /// </summary>
        /// <param name="projectId">项目id</param>
        /// <param name="sign">标记 1:立项，2:提交审核，3:项目审核
        /// 4:报告上传 5:报告修改 6:报告发送
        /// </param>
        public void WriteInProjectStateInfo(long projectId,StateInfoEnum stateInfoEnum)
        {
            string content = string.Empty;
            switch((int)stateInfoEnum)
            {
                case 1:
                    content="立项";
                    break;
                case 2:
                    content = "提交审核";
                    break;
                case 3:
                    content = "项目审核";
                    break;
                case 4:
                    content = "报告上传";
                    break;
                case 5:
                    content = "报告修改";
                    break;
                case 6:
                    content = "报告发送";
                    break;
                default:
                    content = "其他";
                    break;
            }
            ProjectStateInfo entity = new ProjectStateInfo();
            entity.Content = content;
            entity.OperationTime = DateTime.Now;
            entity.ProjectId = projectId;
            //entity.Operator = UserService.Instance.GetCurrentUser().EmployeeName;
            entity.Operator = CookieHelper.GetCookie(CookieHelper.UserStateKey); 
            ProjectStateInfoRepository.Instance.Insert(entity);
        }
    }

    public enum StateInfoEnum
    {
        立项 = 1,
        提交审核 = 2,
        项目审核 = 3,
        报告上传 = 4,
        报告修改 = 5,
        报告发送 = 6
    }
}
