using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jqpress.Blog.Data;
using Jqpress.Blog.Entity;

namespace Jqpress.Blog.Services
{
    /// <summary>
    /// 博客文章管理
    /// </summary>
    public class PostService
    {
        /// <summary>
        /// 列表
        /// </summary>
        private static List<PostInfo> _posts;
        /// <summary>
        /// 列表统计数量
        /// </summary>
        private static int _postcount;

        /// <summary>
        /// lock
        /// </summary>
        private static object lockHelper = new object();

        static PostService()
        {
            LoadPost();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public static void LoadPost()
        {
            if (_posts == null)
            {
                lock (lockHelper)
                {
                    if (_posts == null)
                    {
                        _posts = DatabaseProvider.Instance.GetPostList();
                    }
                }
            }
        }


        /// <summary>
        /// 添加文章
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        public static int InsertPost(PostInfo post)
        {
            post.PostId = DatabaseProvider.Instance.InsertPost(post);

            _posts.Add(post);
            _posts.Sort();

            //统计
            StatisticsService.UpdateStatisticsPostCount(1);
            //用户
            UserService.UpdateUserPostCount(post.UserId, 1);
            //分类
            CategoryService.UpdateCategoryCount(post.CategoryId, 1);
            //标签
            TagService.UpdateTagUseCount(post.Tag, 1);

            //   RemovePostsCache();

            return post.PostId;
        }

        /// <summary>
        /// 修改文章
        /// </summary>
        /// <param name="_postinfo"></param>
        /// <returns></returns>
        public static int UpdatePost(PostInfo _postinfo)
        {
            //   PostInfo oldPost = GetPost(_postinfo.PostId);   //好像有问题,不能缓存

            PostInfo oldPost = GetPostByDatabase(_postinfo.PostId);

            int result = DatabaseProvider.Instance.UpdatePost(_postinfo);

            if (oldPost != null && oldPost.CategoryId != _postinfo.CategoryId)
            {
                //分类
                CategoryService.UpdateCategoryCount(oldPost.CategoryId, -1);
                CategoryService.UpdateCategoryCount(_postinfo.CategoryId, 1);
            }

            //     CacheHelper.Remove(CacheKey);

            //标签
            TagService.UpdateTagUseCount(oldPost.Tag, -1);
            TagService.UpdateTagUseCount(_postinfo.Tag, 1);

            //   RemovePostsCache();

            return result;
        }

        /// <summary>
        /// 删除文章
        /// </summary>
        /// <param name="postid"></param>
        /// <returns></returns>
        public static int DeletePost(int postid)
        {
            PostInfo oldPost = GetPost(postid);

            _posts.Remove(oldPost);

            int result = DatabaseProvider.Instance.DeletePost(postid);

            //统计
            StatisticsService.UpdateStatisticsPostCount(-1);
            //用户
            UserService.UpdateUserPostCount(oldPost.UserId, -1);
            //分类
            CategoryService.UpdateCategoryCount(oldPost.CategoryId, -1);
            //标签
            TagService.UpdateTagUseCount(oldPost.Tag, -1);

            //删除所有评论
            CommentService.DeleteCommentByPost(postid);

            //     RemovePostsCache();

            return result;
        }

        /// <summary>
        /// 根据Id获取文章
        /// </summary>
        /// <param name="postid"></param>
        /// <returns></returns>
        public static PostInfo GetPost(int postid)
        {
            //PostInfo p = DatabaseProvider.Instance.GetPost(postid);
            ////  BuildPost(p);
            //return p;


            foreach (PostInfo post in _posts)
            {
                if (post.PostId == postid)
                {
                    return post;
                }
            }
            return null;
        }

        /// <summary>
        /// 从数据库获取文章
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        public static PostInfo GetPostByDatabase(int postId)
        {
            return DatabaseProvider.Instance.GetPost(postId);
        }

        /// <summary>
        /// 根据别名获取文章
        /// </summary>
        /// <param name="slug"></param>
        /// <returns></returns>
        public static PostInfo GetPost(string slug)
        {
            foreach (PostInfo post in _posts)
            {
                if (!string.IsNullOrEmpty(slug) && post.Slug.ToLower() == slug.ToLower())
                {
                    return post;
                }
            }
            return null;
        }

        ///// <summary>
        ///// 获取文章列表
        ///// </summary>
        ///// <param name="pageSize"></param>
        ///// <param name="pageIndex"></param>
        ///// <param name="recordCount"></param>
        ///// <returns></returns>
        //public static List<PostInfo> GetPostList(int pageSize, int pageIndex, out int recordCount)
        //{
        //    return GetPostList(pageSize, pageIndex, out recordCount,-1, -1, -1,-1, -1, -1, -1, null, null, null);
        //}


        /// <summary>
        /// 获取全部文章,是缓存的
        /// </summary>
        /// <returns></returns>
        public static List<PostInfo> GetPostList()
        {
            return _posts;
        }

        /// <summary>
        /// 获取文章数
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="tagId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static int GetPostCount(int categoryId, int tagId, int userId)
        {
            int recordCount = 0;
            GetPostList(1, 1, out recordCount, categoryId, tagId, userId, -1, -1, -1, -1, string.Empty, string.Empty, string.Empty);

            return recordCount;
        }

        /// <summary>
        /// 获取文章数
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="tagId"></param>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        /// <param name="hidestatus"></param>
        /// <returns></returns>
        public static int GetPostCount(int categoryId, int tagId, int userId,int status,int hidestatus)
        {
            int recordCount = 0;
            GetPostList(1, 1, out recordCount, categoryId, tagId, userId, -1, status, -1, hidestatus,  string.Empty, string.Empty, string.Empty);

            return recordCount;
        }

        /// <summary>
        /// 获取文章列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="recordCount"></param>
        /// <param name="categoryId"></param>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        /// <param name="topstatus"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public static List<PostInfo> GetPostList(int pageSize, int pageIndex, out int recordCount, int categoryId, int tagId, int userId, int recommend, int status, int topstatus, int hidestatus, string begindate, string enddate, string keyword)
        {
            List<PostInfo> list;
            try {
                if (pageIndex == 1 && tagId <= 0 && string.IsNullOrEmpty(begindate)&& string.IsNullOrEmpty(enddate) && string.IsNullOrEmpty(keyword))
                {
                    list = GetPostList(pageSize, categoryId, userId, recommend, status, topstatus, hidestatus);
                    recordCount = _postcount;
                }
                else
                {
                    list = DatabaseProvider.Instance.GetPostList(pageSize, pageIndex, out recordCount, categoryId, tagId, userId, recommend, status, topstatus, hidestatus, begindate, enddate, keyword);
                }

                return list;
            }catch(Exception e){
                throw e;
            }

        }

        public static List<PostInfo> GetPostList(int rowCount, int categoryId, int userId, int recommend, int status, int topstatus, int hidestatus)
        {
            try{
                     List<PostInfo> list = _posts;
                    if (categoryId != -1)
                    {
                        list = list.FindAll(post => post.CategoryId == categoryId);
                    }

                    if (userId != -1)
                    {
                        list = list.FindAll(post => post.UserId == userId);
                    }
                    if (recommend != -1)
                    {
                        list = list.FindAll(post => post.Recommend == recommend);
                    }
                    if (status != -1)
                    {
                        list = list.FindAll(post => post.Status == status);
                    }
                    if (topstatus != -1)
                    {
                        list = list.FindAll(post => post.TopStatus == topstatus);
                    }
                    if (hidestatus != -1)
                    {
                        list = list.FindAll(post => post.HideStatus == hidestatus);
                    }
                    _postcount = list.Count;
                    if (rowCount > list.Count)
                    {
                        return list;
                    }
                    List<PostInfo> list2 = new List<PostInfo>();
                    for (int i = 0; i < rowCount; i++)
                    {
                        list2.Add(list[i]);
                    }
                    return list2;
            
            }catch(Exception e){
                throw e;
            
            }
       
        }


        /// <summary>
        /// 更新点击数
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="addCount"></param>
        /// <returns></returns>
        public static int UpdatePostViewCount(int postId, int addCount)
        {
            //   CacheHelper.Remove(CacheKey);

            PostInfo post = GetPost(postId);

            if (post != null)
            {
                post.ViewCount += addCount;
            }
            return DatabaseProvider.Instance.UpdatePostViewCount(postId, addCount);
        }

        /// <summary>
        /// 更新评论数
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="addCount"></param>
        /// <returns></returns>
        public static int UpdatePostCommentCount(int postId, int addCount)
        {
            PostInfo post = GetPost(postId);

            if (post != null)
            {
                post.CommentCount += addCount;

                return DatabaseProvider.Instance.UpdatePost(post);
            }
            return 0;

        }
    }
}
