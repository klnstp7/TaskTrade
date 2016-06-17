using Peacock.PEP.Data.Entities;
using Peacock.PEP.Repository.Repositories;
using Peacock.PEP.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacock.PEP.Service
{
    public class ConfigFunctioncolService:SingModel<ConfigFunctioncolService>
    {
        private ConfigFunctioncolService() { }

        /// <summary>
        /// 查询用户所拥有所配置的列
        /// </summary>
        /// <param name="SolutionId"></param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public List<ConfigUserFuncCol> GetListByUser(long SolutionId, int index, int size, out int total)
        {
            var query = ConfigUserFuncColRepository.Instance.Find(x=>x.SolutionID==SolutionId).OrderByDescending(x=>x.SolutionID);
            return ConfigUserFuncColRepository.Instance.FindForPaging(size,index,query,out total).ToList();
        }

        /// <summary>
        /// 根据功能模块标识获取列
        /// </summary>
        /// <param name="SolutionId"></param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public List<ConfigFunctioncol> GetListByFunction(FuncCodeType FuncCode, UseType SolutionTyp)
        {
            //var FunctionId = ConfigListFunctionRepository.Instance.Find(x => x.FunCode == FuncCode).FirstOrDefault().tid;
            var query = ConfigFunctioncolRepository.Instance.Find(x => x.ConfigListFunction.FunCode == FuncCode.ToString() && x.UseType == SolutionTyp).OrderBy(x => x.OrderBy);
            return query.ToList();
        } 
    }
}
