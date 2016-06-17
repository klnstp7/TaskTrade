using Peacock.PEP.Data.Entities;
using Peacock.PEP.DataAdapter.Interface;
using Peacock.PEP.Model.DTO;
using Peacock.PEP.Model.Enum;
using Peacock.PEP.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacock.PEP.DataAdapter.Implement
{
    public class ConfigSolutionAdapter : IConfigSolutionAdapter
    {
        public List<ConfigSolutionModel> GetList(string SolutionType, string MoudelCode, int index, int size, out int total)
        {
            return ConfigSolutionService.Instance.GetList((UseType)Enum.Parse(typeof(UseType), SolutionType), MoudelCode, index, size, out total).ToListModel<ConfigSolutionModel, ConfigSolution>();
        }

        /// <summary>
        /// 获取解决方案实体
        /// </summary>
        /// <param name="Tid"></param>
        /// <returns></returns>
        public ConfigSolutionModel GetSolutionEntityById(long Tid)
        {
            return ConfigSolutionService.Instance.GetSolutionEntityById(Tid).ToModel<ConfigSolutionModel>();
        }

        /// <summary>
        /// 保存解决方案
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool SaveSolution(ConfigSolutionModel entity)
        {
            return ConfigSolutionService.Instance.SaveSolution(entity.ToModel<ConfigSolution>());
        }

        /// <summary>
        /// 保存用户配置的列信息
        /// </summary>
        /// <param name="SolutionId"></param>
        /// <param name="ColIds"></param>
        /// <returns></returns>
        public bool SaveSolutionColInfo(long SolutionId, List<long> ColIds)
        {
            return ConfigSolutionService.Instance.SaveSolutionColInfo(SolutionId, ColIds);
        }

        /// <summary>
        /// 根据解决方案ID获取配置的相关列
        /// </summary>
        /// <param name="SolutionID">方案ID</param>
        /// <param name="FuncCode">模块编码(方案ID不为空时传入，用于查询默认显示的列)</param>
        /// <returns></returns>
        public List<ConfigFunctioncolModel> GetUserConfigColBySolutionId(long SolutionID, FuncCodeTypes FuncCode, string UseType = "Query")
        {
            return ConfigSolutionService.Instance.GetUserConfigColBySolutionId(SolutionID, 
                (FuncCodeType)Enum.Parse(typeof(FuncCodeType), FuncCode.ToString()), UseType).ToListModel<ConfigFunctioncolModel, ConfigFunctioncol>();
        }

        /// <summary>
        /// 删除解决方案
        /// </summary>
        /// <param name="SolutionId"></param>
        public void DeleteSolutionById(long SolutionId)
        {
            ConfigSolutionService.Instance.DeleteSolutionById(SolutionId);
        }

        /// <summary>
        /// 设置默认方案
        /// </summary>
        /// <param name="SolutionId"></param>
        /// <param name="SolutionType"></param>
        public void SetSolutionIsDefault(long SolutionId, SolutionUseType SolutionType)
        {
            ConfigSolutionService.Instance.SetSolutionIsDefault(SolutionId, (UseType)Enum.Parse(typeof(UseType), SolutionType.ToString()));
        }

        /// <summary>
        /// 根据FuncCode获取功能模块标识
        /// </summary>
        /// <param name="FuncCode"></param>
        /// <returns></returns>
        public ConfigListFunctionModel GetConfigListFunctionByFuncCode(FuncCodeTypes FuncCode)
        {
            return ConfigSolutionService.Instance.GetConfigListFunctionByFuncCode((FuncCodeType)Enum.Parse(typeof(FuncCodeType), FuncCode.ToString())).ToModel<ConfigListFunctionModel>();
        }

        /// <summary>
        /// 根据当前登录用户所拥有的解决方案
        /// </summary>
        /// <returns></returns>
        public List<ConfigSolutionModel> GetUserSolutionByUserId(SolutionUseType SolutionType, FuncCodeTypes FuncCode)
        {
            return ConfigSolutionService.Instance.GetUserSolutionByUserId((UseType)Enum.Parse(typeof(UseType), SolutionType.ToString())
                , (FuncCodeType)Enum.Parse(typeof(FuncCodeType), FuncCode.ToString())).ToListModel<ConfigSolutionModel, ConfigSolution>();
        }

        /// <summary>
        /// 根据当前登录用户所拥有的解决方案
        /// </summary>
        /// <returns></returns>
        public List<ConfigFunctioncolModel> GetUserDefaultCols(SolutionUseType SolutionType, FuncCodeTypes FuncCode)
        {
            return ConfigSolutionService.Instance.GetUserDefaultCol((UseType)Enum.Parse(typeof(UseType), SolutionType.ToString())
                , (FuncCodeType)Enum.Parse(typeof(FuncCodeType), FuncCode.ToString())).ToListModel<ConfigFunctioncolModel, ConfigFunctioncol>();
        }


        /// <summary>
        /// 根据模块标识获取列信息
        /// </summary>
        /// <param name="FuncCode"></param>
        /// <returns></returns>
        public List<ConfigFunctioncolModel> GetConfigFuncColListByFuncCode(FuncCodeTypes FuncCode, SolutionUseType solutionType)
        {
            return ConfigSolutionService.Instance.GetConfigFuncColListByFuncCode((FuncCodeType)Enum.Parse(typeof(FuncCodeType), FuncCode.ToString())
                    , (UseType)Enum.Parse(typeof(UseType), solutionType.ToString())).ToListModel<ConfigFunctioncolModel, ConfigFunctioncol>();
        }
    }
}
