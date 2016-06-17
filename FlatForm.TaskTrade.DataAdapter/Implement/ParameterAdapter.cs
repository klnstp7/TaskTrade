using System.Runtime.InteropServices;
using Peacock.PEP.Data.Entities;
using Peacock.PEP.DataAdapter.Interface;
using Peacock.PEP.Model;
using Peacock.PEP.Service;
using System.Collections.Generic;
using System.Linq;

namespace Peacock.PEP.DataAdapter.Implement
{
    /// </summary>
    /// <summary>
    /// 系统参数数据转换类
    /// </summary>
    public class ParameterAdapter : IParameterAdapter
    {
        private IList<Parameter> allParameters = ParameterService.Instance.GetParameterList();
        /// <summary>
        /// 获取参数信息
        /// </summary>
        /// <returns></returns>
        public IList<ParameterModel> GetParameterList()
        {
            var Source = ParameterService.Instance.GetAll();
            return Source.ToListModel<ParameterModel, Parameter>();
        }
        public List<ParamTreeModel> GetTreeData(long ParentId, IList<Parameter> Source, bool isChildren = true)
        {
            List<ParamTreeModel> returndata = new List<ParamTreeModel>();
            var children = Source.Where(x => x.ParentId == ParentId);
            foreach (var item in children)
            {
                ParamTreeModel tree = new ParamTreeModel();
                tree.id = item.Id;
                tree.text = item.Name;
                tree.value = item.Value;
                tree.ParentID = item.ParentId;
                if (isChildren)
                {
                    var count = allParameters.Count(x => x.ParentId == item.Id);
                    tree.state = count > 0 ? "closed" : "";
                    if (count > 0)
                    {
                        tree.children = GetTreeData(item.Id, Source);
                    }
                    else
                    {
                        tree.children = null;
                    }
                }
                returndata.Add(tree);
            }
            return returndata;
        }
        /// <summary>
        /// 保存参数
        /// </summary>
        /// <param name="dto"></param>
        public void SaveParameter(ParameterModel model)
        {
            var parameter = model.ToModel<Parameter>();
            ParameterService.Instance.Create(parameter);
        }

        public ParameterModel GetParameterTree(string name)
        {
            return ParameterService.Instance.GetParameterTree(name).ToModel<ParameterModel>();
        }

        /// <summary>
        /// 根据主键获取参数
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        public ParameterModel GetParameterById(long id)
        {
            return ParameterService.Instance.GetParameterEntityById(id).ToModel<ParameterModel>();
        }

        /// <summary>
        /// 根据父节点获取数据列表
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public IList<ParameterModel> GetListByParentId(long parentId)
        {
            return ParameterService.Instance.GetListByParentId(parentId).ToList().ToListModel<ParameterModel, Parameter>();
        }

        /// <summary>
        /// 通过名称查询数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ParameterModel GetDataByName(string name)
        {
            return ParameterService.Instance.GetDataByName(name).ToModel<ParameterModel>();
        }

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="id"></param>
        public void Delete(long id)
        {
            ParameterService.Instance.Delete(id);
        }
    }

}
