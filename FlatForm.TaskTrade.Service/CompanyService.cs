

using System;
using System.Linq;
using Peacock.PEP.Service.Base;
using Peacock.PEP.Model;
using Peacock.PEP.Data.Entities;
using Peacock.PEP.Repository;
using ResourceLibraryExtension.Helper;
using Peacock.PEP.Model.Condition;
using System.Collections.Generic;
using Peacock.Common.Helper;
using System.Configuration;
using Peacock.PMS.Service.Services;

namespace Peacock.PEP.Service
{
    public class CompanyService : SingModel<CompanyService>
    {
        private static readonly string AppCode = ConfigurationManager.AppSettings["YewuAppCode"];
        private static readonly int CacheTime = Convert.ToInt32(ConfigurationManager.AppSettings["CrmCacheTime"]);
        private static readonly PmsService PmsService = new PmsService(AppCode, CacheTime);

        private CompanyService()
        {

        }

        #region 新增公司

        /// <summary>
        /// 保存公司信息到权限系统
        /// </summary>
        /// <param name="dto">收集的公司信息</param>
        /// <returns></returns>
        public ResultInfo SaveCompany(CompanyModel dto, CompanyResourseModel resourseDto)
        {
            LogHelper.Ilog("SaveCompany?CompanyModel=" + dto.ToJson() + "&CompanyResourseModel=" + resourseDto.ToJson(), "注册公司-" + Instance.ToString());
            ResultInfo result = new ResultInfo();

            var crmUser = PmsService.GetUser(dto.UserName);
            var companyNameExist = CompanyRepository.Instance.Find(t => t.CompanyName == dto.CompanyName).ToList();
            var userNameExist = CompanyRepository.Instance.Find(t => t.UserName == dto.UserName).ToList();
            if (companyNameExist != null && companyNameExist.Count > 0)
            {
                result.Message = "相同公司名称已经存在";
            }
            else if ((userNameExist != null && userNameExist.Count > 0) || (crmUser != null && crmUser.Id > 0))
            {
                result.Message = "相同管理员用户名已经存在";
            }
            else
            {
                Company data = new Company();
                data.IsApprove = null;
                data.Id = dto.Id;
                data.CompanyName = dto.CompanyName;
                data.Address = dto.Address;
                data.UserName = dto.UserName;
                data.Password = dto.Password;
                data.City = dto.City;
                data.Contact = dto.Contact;
                data.Tel = dto.Tel;
                data.Remark = dto.Remark;
                data.CreateTime = DateTime.Now;
                CompanyRepository.Instance.Transaction(() =>
                {
                    Company saveItem = CompanyRepository.Instance.InsertReturnEntity(data);
                    if (saveItem != null && saveItem.Id > 0)
                    {
                        CompanyResourse resourse = new CompanyResourse();
                        resourse.Id = resourseDto.Id;
                        resourse.ResourceId = SingleFileManager.SaveFileResource(resourseDto.FileConcent, resourseDto.Ext, resourseDto.FileName, saveItem.Id.ToString());
                        resourse.FileName = resourseDto.FileName;
                        resourse.CompanyId = saveItem.Id;
                        resourse.CreateTime = DateTime.Now;
                        CompanyResourse resourseSaveItem = CompanyResourseRepository.Instance.InsertReturnEntity(resourse);
                        if (resourseSaveItem != null && resourseSaveItem.Id > 0)
                        {
                            result.Data = saveItem;
                            result.Others = resourseSaveItem;
                            result.Success = true;
                        }
                    }
                });
            }
            return result;
        }

        #endregion

        #region 公司查询

        /// <summary>
        /// 获取公司信息
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每天显示条数</param>
        /// <param name="total">总条数</param>
        /// <returns></returns>
        public List<Company> GetCompanyList(CompanyCondition condition, int pageIndex, int pageSize, out int total)
        {
            LogHelper.Ilog("GetCompanyList?CompanyCondition=" + condition.ToJson() + "&pageIndex=" + pageIndex + "&pageSize=" + pageSize, "查询公司-" + Instance.ToString());            
            List<Company> result = new List<Company>();
            var query = CompanyRepository.Instance.Source;
            if (!string.IsNullOrEmpty(condition.CompanyName))
            {
                query = query.Where(p => p.CompanyName.Contains(condition.CompanyName));
            }
            if (!string.IsNullOrEmpty(condition.City))
            {
                query = query.Where(p => p.City == condition.City);
            }
            if (!string.IsNullOrEmpty(condition.Tel))
            {
                query = query.Where(p => p.Tel.Contains(condition.Tel));
            }
            if (condition.ApproveStatus.HasValue)
            {
                switch (condition.ApproveStatus)
                {
                    case 0:
                        query = query.Where(p =>p.IsApprove.HasValue==false);
                        break;
                    case 1:
                        query = query.Where(p => p.IsApprove.Value==true);
                        break;
                    case -1:
                        query = query.Where(p => p.IsApprove.Value==false);
                        break;
                    default:
                        break;

                }
            }
            query = query.OrderBy(p => p.IsApprove).ThenByDescending(p=>p.CreateTime);

            result = CompanyRepository.Instance.FindForPaging(pageSize, pageIndex, query, out total).ToList();
            return result;
        }

        #endregion

        #region 获取某个公司的信息

        /// <summary>
        /// 获取某个公司的信息
        /// </summary>
        /// <param name="id">公司编号</param>
        /// <returns></returns>
        public ResultInfo GetCompany(long id)
        {
            ResultInfo result = new ResultInfo();
            var findItem = CompanyRepository.Instance.Find(t => t.Id == id).ToList();
            if (findItem != null && findItem.Count > 0)
            {
                var resource = CompanyResourseRepository.Instance.Find(t => t.CompanyId == id).ToList();
                if (resource != null && resource.Count > 0)
                {
                    result.Others = resource.FirstOrDefault().ResourceId;
                }

                result.Data = findItem.FirstOrDefault();
                result.Message = "操作成功";
                result.Success = true;
            }
            return result;
        }

        #endregion

        #region 审核公司
        /// <summary>
        /// 审核公司信息
        /// </summary>
        /// <param name="id">公司编号</param>
        /// <returns></returns>
        public ResultInfo ApproveCompany(long id)
        {
            LogHelper.Ilog("ApproveCompany?id=" + id, "审核公司通过-" + Instance.ToString());
            ResultInfo result = new ResultInfo();
            var item = GetCompany(id);
            if (item.Success)
            {
                Company saveItem = item.Data;
                saveItem.IsApprove = true;
                if (CompanyRepository.Instance.Save(saveItem))
                {
                    result.Data = true;
                    result.Message = "操作成功";
                    result.Success = true;
                }
            }
            return result;
        }
        #endregion

        #region 保存拒绝理由
        /// <summary>
        /// 保存拒绝理由
        /// </summary>
        /// <param name="id">公司编号</param>
        /// <param name="reson">拒绝理由</param>
        /// <returns></returns>
        public ResultInfo SaveReson(long id, string reson)
        {
            LogHelper.Ilog("SaveReson?id=" + id + "&reson=" + reson, "审核公司不通过-" + Instance.ToString());            
            ResultInfo result = new ResultInfo();
            var item = GetCompany(id);
            if (item.Success)
            {
                Company saveItem = item.Data;
                saveItem.IsApprove = false;
                saveItem.Reson = reson;
                if (CompanyRepository.Instance.Save(saveItem))
                {
                    result.Data = true;
                    result.Message = "操作成功";
                    result.Success = true;
                }
            }
            return result;
        }
        #endregion

        #region 修改公司
        /// <summary>
        /// 修改公司
        /// </summary>
        /// <param name="item">修改对象</param>
        /// <returns></returns>
        public ResultInfo UpdateCompany(CompanyModel dto)
        {
            LogHelper.Ilog("UpdateCompany?CompanyModel=" + dto.ToJson(), "修改公司信息-" + Instance.ToString());
            ResultInfo result = new ResultInfo();
            var saveItem = GetById(dto.Id);
            saveItem.Address = dto.Address;
            saveItem.City = dto.City;
            saveItem.CompanyName = dto.CompanyName;
            saveItem.Contact = dto.Contact;
            saveItem.Remark = dto.Remark;
            saveItem.Tel = dto.Tel;
            if (CompanyRepository.Instance.Save(saveItem))
            {
                result.Data = true;
                result.Message = "操作成功";
                result.Success = true;
            }
            return result;
        }
        #endregion

        #region 删除公司

        /// <summary>
        /// 删除公司信息
        /// </summary>
        /// <param name="id">公司编号</param>
        /// <returns></returns>
        public ResultInfo DeleteCompany(long id)
        {
            LogHelper.Ilog("DeleteCompany?id=" + id, "删除公司信息-" + Instance.ToString());
            ResultInfo result = new ResultInfo();
            CompanyRepository.Instance.Transaction(() =>
            {
                if (CompanyRepository.Instance.Delete(x => x.Id == id))
                {
                    CompanyResourseRepository.Instance.Delete(x => x.CompanyId == id);

                    result.Data = true;
                    result.Message = "操作成功";
                    result.Success = true;
                }
            });

            List<CompanyResourse> deleteResourses = CompanyResourseRepository.Instance.Find(x => x.CompanyId == id).ToList();
            foreach (CompanyResourse resourceItem in deleteResourses)
            {
                SingleFileManager.DeleteRelationResource(resourceItem.ResourceId, "", "");
            }
            return result;
        }

        #endregion

        /// <summary>
        /// 获取公司实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Company GetById(long id)
        {
            var entity = CompanyRepository.Instance.Find(x => x.Id == id).FirstOrDefault();
            return entity;
        }
    }
}
