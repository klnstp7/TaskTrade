using Peacock.Common.Exceptions;
using Peacock.PEP.Data.Entities;
using Peacock.PEP.Model.Enum;
using Peacock.PEP.Repository.Repositories;
using Peacock.PEP.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacock.PEP.Service
{
    public class ConfigSolutionService : SingModel<ConfigSolutionService>
    {
        private ConfigSolutionService() { }

        /// <summary>
        /// 获取解决方案列表
        /// </summary>
        /// <param name="SolutionType"></param>
        /// <param name="MoudelCode"></param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public List<ConfigSolution> GetList(UseType SolutionType, string MoudelCode, int index, int size, out int total)
        {
            var userInfo = UserService.Instance.GetCrmUser().Id;
            var query = ConfigSolutionRepository.Instance.Source.Where(x => x.ConfigListFunction.FunCode == MoudelCode && x.UserID == userInfo);
            if (SolutionType != UseType.全部)
                query = query.Where(x => x.SolutionType == SolutionType);
            query = query.OrderByDescending(x => x.tid);
            return ConfigSolutionRepository.Instance.FindForPaging(size, index, query, out total).ToList();
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="Tid"></param>
        /// <returns></returns>
        public ConfigSolution GetSolutionEntityById(long Tid)
        {
            return ConfigSolutionRepository.Instance.Find(x => x.tid == Tid).FirstOrDefault();
        }

        /// <summary>
        /// 根据FuncCode获取功能模块标识
        /// </summary>
        /// <param name="FuncCode"></param>
        /// <returns></returns>
        public ConfigListFunction GetConfigListFunctionByFuncCode(FuncCodeType FuncCode)
        {
            return ConfigListFunctionRepository.Instance.Find(x => x.FunCode == FuncCode.ToString()).FirstOrDefault();
        }

        public ConfigListFunction GetConfigListFunctionByID(long Tid)
        {
            return ConfigListFunctionRepository.Instance.Find(x => x.tid == Tid).FirstOrDefault();
        }
        /// <summary>
        /// 保存解决方案
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool SaveSolution(ConfigSolution entity)
        {
            if (string.IsNullOrEmpty(entity.SolutionName))
                throw new ServiceException("解决方案名称不能为空");
            var userInfo = UserService.Instance.GetCrmUser().Id;
            var exists = ConfigSolutionRepository.Instance.Count(x => x.SolutionName == entity.SolutionName && x.UserID == userInfo && x.tid != entity.tid);
            entity.UserID = UserService.Instance.GetCrmUser().Id;
            var bl = false;
            if (exists > 0)
                throw new ServiceException("解决方案已经存在");
            if (entity.tid < 1)
                bl = ConfigSolutionRepository.Instance.Insert(entity);
            else
                bl = ConfigSolutionRepository.Instance.Save(entity);
            if (entity.IsDefault)
                SetSolutionIsDefault(entity.tid, entity.SolutionType);
            return bl;
        }

        /// <summary>
        /// 保存用户配置的列信息
        /// </summary>
        /// <param name="SolutionId"></param>
        /// <param name="ColIds"></param>
        /// <returns></returns>
        public bool SaveSolutionColInfo(long SolutionId, List<long> ColIds)
        {
            if (SolutionId < 1)
                throw new ServiceException("解决方案ID不能为空");
            if (ColIds.Count < 1)
                throw new ServiceException("至少选择一项需要显示的列");
            try
            {
                ConfigUserFuncColRepository.Instance.Transaction(() =>
                {
                    ConfigUserFuncColRepository.Instance.Delete(x => x.SolutionID == SolutionId);
                    foreach (var colid in ColIds)
                    {
                        ConfigUserFuncCol entity = new ConfigUserFuncCol
                        {
                            SolutionID = SolutionId,
                            ColID = colid
                        };
                        ConfigUserFuncColRepository.Instance.Insert(entity);
                    }
                });
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 根据解决方案ID获取配置的相关列（用于选择方案返回对应列）
        /// </summary>
        /// <param name="SolutionID">方案ID(用户不选时，请传0)</param>
        /// <param name="FuncCode">模块编码(方案ID不为空时传入，用于查询默认显示的列)</param>
        /// <returns></returns>
        public List<ConfigFunctioncol> GetUserConfigColBySolutionId(long SolutionID, FuncCodeType FuncCode,string  useType)
        {
            var data = new List<ConfigFunctioncol>();
            if (SolutionID > 0)
                data = GetSolutionEntityById(SolutionID).ConfigUserFuncCols.Select(x => x.ConfigFunctioncol).ToList();
            else
                data = ConfigFunctioncolRepository.Instance.Find(x => x.ConfigListFunction.FunCode == FuncCode.ToString() && x.IsDefault == true).OrderBy(x => x.OrderBy).ToList();
            if (data.Count < 1)//防止方案中没有列，这种只有测试数据中有，基本用不上
                throw new ServiceException("请在方案中配置数据列！");
                //data = ConfigFunctioncolRepository.Instance.Find(x => x.ConfigListFunction.FunCode == FuncCode.ToString() && x.IsDefault == true).OrderBy(x => x.OrderBy).ToList();
            return data;
        }

        /// <summary>
        /// 根据模块标识获取列信息
        /// </summary>
        /// <param name="FuncCode"></param>
        /// <returns></returns>
        public List<ConfigFunctioncol> GetConfigFuncColListByFuncCode(FuncCodeType FuncCode, UseType UseType)
        {
            return ConfigFunctioncolRepository.Instance.Find(x => x.ConfigListFunction.FunCode == FuncCode.ToString() && x.UseType == UseType).ToList();
        }

        /// <summary>
        /// 删除解决方案
        /// </summary>
        /// <param name="SolutionId"></param>
        public void DeleteSolutionById(long SolutionId)
        {
            ConfigSolutionRepository.Instance.Delete(x => x.tid == SolutionId);
        }

        /// <summary>
        /// 设置默认方案
        /// </summary>
        /// <param name="SolutionId"></param>
        /// <param name="SolutionType"></param>
        public void SetSolutionIsDefault(long SolutionId, UseType SolutionType)
        {
            ConfigSolutionRepository.Instance.Transaction(() =>
            {
                var SameSolution = ConfigSolutionRepository.Instance.Find(x => x.SolutionType == SolutionType && x.tid != SolutionId).ToList();
                foreach (var entity in SameSolution)
                {
                    entity.IsDefault = false;
                    ConfigSolutionRepository.Instance.Save(entity);
                }
                var DefaultSolution = ConfigSolutionRepository.Instance.Find(x => x.tid == SolutionId).FirstOrDefault();
                DefaultSolution.IsDefault = true;
                ConfigSolutionRepository.Instance.Save(DefaultSolution);
            });
        }

        /// <summary>
        /// 根据当前登录用户所拥有的解决方案（用于读取操作界面下拉框的数据）
        /// </summary>
        /// <returns></returns>
        public List<ConfigSolution> GetUserSolutionByUserId(UseType SolutionType, FuncCodeType FuncCode)
        {
            var userId = UserService.Instance.GetCrmUser().Id;
            return ConfigSolutionRepository.Instance.Find(x => x.UserID == userId && x.SolutionType == SolutionType && x.ConfigListFunction.FunCode == FuncCode.ToString()).ToList();
        }

        /// <summary>
        /// 获取用户默认方案列表，若无默认，则全部列展示
        /// </summary>
        /// <param name="solutionType"></param>
        /// <param name="funcCode"></param>
        /// <returns></returns>
        public List<ConfigFunctioncol> GetUserDefaultCol(UseType solutionType,FuncCodeType funcCode){
            var userId = UserService.Instance.GetCrmUser().Id;
            var solution= ConfigSolutionRepository.Instance.Find(x => x.UserID == userId && x.SolutionType == solutionType 
                    && x.IsDefault && x.ConfigListFunction.FunCode == funcCode.ToString()).FirstOrDefault();
            if (solution == null || solution.ConfigUserFuncCols == null || solution.ConfigUserFuncCols.Count==0)
            {
                return ConfigFunctioncolService.Instance.GetListByFunction(funcCode, solutionType);
            }
            else
                return solution.ConfigUserFuncCols.Select(c=>c.ConfigFunctioncol).ToList();
        }
    }
}
