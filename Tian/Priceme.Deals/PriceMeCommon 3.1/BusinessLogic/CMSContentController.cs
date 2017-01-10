using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PriceMeDBA;

namespace PriceMeCommon
{
    public static class CMSContentController
    {
        public static List<PriceMeDBA.CSK_Content> GetAll()
        {
            return PriceMeDBA.CSK_Content.All().ToList();
        }

        public static PriceMeDBA.CSK_Content Insert(string pageID, string metaDescription, string metaKeywords, string title, string fromEmail, string fromName, string replyTo, int type, string ctx, string fileName)
        {
            PriceMeDBA.CSK_Content ftb = new PriceMeDBA.CSK_Content();

            //ftb.IsNew = true;
            //ftb.PageID = pageID;
            //ftb.MetaDescription = metaDescription;
            //ftb.MetaKeywords = metaKeywords;
            //ftb.Title = title;
            //ftb.FromEmail = fromEmail;
            //ftb.FromName = fromName;
            //ftb.ReplyTo = replyTo;
            //if (type == 1)
            //    ftb.IsEmail = true;
            ftb.Ctx = ctx;
            ftb.FileName = fileName;

            ftb.Save();
            return ftb;
        }

        public static PriceMeDBA.CSK_Content update(int id, string pageID, string metaDescription, string metaKeywords, string title, string fromEmail, string fromName, string replyTo, int type, string ctx, string fileName)
        {
            PriceMeDBA.CSK_Content ftb = new PriceMeDBA.CSK_Content();
            //ftb.IsNew = false;

            //ftb.ID = id;
            //ftb.PageID = pageID;
            //ftb.MetaDescription = metaDescription;
            //ftb.MetaKeywords = metaKeywords;
            //ftb.Title = title;
            //ftb.FromEmail = fromEmail;
            //ftb.FromName = fromName;
            //ftb.ReplyTo = replyTo;
            //if (type == 1)
            //    ftb.IsEmail = true;
            //ftb.Ctx = ctx;
            ftb.FileName = fileName;

            ftb.Save();
            return ftb;
        }

        public static void delete(int id)
        {
            PriceMeDBA.CSK_Content.Delete(content => content.ID == id);
        }

        public static PriceMeDBA.CSK_Content FindByPageID(string pageID)
        {
            PriceMeDBA.CSK_Content content = PriceMeDBA.CSK_Content.SingleOrDefault(cont => cont.PageID == pageID);
            if (content != null)
                return content;
            return null;
        }
    }
}
