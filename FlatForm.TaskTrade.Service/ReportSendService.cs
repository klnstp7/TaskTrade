using System.Collections.Generic;
using System.Linq;
using Peacock.PEP.Service.Base;
using Peacock.PEP.Data.Entities;
using Peacock.PEP.Repository.Repositories;
using Peacock.PEP.Model.Condition;
using Peacock.PEP.Model;
using Peacock.Common.Helper;

namespace Peacock.PEP.Service
{
    public class ReportSendService : SingModel<ReportSendService>
    {
        private ReportSendService()
        {
        }

        /// <summary>
        /// 获取报告发送记录
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页显示数量</param>
        /// <param name="total">返回总数</param>
        /// <returns></returns>
        public List<ReportSend> GetReportSendList(ReportSendCondition condition, int pageIndex, int pageSize, out int total)
        {
            LogHelper.Ilog("GetReportSendList?condition=" + condition.ToJson() + "&pageIndex=" + pageIndex + "&pageSize=" + pageSize, "获取报告发送记录-" + Instance.ToString());
            List<ReportSend> result = new List<ReportSend>();
            var query = ReportSendRepository.Instance.Source;
            query = query.Where(p => p.ProjectId == condition.ProjectId);
            //判断快递公司是否为空
            if (!string.IsNullOrEmpty(condition.SendExpress))
            {
                query = query.Where(p => p.SendExpress.Contains(condition.SendExpress));
            }
            //判断快递号是否为空
            if (!string.IsNullOrEmpty(condition.ExpressNo))
            {
                query = query.Where(p => p.ExpressNo.Contains(condition.ExpressNo));
            }
            //判断接收地址是否为空
            if (!string.IsNullOrEmpty(condition.SendAddress))
            {
                query = query.Where(p => p.SendAddress.Contains(condition.SendAddress));
            }
            //判断收获人电话是否为空
            if (!string.IsNullOrEmpty(condition.ReciverMobile))
            {
                query = query.Where(p => p.ReciverMobile.Contains(condition.ReciverMobile));
            }
            query = query.OrderByDescending(p => p.Id);
            if (pageIndex == -1)
            {
                result = query.ToList();
                total = result.Count;
            }
            else
            {
                result = ReportSendRepository.Instance.FindForPaging(pageSize, pageIndex, query, out total).ToList();
            }
            return result;
        }

        /// <summary>
        /// 获取项目报告发送数量
        /// </summary>
        /// <param name="projectId">项目编号</param>
        /// <returns></returns>
        public int GetProjectReportCount(long projectId)
        {
            int result = 0;
            var query = ReportSendRepository.Instance.Find(p => p.ProjectId == projectId).ToList();
            if (query != null && query.Count > 0)
            {
                foreach (var item in query)
                {
                    result += item.SendQuantity ?? 0;
                }
            }            
            return result;
        }

        /// <summary>
        /// 保存报告发送记录
        /// </summary>
        /// <param name="saveItem">保存对象</param>
        /// <returns></returns>
        public ResultInfo SaveReportSendData(ReportSend saveItem)
        {
            ResultInfo result = new ResultInfo();
            ReportSendRepository.Instance.Transaction(() => {                
                ReportSend item = ReportSendRepository.Instance.InsertReturnEntity(saveItem);
                if (item != null && item.Id > 0)
                {
                    //写入流程记录(报告发送)
                    int projectId = (int)saveItem.ProjectId;
                    ProjectStateInfoService.Instance.WriteInProjectStateInfo(projectId, StateInfoEnum.报告发送);
                    Project project = ProjectRepository.Instance.Find(t => t.Id == saveItem.ProjectId).FirstOrDefault();
                    project.IsSent = true;
                    ProjectRepository.Instance.Save(project);

                    result.Data = item;
                    result.Message = "操作成功";
                    result.Success = true;

                    LogHelper.Ilog("SaveReportSendData?saveItem=" + saveItem.ToJson(), "保存报告发送记录-" + Instance.ToString());
                    //委托进度查询(递送纸质报告)
                    OnlineBusinessService.Instance.WriteFeedBackKafkaMQ(projectId, ProgressEnum.递送纸质报告);
                }
            });
            return result;
        }

    }
}