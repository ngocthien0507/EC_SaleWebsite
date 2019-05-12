using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;

namespace Model.DAO
{
    public class ContentDAO
    {
        TmdtDbContext db = null;
        public ContentDAO()
        {
            db = new TmdtDbContext();
        }
        public Content GetByID(long id)
        {
            return db.Contents.Find(id);
        }/// <summary>
        /// List all content for admin
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IEnumerable<Content> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Content> model = db.Contents;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString) || x.Name.Contains(searchString));
            }
            return model.OrderByDescending(x => x.CreateDate).ToPagedList(page, pageSize);
        }
        /// <summary>
        /// List all content for client
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IEnumerable<Content> ListAllPaging(int page, ref int totalRecord, int pageSize)
        {
            IQueryable<Content> model = db.Contents;
            IEnumerable<Content> a = db.Contents.Where(x => x.Status == true);
            totalRecord = a.Count();
            return model.OrderByDescending(x => x.CreateDate).ToPagedList(page, pageSize);
        }
        public Tag GetTag(string id)
        {
            return db.Tags.Find(id);
        }
        // list all news by tag
        public IEnumerable<Content> ListAllByTag(string tag, ref int totalRecord, int page, int pageSize)
        {
            IEnumerable<Content> q = db.Contents.Where(x => x.Status == true);
            totalRecord = q.Count();
            var model = (from a in db.Contents
                                        join b in db.ContentTags
                                        on a.ID equals b.ContentID
                                        where b.TagID == tag
                                        select new
                                        {
                                            Name = a.Name,
                                            MetaTitle = a.MetaTitle,
                                            Image = a.Image,
                                            Descriptions = a.Descriptions,
                                            CreateDate = a.CreateDate,
                                            CreateBy = a.CreateBy,
                                            ID = a.ID
                                        }).AsEnumerable().Select(x=>new Content()
                                        {
                                            Name = x.Name,
                                            MetaTitle = x.MetaTitle,
                                            Image = x.Image,
                                            Descriptions = x.Descriptions,
                                            CreateDate = x.CreateDate,
                                            CreateBy = x.CreateBy,
                                            ID = x.ID
                                        });
            return model.OrderByDescending(x => x.CreateDate).ToPagedList(page, pageSize);
        }
        //list tag
        public List<Tag> ListTag(long contentId)
        {
            var model = (from a in db.Tags
                         join b in db.ContentTags
                         on a.ID equals b.TagID
                         where b.ContentID == contentId
                         select new
                         {
                             ID = b.TagID,
                             Name = a.Name
                         }).AsEnumerable().Select(x => new Tag()
                         {
                             ID = x.ID,
                             Name=x.Name
                        });
            return model.ToList();
        }
    }
}
