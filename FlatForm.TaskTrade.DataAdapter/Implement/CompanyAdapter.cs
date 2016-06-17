using Peacock.PEP.Data.Entities;
using Peacock.PEP.DataAdapter.Interface;
using Peacock.PEP.Model;
using Peacock.PEP.Model.Condition;
using Peacock.PEP.Service;
using System.Collections.Generic;

namespace Peacock.PEP.DataAdapter.Implement
{
    public class CompanyAdapter : ICompanyAdapter
    {
        public ResultInfo SaveCompany(CompanyModel dto, CompanyResourseModel resourseDto)
        {
            return CompanyService.Instance.SaveCompany(dto, resourseDto);
        }

        /// <summary>
        /// 获取公司信息
        /// </summary>
        /// <param name="condition">查询条件</param>
        public CompanyModel GetById(long id)
        {
            return CompanyService.Instance.GetById(id).ToModel<CompanyModel>();
        }
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每天显示条数</param>
        /// <param name="total">总条数</param>
        /// <returns></returns>
        public List<CompanyModel> GetCompanyList(CompanyCondition condition, int pageIndex, int pageSize, out int total)
        {
            return CompanyService.Instance.GetCompanyList(condition, pageIndex, pageSize, out total).ToListModel<CompanyModel, Company>();
        }

        /// <summary>
        /// 获取某个公司的信息
        /// </summary>
        /// <param name="id">公司编号</param>
        /// <returns></returns>
        public ResultInfo GetCompany(long id)
        {
            ResultInfo result = CompanyService.Instance.GetCompany(id);
            Company item = result.Data;
            CompanyModel dto = item.ToModel<CompanyModel>();
            result.Data = dto;
            return result;
        }

        /// <summary>
        /// 审核公司信息
        /// </summary>
        /// <param name="id">公司编号</param>
        /// <returns></returns>
        public ResultInfo ApproveCompany(long id)
        {
            return CompanyService.Instance.ApproveCompany(id);
        }

        /// <summary>
        /// 保存拒绝理由
        /// </summary>
        /// <param name="id">公司编号</param>
        /// <param name="reson">拒绝理由</param>
        /// <returns></returns>
        public ResultInfo SaveReson(long id, string reson)
        {
            return CompanyService.Instance.SaveReson(id, reson);
        }

        /// <summary>
        /// 修改公司
        /// </summary>
        /// <param name="item">修改对象</param>
        /// <returns></returns>
        public ResultInfo UpdateCompany(CompanyModel dto)
        {
            return CompanyService.Instance.UpdateCompany(dto);
        }

        /// <summary>
        /// 删除公司信息
        /// </summary>
        /// <param name="id">公司编号</param>
        /// <returns></returns>
        public ResultInfo DeleteCompany(long id)
        {
            return CompanyService.Instance.DeleteCompany(id);
        }
        
    }
}