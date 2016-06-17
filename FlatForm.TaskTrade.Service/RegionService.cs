using Peacock.PEP.Data.Entities;
using Peacock.PEP.Service.Base;
using Peacock.PEP.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Peacock.PEP.Repository.Repositories;
using ResourceLibraryExtension.Helper;
using MemcacheClient;

namespace Peacock.PEP.Service
{
    public class RegionService : SingModel<RegionService>
    {
        private static readonly ICacheClient MemcachedClient = CacheClient.GetInstance();
        private static readonly string RegionCacheKey = "Pep_Region";

        private RegionService()
        {

        }

        /// <summary>
        /// 获取所有Region列表
        /// </summary>
        /// <returns></returns>
        public IList<Region> GetAll()
        {
            IList<Region> list =null;

            if (MemcachedClient.Get(RegionCacheKey) == null)
            {
                list = RegionRepository.Instance.Source.ToList();
                MemcachedClient.Set(RegionCacheKey, list, 60 * 60 * 24);//默认缓存时间1天
            }
            else
            {
                list = MemcachedClient.Get<List<Region>>(RegionCacheKey);
            }
            return list;
        }
    }
}
