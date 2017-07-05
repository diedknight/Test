using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PriceMeDBA;
using PriceMeCommon.BusinessLogic;

namespace PriceMe
{
    public static class CMSContentController
    {
        //public static List<CSK_Content> GetAll(int countryId)
        //{
        //    return CSK_Content.All().ToList();
        //}

        //public static CSK_Content Insert(string pageID, string metaDescription, string metaKeywords, string title, string fromEmail, string fromName, string replyTo, int type, string ctx, string fileName)
        //{
        //    CSK_Content ftb = new CSK_Content();
        //    ftb.Ctx = ctx;
        //    ftb.FileName = fileName;

        //    ftb.Save();
        //    return ftb;
        //}

        //public static CSK_Content update(int id, string pageID, string metaDescription, string metaKeywords, string title, string fromEmail, string fromName, string replyTo, int type, string ctx, string fileName)
        //{
        //    CSK_Content ftb = new CSK_Content();
        //    ftb.FileName = fileName;

        //    ftb.Save();
        //    return ftb;
        //}

        //public static void delete(int id)
        //{
        //    CSK_Content.Delete(content => content.ID == id);
        //}

        public static CSK_Content FindByPageID(string pageId, int countryId)
        {
            using (SubSonic.DataProviders.SharedDbConnectionScope sdbs = new SubSonic.DataProviders.SharedDbConnectionScope(MultiCountryController.GetDBProvider(countryId)))
            {
                CSK_Content content = CSK_Content.SingleOrDefault(cont => cont.PageID == pageId);
                if (content != null)
                    return content;
                return null;
            }
        }
    }
}
