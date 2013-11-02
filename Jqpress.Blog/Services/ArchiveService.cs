using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Jqpress.Blog.Entity;
using Jqpress.Framework.Cache;
using Jqpress.Blog.Data;

namespace Jqpress.Blog.Services
{
    /// <summary>
    /// 归档管理
    /// </summary>
    public class ArchiveService
    {

        /// <summary>
        /// 获取归档
        /// </summary>
        /// <returns></returns>
        public static List<ArchiveInfo> GetArchive()
        {
            string archiveCacheKey = "archive";
            List<ArchiveInfo> list = (List<ArchiveInfo>)CacheHelper.Get(archiveCacheKey);

            if (list == null)
            {
                list = DatabaseProvider.Instance.GetArchive();


                CacheHelper.Insert(archiveCacheKey, list, CacheHelper.HourFactor * 12);

            }

            return list;
        }
    }
}
