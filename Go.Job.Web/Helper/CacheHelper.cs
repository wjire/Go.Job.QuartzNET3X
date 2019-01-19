using System;
using System.Web;
using System.Web.Caching;

namespace Go.Job.Web.Helper
{
    public class CacheHelper
    {

        public static T GetCache<T>(string key)
        {
            object objCache = HttpRuntime.Cache.Get(key);
            if (objCache == null)
            {
                return default(T);
            }
            return (T)objCache;
        }




        /// <summary>
        /// 设置数据缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey">key</param>
        /// <param name="data">value</param>
        /// <param name="timeout">绝对过期时间,单位:秒</param>
        public static void SetCache<T>(string cacheKey, T data, int timeout = 7200)
        {
            try
            {
                if (data == null)
                {
                    return;
                }

                Cache objCache = HttpRuntime.Cache;
                //相对过期  
                //objCache.Insert(cacheKey, data, null, DateTime.MaxValue, timeout, CacheItemPriority.NotRemovable, null);  
                //绝对过期时间  
                objCache.Insert(cacheKey, data, null, DateTime.Now.AddSeconds(timeout), TimeSpan.Zero, CacheItemPriority.High, null);
            }
            catch (Exception)
            {
                //throw;  
            }
        }


        /// <summary>
        /// 设置数据缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey">key</param>
        /// <param name="data">value</param>
        /// <param name="path">依赖的文件全路径</param>
        public static void SetCache<T>(string cacheKey, T data, string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new Exception("文件路径为空");
            }

            CacheDependency caDep = new CacheDependency(path);
            SetCache(cacheKey, data, caDep);
        }

        /// <summary>
        /// 设置文件依赖的数据缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey">key</param>
        /// <param name="data">value</param>
        /// <param name="caDep">文件依赖项</param>
        public static void SetCache<T>(string cacheKey, T data, CacheDependency caDep)
        {
            try
            {
                if (data == null)
                {
                    return;
                }

                Cache objCache = HttpRuntime.Cache;
                objCache.Insert(cacheKey, data, caDep,
                        Cache.NoAbsoluteExpiration, //从不过期
                      Cache.NoSlidingExpiration, //禁用可调过期
                      CacheItemPriority.Default, null);
            }
            catch (Exception)
            {
                //throw;  
            }
        }
    }
}