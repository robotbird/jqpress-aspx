using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jqpress.Blog.Data;
using Jqpress.Blog.Entity;

namespace Jqpress.Blog.Services
{
    public class StatisticsService
    {
        /// <summary>
        /// 缓存统计
        /// </summary>
        private static StatisticsInfo _statistics=null;

        /// <summary>
        /// lock
        /// </summary>
        private static object lockHelper = new object();

        static StatisticsService()
        {
            LoadStatistics();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private static void LoadStatistics()
        {
            if (_statistics == null)
            {
                lock (lockHelper)
                {
                    if (_statistics == null)
                    {
                        _statistics = DatabaseProvider.Instance.GetStatistics();
                    }
                }
            }
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <returns></returns>
        public static StatisticsInfo GetStatistics()
        {
            return _statistics;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        public static bool UpdateStatistics()
        {
            return DatabaseProvider.Instance.UpdateStatistics(_statistics);
        }

        /// <summary>
        /// 更新文章数
        /// </summary>
        /// <param name="addCount">增加数，可为负数</param>
        /// <returns></returns>
        public static bool UpdateStatisticsPostCount(int addCount)
        {
            _statistics.PostCount += addCount;
            return UpdateStatistics();
        }

        /// <summary>
        /// 更新评论数
        /// </summary>
        /// <param name="addCount">增加数，可为负数</param>
        /// <returns></returns>
        public static bool UpdateStatisticsCommentCount(int addCount)
        {
            _statistics.CommentCount += addCount;
            return UpdateStatistics();
        }
    }
}
