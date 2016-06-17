using MemcacheClient;
using Memcached.ClientLibrary;
using Peacock.Common;
using Peacock.Common.Exceptions;
using Peacock.Common.Helper;
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
    public class ParameterService : SingModel<ParameterService>
    {
        private static readonly ICacheClient MemcachedClient = CacheClient.GetInstance();

        private static readonly string ParameterCacheKey = "PEPSysParameters1";

        private ParameterService()
        {
        }

        /// <summary>
        /// 创建参数
        /// </summary>
        /// <param name="dto"></param>
        public void Create(Parameter entity)
        {
            LogHelper.Ilog("Create?entity=" + entity.ToJson(), "创建参数-" + Instance.ToString());
            if (string.IsNullOrWhiteSpace(entity.Name) || String.IsNullOrWhiteSpace(entity.Value))
            {
                throw new ServiceException("传入数据中,必要数据字段存为空!");
            }
            var allParameters = Instance.GetAll();
            if (allParameters.Any(x => x.Id != entity.Id && x.Name == entity.Name) || allParameters.Any(x => x.Value == entity.Value && x.Id != entity.Id))
            {
                throw new ServiceException("已存在相同名称或者值的参数");
            }
            if (entity.Id < 1)
            {
                entity.CreatedTime = DateTime.Now;
                ParameterRepository.Instance.Insert(entity);
            }
            else
            {
                var param = ParameterRepository.Instance.Source.FirstOrDefault(x => x.Id == entity.Id);
                if (param != null)
                {
                    param.Name = entity.Name;
                    param.Value = entity.Value;
                    ParameterRepository.Instance.Save(param);
                }
            }
            DeleteParameterCache();
        }

        /// <summary>
        /// 删除参数缓存
        /// </summary>
        public void DeleteParameterCache()
        {
            LogHelper.Ilog("DeleteParameterCache?ParameterCacheKey=" + ParameterCacheKey, "删除参数-" + Instance.ToString());            
            MemcachedClient.Delete(ParameterCacheKey);
        }
        /// <summary>
        /// 获取参数信息列表
        /// </summary>
        /// <returns></returns>
        public IList<Parameter> GetParameterList(string ParametereName = "")
        {
            LogHelper.Ilog("GetParameterList?ParametereName=" + ParametereName, "获取参数信息列表-" + Instance.ToString());            
            if (string.IsNullOrEmpty(ParametereName))
                return GetAll();
            else
                return GetAll().Where(x => x.Name == ParametereName).ToList();
        }

        /// <summary>
        /// 根据名称获取参数树列表
        /// </summary>
        /// <param name="dtoInput">
        public Parameter GetParameterTree(string name)
        {
            LogHelper.Ilog("GetParameterTree?name=" + name, "根据名称获取参数树列表-" + Instance.ToString());
            return GetAll().FirstOrDefault(x => x.Name == name);
        }
        /// <summary>
        /// 根据参数名获取数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Parameter GetDataByName(string name)
        {
            LogHelper.Ilog("GetDataByName?name=" + name, "根据参数名获取数据-" + Instance.ToString());
            return GetAll().FirstOrDefault(x => x.Name == name);
        }
        /// <summary>
        /// 根据主键获取参数
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        public Parameter GetParameterEntityById(long tid)
        {
            LogHelper.Ilog("GetParameterEntityById?tid=" + tid, "根据主键获取参数-" + Instance.ToString());
            return GetAll().FirstOrDefault(x => x.Id == tid);
        }
        /// <summary>
        /// 根据父级ID获取数据
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public IList<Parameter> GetListByParentId(long parentId)
        {
            LogHelper.Ilog("GetListByParentId?parentId=" + parentId, "获取子集参数-" + Instance.ToString());
            return GetAll().Where(x => x.ParentId == parentId).ToList();
            //return null;
        }

        /// <summary>
        /// 根据ID删除参数
        /// </summary>
        /// <param name="id"></param>
        public void Delete(long id)
        {
            LogHelper.Ilog("Delete?id=" + id, "删除参数-" + Instance.ToString());
            var childrencount = ParameterRepository.Instance.Find(x => x.ParentId == id).Count();
            if (childrencount > 0)
            {
                throw new ServiceException("参数项有下级参数，不能删除");
            }
            ParameterRepository.Instance.Transaction(delegate ()
            {
                var entity = ParameterRepository.Instance.Find(x => x.Id == id).FirstOrDefault();
                ParameterRepository.Instance.Delete(x => x.Id == id);
                DeleteParameterCache();
            });
        }

        /// <summary>
        /// 获取所有数据 
        /// </summary>
        /// <returns></returns>
        public IList<Parameter> GetAll()
        {
            LogHelper.Ilog("GetAll", "获取所有数据-" + Instance.ToString());
            IList<Parameter> list = new List<Parameter>();
            if (MemcachedClient.Get(ParameterCacheKey) == null)
            {
                list = ParameterRepository.Instance.Source.ToList<Parameter>();
                MemcachedClient.Set(ParameterCacheKey, list, 60 * 60 * 24);//默认缓存时间1天
            }
            else
            {
                list = MemcachedClient.Get<IList<Parameter>>(ParameterCacheKey);
            }
            return list;
        }
    }
}
