using Peacock.PEP.Model.DTO;
using Peacock.PEP.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacock.PEP.DataAdapter.Interface
{
    public interface IConfigSolutionAdapter
    {
        /// <summary>
        /// 获取解决方案列表，根据功能模块标识、方案类型
        /// </summary>
        /// <param name="SolutionType"></param>
        /// <param name="MoudelCode"></param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        List<ConfigSolutionModel> GetList(string SolutionType, string MoudelCode, int index, int size, out int total);

        /// <summary>
        /// 获取解决方案实体
        /// </summary>
        /// <param name="Tid"></param>
        /// <returns></returns>
        ConfigSolutionModel GetSolutionEntityById(long Tid);

        /// <summary>
        /// 保存解决方案
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool SaveSolution(ConfigSolutionModel entity);

        /// <summary>
        /// 保存用户配置的列信息
        /// </summary>
        /// <param name="SolutionId"></param>
        /// <param name="ColIds"></param>
        /// <returns></returns>
        bool SaveSolutionColInfo(long SolutionId, List<long> ColIds);

        /// <summary>
        /// 根据解决方案ID获取配置的相关列(用户过滤显示和导出字段)
        /// </summary>
        /// <param name="SolutionID">方案ID</param>
        /// <param name="FuncCode">模块编码(方案ID不为空时传入，用于查询默认显示的列)</param>
        /// <returns></returns>
        List<ConfigFunctioncolModel> GetUserConfigColBySolutionId(long SolutionID, FuncCodeTypes FuncCode,string UseType="Query");

        /// <summary>
        /// 删除解决方案
        /// </summary>
        /// <param name="SolutionId"></param>
        void DeleteSolutionById(long SolutionId);

        /// <summary>
        /// 设置默认方案
        /// </summary>
        /// <param name="SolutionId"></param>
        /// <param name="SolutionType"></param>
        void SetSolutionIsDefault(long SolutionId, SolutionUseType SolutionType);

        /// <summary>
        /// 根据当前登录用户所拥有的解决方案（用户读取用户解决方案列表、下拉框）
        /// </summary>
        /// <returns></returns>
        List<ConfigSolutionModel> GetUserSolutionByUserId(SolutionUseType SolutionType, FuncCodeTypes FuncCode);

        /// <summary>
        /// 根据FuncCode获取功能模块标识
        /// </summary>
        /// <param name="FuncCode"></param>
        /// <returns></returns>
        ConfigListFunctionModel GetConfigListFunctionByFuncCode(FuncCodeTypes FuncCode);


        /// <summary>
        /// 根据模块标识获取列信息
        /// </summary>
        /// <param name="FuncCode"></param>
        /// <returns></returns>
        List<ConfigFunctioncolModel> GetConfigFuncColListByFuncCode(FuncCodeTypes FuncCode, SolutionUseType solutionType);

        List<ConfigFunctioncolModel> GetUserDefaultCols(SolutionUseType SolutionType, FuncCodeTypes FuncCode);
    }
}
