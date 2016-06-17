using Peacock.PEP.Model;
using System.Collections.Generic;

namespace Peacock.PEP.DataAdapter.Interface
{
    public interface IParameterAdapter
    {
        /// <summary>
        /// 获取参数树列表
        /// </summary>
        /// <param name="dtoInput">
        /// 输入的DTO对象。
        /// </param>
        /// <returns></returns>
        ParameterModel GetParameterTree(string name);

        /// <summary>
        /// 根据主键获取参数
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        ParameterModel GetParameterById(long id);

        /// <summary>
        /// 根据父节点获取数据列表
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        IList<ParameterModel> GetListByParentId(long parentId);

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="id"></param>
        void Delete(long id);

        /// <summary>
        /// 保存参数
        /// </summary>
        /// <param name="dto"></param>
        void SaveParameter(ParameterModel model);

        /// <summary>
        /// 通过名称查询数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        ParameterModel GetDataByName(string name);

        /// <summary>
        /// 获取参数信息
        /// </summary>
        /// <returns></returns>
        IList<ParameterModel> GetParameterList();
    }
}
