using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jqpress.Blog.Data;
using Jqpress.Blog.Entity;
using Jqpress.Framework.Utils;

namespace Jqpress.Blog.Services
{
   public class TagService
    {
        /// <summary>
        /// 标签列表
        /// </summary>
        private static List<TagInfo> _tags;

        private static object lockHelper = new object();

        /// <summary>
        /// 初始化
        /// </summary>
        public static void LoadTag()
        {
            if (_tags == null)
            {
                lock (lockHelper)
                {
                    if (_tags == null)
                    {
                        _tags = DatabaseProvider.Instance.GetTagList();
                    }
                }
            }
        }

        /// <summary>
        /// 标签列表
        /// </summary>
        private static List<TagInfo> Tags
        {
            get
            {
                if (_tags == null)
                {
                    LoadTag();
                }

                return _tags;
            }
        }

        /// <summary>
        /// 获取全部
        /// </summary>
        /// <returns></returns>
        public static List<TagInfo> GetTagList()
        {
            return Tags;
        }

        /// <summary>
        /// 添加标签
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static int InsertTag(TagInfo tag)
        {
            tag.TagId = DatabaseProvider.Instance.InsertTag(tag);

            Tags.Add(tag);
            Tags.Sort();

            return tag.TagId;
        }

        /// <summary>
        /// 修改标签
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static int UpdateTag(TagInfo tag)
        {
            Tags.Sort();
            return DatabaseProvider.Instance.UpdateTag(tag);
        }

        /// <summary>
        /// 删除标签
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
        public static int DeleteTag(int tagId)
        {
            Tags.RemoveAll(tag => tag.TagId == tagId);
            return DatabaseProvider.Instance.DeleteTag(tagId);
        }

        /// <summary>
        /// 获取标签
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
        public static TagInfo GetTag(int tagId)
        {
            foreach (TagInfo t in Tags)
            {
                if (t.TagId == tagId)
                {
                    return t;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取标签
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static TagInfo GetTag(string name)
        {

            foreach (TagInfo t in Tags)
            {
                if (t.CateName == name)
                {
                    return t;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取标签
        /// </summary>
        /// <param name="slug"></param>
        /// <returns></returns>
        public static TagInfo GetTagBySlug(string slug)
        {

            foreach (TagInfo t in Tags)
            {

                if (!string.IsNullOrEmpty(slug) && t.Slug.ToLower() == slug.ToLower())
                {
                    return t;
                }
            }
            return null;
        }


        /// <summary>
        /// 获取指定条数标签
        /// </summary>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public static List<TagInfo> GetTagList(int rowCount)
        {
            if (Tags.Count <= rowCount)
            {
                return Tags;
            }
            else
            {
                var list = Tags.AsQueryable().OrderByDescending(t => t.PostCount).ToList();
                list = list.Take(rowCount).ToList();
                return list;
            }
        }

        /// <summary>
        /// 获取分页标签
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public static List<TagInfo> GetTagList(int pageSize, int pageIndex, out int recordCount)
        {
            recordCount = Tags.Count;
            List<TagInfo> rlist = new List<TagInfo>();

            int start = (pageIndex - 1) * pageSize;
            int end = start + pageSize;
            if (end > Tags.Count)
            {
                end = Tags.Count;
            }
            for (int i = start; i < end; i++)
            {
                rlist.Add(Tags[i]);
            }
            return rlist;
        }



        /// <summary>
        /// 获取ID
        /// </summary>
        /// <param name="tagID"></param>
        /// <returns></returns>
        public static int GetTagId(string slug)
        {

            foreach (TagInfo t in Tags)
            {

                if (!string.IsNullOrEmpty(slug) && t.Slug.ToLower() == slug.ToLower())
                {
                    return t.TagId;
                }
            }
            return 0;
        }

        /// <summary>
        /// 获取标签名称
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
        public static string GetTagName(int tagId)
        {

            foreach (TagInfo t in Tags)
            {
                if (t.TagId == tagId)
                {
                    return t.CateName;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 根据标签Id获取标签列表
        /// </summary>
        /// <param name="ids">标签Id,逗号隔开</param>
        /// <returns></returns>
        public static List<TagInfo> GetTagList(string ids)
        {


            List<TagInfo> list = GetTagList();

            List<TagInfo> list2 = new List<TagInfo>();

            string[] tempids = ids.Split(',');

            //foreach (TagInfo term in list)
            //{
            //    foreach (string str in tempids)
            //    {
            //        if (term.TagId.ToString() == str)
            //        {
            //            list2.Add(term);

            //            continue;
            //        }
            //    }
            //}
            foreach (string str in tempids)
            {
                foreach (TagInfo tag in list)
                {
                    if (tag.TagId.ToString() == str)
                    {
                        list2.Add(tag);
                        continue;
                    }
                }
            }
            return list2;
        }

        /// <summary>
        /// 更新标签对应文章数
        /// </summary>
        /// <param name="tagids">格式:{2}{26}</param>
        /// <returns></returns>
        public static bool UpdateTagUseCount(string tagids, int addCount)
        {
            if (string.IsNullOrEmpty(tagids))
            {
                return false;
            }

            string[] tagidlist = tagids.Replace("{", "").Split('}');

            foreach (string tagId in tagidlist)
            {
                TagInfo tag = GetTag(Jqpress.Framework.Utils.TypeConverter.StrToInt(tagId, 0));
                if (tag != null)
                {
                    tag.PostCount += addCount;
                    DatabaseProvider.Instance.UpdateTag(tag);
                }
            }
            return true;
        }
    }
}
