using Peacock.PEP.Data.Entities;
using Peacock.PEP.Model;
using Peacock.PEP.Model.Condition;
using System.Collections.Generic;

namespace Peacock.PEP.DataAdapter.Interface
{
    public interface ICompanyAdapter
    {
        ResultInfo SaveCompany(CompanyModel dto, CompanyResourseModel resourseDto);

        /// <summary>
        CompanyModel GetById(long id);
        /// 获取公司信息
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每天显示条数</param>
        /// <param name="total">总条数</param>
        /// <returns></returns>
        List<CompanyModel> GetCompanyList(CompanyCondition condition, int pageIndex, int pageSize, out int total);

        /// <summary>
        /// 获取某个公司的信息
        /// </summary>
        /// <param name="id">公司编号</param>
        /// <returns></returns>
        ResultInfo GetCompany(long id);

        /// <summary>
        /// 审核公司信息
        /// </summary>
        /// <param name="id">公司编号</param>
        /// <returns></returns>
        ResultInfo ApproveCompany(long id);

        /// <summary>
        /// 保存拒绝理由
        /// </summary>
        /// <param name="id">公司编号</param>
        /// <param name="reson">拒绝理由</param>
        /// <returns></returns>
        ResultInfo SaveReson(long id, string reson);

        /// <summary>
        /// 修改公司
        /// </summary>
        /// <param name="item">修改对象</param>
        /// <returns></returns>
        ResultInfo UpdateCompany(CompanyModel dto);

        /// <summary>
        /// 删除公司信息
        /// </summary>
        /// <param name="id">公司编号</param>
        /// <returns></returns>
        ResultInfo DeleteCompany(long id);
    }
}
