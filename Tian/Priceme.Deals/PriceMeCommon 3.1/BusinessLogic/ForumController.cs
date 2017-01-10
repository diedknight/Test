using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SubSonic.Schema;
using PriceMeDBA;
using System.Net;

namespace PriceMeCommon.BusinessLogic {
    public class ForumController {

        private static readonly bool TopicDefaultIsApproved = true;
        private static readonly bool QPostDefaultIsApproved = true;
        private static PriceMeDBDB db = PriceMeStatic.PriceMeDB;

        public static IDataReader GetRecentProductQuestions( int count) {
            //StoredProcedure sp = new StoredProcedure("CSK_Forum_TopicListShowAllCat");
            StoredProcedure sp = db.CSK_Forum_TopicListShowAllCat();
            sp.Command.AddParameter("@PageSize", count, DbType.Int32);
            return sp.ExecuteReader();
        }

        public static string GetForumUserName(string user) {
            if (string.IsNullOrEmpty(user.Trim()))
                return "anonymous";
            else
                return user.Trim();
        }

        public static string GetForumDate(string date) {
            if (date.Trim() == "")
                return "";

            DateTime d;
            DateTime.TryParse(date, out d);
            return d.ToString("d MMMM yyyy");
        }

        public static IDataReader GetToplicList( int page, int pageSize) {
            //StoredProcedure sp = new StoredProcedure("CSK_Forum_TopicListShowAllCat");
            StoredProcedure sp = db.CSK_Forum_TopicListShowAllCat();
            sp.Command.AddParameter("@Page", page, DbType.Int32);
            sp.Command.AddParameter("@PageSize", pageSize, DbType.Int32);
            return sp.ExecuteReader();
        }

        public static IDataReader GetTopicListByCategory( int cid, int page ,int pageSize ) {
            //StoredProcedure sp = new StoredProcedure("CSK_Forum_TopicList");
            StoredProcedure sp = db.CSK_Forum_TopicList();
            sp.Command.AddParameter("@CATID", cid, DbType.Int32);
            sp.Command.AddParameter("@Page", page, DbType.Int32);
            sp.Command.AddParameter("@PageSize", pageSize, DbType.Int32);
            return sp.ExecuteReader();
        }

        public static IDataReader GetCategorySummary() {
            //StoredProcedure sp = new StoredProcedure("CSK_Forum_CategorySummary");
            StoredProcedure sp = db.CSK_Forum_CategorySummary();
            return sp.ExecuteReader();
        }

        public static IDataReader GetAbuseList( int page, int pageSize) {
            //StoredProcedure sp = new StoredProcedure("CSK_Forum_AbuseList");
            StoredProcedure sp = db.CSK_Forum_AbuseList();
            sp.Command.AddParameter("@PageSize", pageSize, DbType.Int32);
            sp.Command.AddParameter("@Page", page, DbType.Int32);
            return sp.ExecuteReader();
        }

        public static List<CSK_Forum_ReportedAbsu> GetAbuse(int qpostID) {
            //SELECT IP,Description,CreatedOn FROM CSK_Forum_ReportedAbsus WHERE QPostID = @QPostID ORDER BY CreatedOn DESC
            return CSK_Forum_ReportedAbsu.Find(r => r.QPostID == qpostID).OrderByDescending(r => r.CreatedOn).ToList();
        }

        public static IDataReader GetTopicInfo( int topicID ) {
            //StoredProcedure sp = new StoredProcedure("CSK_Forum_TopicInfo");
            StoredProcedure sp = db.CSK_Forum_TopicInfo();
            sp.Command.AddParameter("@TopicID", topicID, DbType.Int32);
            return sp.ExecuteReader();
        }

        public static IDataReader GetQPostsByTopicID( int topicID, int page, int pageSize ) {
            //StoredProcedure sp = new StoredProcedure("CSK_Forum_QPostList");
            StoredProcedure sp = db.CSK_Forum_QPostList();
            sp.Command.AddParameter("@TopicID", topicID, DbType.Int32);
            sp.Command.AddParameter("@PageSize", pageSize, DbType.Int32);
            sp.Command.AddParameter("@Page", page, DbType.Int32);
            return sp.ExecuteReader();
        }

        public static void CloseTopic( int topicID ) {
            //ft = db.CSK_Forum_Topics.Where(t => t.ID == topicID).FirstOrDefault();
            var ft = CSK_Forum_Topic.SingleOrDefault(t => t.ID == topicID);
            if (ft != null) {
                ft.IsClosed = true;
                ft.Update();
            }
        }

        public static void DeleteTopic(int topicID) {
            var qp = CSK_Forum_QPost.SingleOrDefault(q => q.TopicID == topicID);
            if (qp != null) {
                qp.IsApproved = false;
                qp.Update();
            } 
        }

        public static bool ReportAbuse( int qpostID, IPAddress ipa, string desc) {
            var qpost = CSK_Forum_QPost.SingleOrDefault(q => q.ID == qpostID);
            if (qpost != null) {
                qpost.IsApproved = true;
                qpost.Save();

                var frab = new CSK_Forum_ReportedAbsu();
                frab.QPostID = qpostID;

                byte[] bIPAddress = ipa.GetAddressBytes();
                //192.168.0.1
                //1 * 256^3 + 0 * 256^2 + 168 * 256 ^1 + 192 * 256^0
                long ipn = (bIPAddress[3] * 16777216L + bIPAddress[2] * 65536L + bIPAddress[1] * 256L + bIPAddress[0]);
                frab.IP = ipn;
                frab.Description = desc;
                frab.Save();

                return true;
            }
            return false;
        }

        public static void ResetAbuse(int qpostID) {
            var qpost = CSK_Forum_QPost.SingleOrDefault(q => q.ID == qpostID);
            if (qpost != null) {
                qpost.ReportedAsAbuse = false;
                qpost.Save();
            }
        }

        public static void DeleteAbuse( int id ) {
            CSK_Forum_QPost qpost = CSK_Forum_QPost.SingleOrDefault(q => q.ID == id);
            if (qpost != null) {
                qpost.IsApproved = false;
                qpost.Update();
            }        
        }

        public static int NewTopic( int pid, int cid ) {
            CSK_Forum_Topic ft = new CSK_Forum_Topic();
            ft.ProductID = pid;
            ft.CategoryID = cid;
            ft.IsApproved = TopicDefaultIsApproved;
            ft.IsClosed = false;

            ft.Save();
            return ft.ID;
        }

        public static CSK_Forum_QPost UpdateQPost( bool isQuestion, int topicID, int qpostID, string title, string ctx, string showName, IPAddress ipa ) {
            CSK_Forum_QPost fq;
            if (qpostID != 0) {
                fq = CSK_Forum_QPost.SingleOrDefault(q => q.ID == qpostID);
                if (fq == null)
                    throw new Exception("Not found");
            } else {
                fq = new CSK_Forum_QPost();
            }

            fq.Title = title.Trim();
            fq.Ctx = ctx.Trim();
            //fq.IP = ipa.Address;
            byte[] bIPAddress = ipa.GetAddressBytes();
            //192.168.0.1
            //1 * 256^3 + 0 * 256^2 + 168 * 256 ^1 + 192 * 256^0
            long ipn = (bIPAddress[3] * 16777216L + bIPAddress[2] * 65536L + bIPAddress[1] * 256L + bIPAddress[0]);
            fq.IP = ipn;

            fq.ShowName = showName.Trim();

            if (qpostID == 0) {//新贴或新回复
                fq.UserName = showName;//HttpContext.Current.User.Identity.Name;
                fq.TopicID = topicID;
                fq.IsApproved = QPostDefaultIsApproved;
                fq.IsQuestion = isQuestion;
            }

            fq.Save();
            return fq;
        }
    }

    //public class ForumTopic {
    //    public int? CategoryID { get; internal set; }
    //    public string CategoryName { get; internal set; }
    //    public int RN { get; internal set; }
    //    public int CNT { get; internal set; }
    //    public int? ProductID { get; internal set; }
    //    public string ProductName { get; internal set; }
    //    public bool? IsClosed { get; internal set; }
    //    public int? Viewd { get; internal set; }
    //    public int TopicID { get; internal set; }
    //    public string Title { get; internal set; }
    //    public string PostUserName { get; internal set; }
    //    public DateTime PostTime { get; internal set; }
    //    public int TopicQPostCount { get; internal set; }
    //    public string LastPostUserName { get; internal set; }
    //    public string LastPostTime { get; internal set; }
    //}

    //public class ForumTopicInfo {
    //    public string CategoryName { get; internal set; }
    //    public int? CategoryID { get; internal set; }
    //    public int ID { get; internal set; }
    //    public int? ProductID { get; internal set; }
    //    public DateTime CreatedOn { get; internal set; }
    //    public bool IsClosed { get; internal set; }
    //    public bool IsApproved { get; internal set; }
    //    public int Viewd { get; internal set; }
    //}

    //public class ForumQPostInfo {
    //    public int ID { get; internal set; }
    //    public int TopicID { get; internal set; }
    //    public string Title { get; internal set; }
    //    public string Ctx { get; internal set; }
    //    public string UserName { get; internal set; }
    //    public bool IsApproved { get; internal set; }
    //    public bool IsQuestion { get; internal set; }
    //    public Decimal IP { get; internal set; }
    //    public DateTime CreatedOn { get; internal set; }
    //    public DateTime ModifiedOn { get; internal set; }
    //    public string ModifiedBy { get; internal set; }
    //    public string ShowName { get; internal set; }
    //}
}
