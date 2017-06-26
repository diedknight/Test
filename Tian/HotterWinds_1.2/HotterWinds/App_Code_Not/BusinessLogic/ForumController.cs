using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Configuration;

/// <summary>
/// Summary description for ForumController
/// </summary>
public static class IPBForumController
{
    static string MySqlConnectionString = ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;
    static string DBPrefix = ConfigurationManager.AppSettings["MySqlDBPrefix"];
    static string SqlString = "select tid, title, starter_name, FROM_UNIXTIME(last_post) as last_post from " + DBPrefix + "topics where state = 'open' order by tid desc LIMIT 5";
    
    static string ForumTopicURLFormat = ConfigurationManager.AppSettings["ForumTopicURLFormat"];

    public static List<ForumInfo> GetRecentPost()
    {
        List<ForumInfo> listForumInfo = new List<ForumInfo>(); 
        try
        {
            using (MySqlConnection conn = new MySqlConnection(MySqlConnectionString))
            using (MySqlCommand cmd = new MySqlCommand(SqlString, conn))
            {
                conn.Open();
                using (MySqlDataReader mysqlDR = cmd.ExecuteReader())
                {
                    while (mysqlDR.Read())
                    {
                        string tid = mysqlDR["tid"].ToString();
                        string title = mysqlDR["title"].ToString().Replace("&#33;", "!");
                        string starterName = mysqlDR["starter_name"].ToString();
                        DateTime dateT = DateTime.Parse(mysqlDR["last_post"].ToString());
                        ForumInfo forumInfo = new ForumInfo();
                        forumInfo.Title = title;
                        forumInfo.PosterName = starterName;
                        forumInfo.PostDateInfo = GetPostDateInfo(dateT);
                        forumInfo.Url = GetForumURL(tid, title);
                        listForumInfo.Add(forumInfo);
                    }
                }
            }
        }
        catch (Exception)
        {

        }
        return listForumInfo;
    }

    public static List<ForumInfo> GetUserPost(string user, string email, int pageIndex, int pageCount, out int postCount)
    {
        postCount = 0;
        List<ForumInfo> listForumInfo = new List<ForumInfo>();
        try
        {
            #region topics
            var sql = string.Format("select tid, title, starter_name, FROM_UNIXTIME(last_post) as last_post from pmtopics where state = 'open' and starter_id = (select member_id from pmmembers where name = '{0}' or email = '{1}') order by tid desc;"
                , user, email);
            using (MySqlConnection conn = new MySqlConnection(MySqlConnectionString))
            using (MySqlCommand cmd = new MySqlCommand(sql, conn))
            {
                conn.Open();
                using (MySqlDataReader mysqlDR = cmd.ExecuteReader())
                {
                    while (mysqlDR.Read())
                    {
                        int tid = int.Parse(mysqlDR["tid"].ToString());
                        string title = mysqlDR["title"].ToString().Replace("&#33;", "!");
                        string starterName = mysqlDR["starter_name"].ToString();
                        DateTime dateT = DateTime.Parse(DateTime.Parse(mysqlDR["last_post"].ToString()).ToString("yyyy-MM-dd HH:mm"));
                        ForumInfo forumInfo = new ForumInfo();
                        forumInfo.TID = tid;
                        forumInfo.isTopic = true;
                        forumInfo.Title = title;
                        forumInfo.PosterName = starterName;
                        forumInfo.PostDate = dateT;
                        forumInfo.PostDateInfo = GetPostDateInfo(dateT);
                        forumInfo.Url = GetForumURL(tid.ToString(), title);
                        listForumInfo.Add(forumInfo);
                    }
                }
            }
            #endregion

            #region posts
            sql = string.Format("select pid,topic_id,title,post,author_name,FROM_UNIXTIME(post_date) as postdate from pmposts p inner join pmtopics t on t.tid = p.topic_id where author_id = (select member_id from pmmembers where name = '{0}' or email = '{1}') order by pid desc;"
                , user, email);

            int count = 0;
            using (MySqlConnection conn = new MySqlConnection(MySqlConnectionString))
            using (MySqlCommand cmd = new MySqlCommand(sql, conn))
            {
                conn.Open();
                using (MySqlDataReader mysqlDR = cmd.ExecuteReader())
                {
                    while (mysqlDR.Read())
                    {
                        int tid = int.Parse(mysqlDR["topic_id"].ToString());
                        string pid = mysqlDR["pid"].ToString();
                        string title = mysqlDR["title"].ToString().Replace("&#33;", "!");
                        string post = mysqlDR["post"].ToString().Replace("&#33;", "!");
                        string starterName = mysqlDR["author_name"].ToString();
                        DateTime dateT = DateTime.Parse(DateTime.Parse(mysqlDR["postdate"].ToString()).ToString("yyyy-MM-dd HH:mm"));
                        ForumInfo forumInfo = new ForumInfo();
                        forumInfo.TID = tid;
                        forumInfo.isTopic = false;
                        forumInfo.Title = post;
                        forumInfo.PosterName = starterName;
                        forumInfo.PostDate = dateT;
                        forumInfo.PostDateInfo = GetPostDateInfo(dateT);
                        forumInfo.Url = GetForumPostURL(tid.ToString(), pid, title);
                        listForumInfo.Add(forumInfo);
                        count++;
                    }
                }
            }
            #endregion

            listForumInfo = listForumInfo.OrderByDescending(p => p.PostDate)
                .ThenByDescending(p => p.isTopic).ToList();
            postCount = count;
        }
        catch (Exception)
        {

        }
        if (listForumInfo.Count <= pageCount)
            return listForumInfo;
        else
            return listForumInfo.Skip((pageIndex - 1) * pageCount).Take(pageCount).ToList();
    }

    private static string GetForumURL(string tid, string title)
    {
        string[] aryReg0 = { "<", ">", ",", ".", ";", ";", ":", "\"", "%", "|", "\\", "[", "]", "{", "}", "(", ")", "~", "!", "@", "#", "$", "%", "^", "&", "_", "=", "+", "-", "*", "/" };
        for (int i = 0; i < aryReg0.Length; i++)
        {
            title = title.Replace(aryReg0[i], string.Empty);
        }
        return string.Format(ForumTopicURLFormat, tid, title.Replace(" ", "-"));
    }

    /// <summary>
    /// http://192.168.1.109:121/topic/1-welcome/?p=1
    /// </summary>
    /// <param name="tid"></param>
    /// <param name="pid"></param>
    /// <param name="title"></param>
    /// <returns></returns>
    private static string GetForumPostURL(string tid, string pid, string title)
    {
        return GetForumURL(tid, title) + "?p=" + pid;
    }
    
    /// <summary>
    /// post a new topic in product page forum tab
    /// </summary>
    public static string PostNewTopic(string user, string email, string ip, string topic, string post, string cate)
    {
        try
        {
            //get user id, if doesn't exsit, create new
            var uid = GetUserID(user, email, ip);
            //get forum id,if doesn't exsit, create new
            var fid = GetForumID(user, uid, cate, topic);
            //create new topic
            var tid = NewTopic(user, uid, topic, fid);
            //create new post
            var pid = NewPost(user, uid, tid, post, ip);
            //modify forum: newest_id,last_title,last_id,seo_last_title,seo_last_name,topics,posts,
            UpdateForum(fid, tid, topic, uid, user);
            //modify topic: topic_firstpost colume
            UpdateTopicFirstPost(tid, pid);
            //return topic url
            return GetForumURL(tid.ToString(), topic);
        }
        catch (Exception ex)
        {
            return ex.Message + "<br/>" + ex.Source + "<br/>" + ex.StackTrace;
        }
    }
    
    /// <summary>
    /// Check the iput topic is exsit
    /// </summary>
    public static bool CheckRepeatTopic(string topic)
    {
        try
        {
            bool flag = false;
            var sql = string.Format(@"select tid from pmtopics where title = '{0}'", topic);
            using (MySqlConnection conn = new MySqlConnection(MySqlConnectionString))
            using (MySqlCommand cmd = new MySqlCommand(sql, conn))
            {
                conn.Open();
                var obj = cmd.ExecuteScalar();
                if (int.Parse(obj.ToString()) > 0)
                    flag = true;
            }
            return flag;
        }
        catch (Exception ex)
        {
            Console.Write(ex.Message + "<br/>" + ex.Source + "<br/>" + ex.StackTrace);
            return false;
        }
    }

    /// <summary>
    /// create a new topic
    /// </summary>
    /// <param name="user"></param>
    /// <param name="uid"></param>
    /// <param name="topic"></param>
    /// <param name="cate"></param>
    /// <param name="fourm"></param>
    /// <returns></returns>
    private static int NewTopic(string user, int uid, string topic, int fourm)
    {
        var tid = 0;
        var sql = string.Format(@"insert into pmtopics(title,state,posts,starter_id,start_date,
            last_poster_id,last_post,starter_name,last_poster_name,poll_state,last_vote,views,forum_id,
            approved,author_mode,pinned,moved_to,topic_hasattach,topic_firstpost,topic_queuedposts,
            topic_open_time,topic_close_time,topic_rating_total,topic_rating_hits,title_seo,seo_last_name,
            seo_first_name,topic_deleted_posts,tdelete_time,moved_on,topic_archive_status,last_real_post,
            topic_answered_pid) values ('{0}','open',1,{1},unix_timestamp(),{1},unix_timestamp(),
            '{2}','{2}',0,0,0,{3},1,1,0,'',0,0,0,0,0,0,0,'{4}','{5}','{5}',0,0,0,0,0,0);", 
            topic, uid, user, fourm, topic.Replace(" ", "-"), user.Replace(" ", "-"));
        using (MySqlConnection conn = new MySqlConnection(MySqlConnectionString))
        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
        {
            conn.Open();
            var obj = cmd.ExecuteScalar();
            tid = GetLatestIDENTITY(conn);
        }

        return tid;
    }

    /// <summary>
    /// modify topic: topic_firstpost = pid
    /// </summary>
    /// <param name="tid"></param>
    /// <param name="pid"></param>
    private static void UpdateTopicFirstPost(int tid, int pid)
    {
        var sql = string.Format("update pmtopics set topic_firstpost = {0} where tid = {1}",
            pid, tid);
        using (MySqlConnection conn = new MySqlConnection(MySqlConnectionString))
        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
        {
            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }

    /// <summary>
    /// create a new post
    /// </summary>
    /// <param name="user"></param>
    /// <param name="topic"></param>
    /// <param name="post"></param>
    /// <param name="cate"></param>
    /// <returns></returns>
    private static int NewPost(string user, int uid, int tid, string post, string ip)
    {
        var pid = 0;
        var sql = string.Format(@"insert into pmposts (append_edit,author_id,author_name,use_sig,
            use_emo,ip_address,post_date,post,queued,topic_id,new_topic,post_key,post_htmlstate,
            post_edit_reason,post_bwoptions,pdelete_time,post_field_int) 
            values (0,{0},'{1}',1,1,'{2}',unix_timestamp(),'{3}',0,{4},1,'{5}',0,'',0,0,0);",
            uid, user, ip, post, tid, Guid.NewGuid().ToString().Replace("-", ""));
        using (MySqlConnection conn = new MySqlConnection(MySqlConnectionString))
        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
        {
            conn.Open();
            var obj = cmd.ExecuteScalar();
            pid = GetLatestIDENTITY(conn);
        }
        return pid;
    }

    /// <summary>
    /// 根据user name 获取member id
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    private static int GetUserID(string user, string email, string ip)
    {
        int uid = 0;
        var sql = string.Format("select member_id from pmmembers where name = '{0}' "+
            "or email = '{1}' limit 1", user, email);
        using (MySqlConnection conn = new MySqlConnection(MySqlConnectionString))
        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
        {
            conn.Open();
            using (MySqlDataReader mysqlDR = cmd.ExecuteReader())
            {
                //如果还没有这个用户,则创建新的
                if (mysqlDR.Read())
                {
                    uid = int.Parse(mysqlDR[0].ToString());
                }
                else
                {
                    mysqlDR.Close();
                    uid = CreateNewMember(user, email, ip);
                }
            }
        }
        return uid;
    }

    /// <summary>
    /// 根据user name 创建新的member,
    /// 并返回id
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    private static int CreateNewMember(string user, string email, string ip)
    {
        int uid = 0;

        var sql = string.Format(@"insert into pmmembers (name,member_group_id,email,joined,ip_address,
            warn_lastwarn,restrict_post,msg_count_new,msg_count_total,msg_count_reset,msg_show_notification,
            mod_posts,login_anonymous,mgroup_others,org_perm_id,member_login_key,member_login_key_expire,
            has_gallery,members_auto_dst,members_display_name,members_seo_name,members_created_remote,
            members_disable_pm,members_l_display_name,members_l_username,failed_login_count,
            members_profile_views,members_pass_hash,members_pass_salt,member_banned,member_uploader,
            members_bitoptions,fb_uid,fb_emailhash,fb_lastsync,members_day_posts,twitter_id,twitter_token,
            twitter_secret,notification_cnt,tc_lastsync,fb_session,ipsconnect_id) values 
            ('{0}',3,'{1}',unix_timestamp(),'{2}',0,0,0,0,0,1,0,'0&0','','','{3}',unix_timestamp(),
            0,1,'{0}','{4}',0,0,'{0}','{0}',0,0,'','',0,'flash',0,0,'',0,'','','','',0,0,'',0);",
            user, email, ip, Guid.NewGuid().ToString().Replace("-", ""), user.Replace(" ", "-"));
        using (MySqlConnection conn = new MySqlConnection(MySqlConnectionString))
        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
        {
            conn.Open();
            var obj = cmd.ExecuteScalar();
            uid = GetLatestIDENTITY(conn);
        }
        return uid;
    }

    /// <summary>
    /// 根据category name 获取forum id
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    private static int GetForumID(string user, int uid, string category, string topic)
    {
        int fid = 0;//General forum
        var sql = "select id from pmforums where name like '%" + category + "%' and (";
        category = category.Replace("&", "").Replace("  ", " ");
        var cates = category.Split(' ');
        foreach (var item in cates)
        {
            sql += "name like '%" + item + "%' or ";
        }
        if (sql.EndsWith(" or "))
        {
            sql = sql.Substring(0, sql.Length - " or ".Length);
        }
        sql += ") limit 1;";
        using (MySqlConnection conn = new MySqlConnection(MySqlConnectionString))
        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
        {
            conn.Open();
            using (MySqlDataReader mysqlDR = cmd.ExecuteReader())
            {
                if (mysqlDR.Read())
                {
                    fid = int.Parse(mysqlDR[0].ToString());
                }
            }

            if (fid <= 0)
            {
                cmd.CommandText = "select id from pmforums where name like '%General forum%';";
                using (MySqlDataReader mysqlDR = cmd.ExecuteReader())
                {
                    if (mysqlDR.Read())
                    {
                        fid = int.Parse(mysqlDR[0].ToString());
                    }
                }
            }
        }
        return fid;
    }

    /// <summary>
    /// modify forum colume: 
    /// newest_id,last_title,last_id,seo_last_title,seo_last_name,topics,posts,
    /// </summary>
    /// <param name="category"></param>
    /// <returns></returns>
    private static void UpdateForum(int fid, int tid, string topic, int uid, string user)
    {
        var sql = string.Format("update pmforums set last_title = '{0}'," +
            "last_id = {1},seo_last_title = '{2}',seo_last_name = '{3}'" +
            ",topics = topics+1,posts = posts+1 where id = {4};update pmmembers set posts=posts+1 where member_id={5}",
            topic, tid, topic.Replace(" ", "-"), user.Replace(" ", "-"), fid, uid);
        using (MySqlConnection conn = new MySqlConnection(MySqlConnectionString))
        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
        {
            conn.Open();
            var obj = cmd.ExecuteNonQuery();
        }
    }

    private static int GetLatestIDENTITY(MySqlConnection conn)
    {
        var id = 0;
        var sql = "select @@IDENTITY;";
        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
        {
            var obj = cmd.ExecuteReader();
            if (obj.Read())
                id = int.Parse(obj[0].ToString());
            obj.Close();
        }
        return id;
    }

    private static string GetPostDateInfo(DateTime dt)
    {
        TimeSpan ts = DateTime.Now - dt;
        //DateTime.Now.Month
        if (ts.Days > 60)
        {
            return ts.Days / 30 + " months ago";
        }
        if (ts.Days > 30)
        {
            return "1 month ago";
        }
        if (ts.Days > 1)
        {
            return ts.Days + " days ago";
        }
        else if (ts.Days == 1)
        {
            return "1 day ago";
        }
        else if (ts.Hours > 1)
        {
            return ts.Hours + " hours ago";
        }
        else if (ts.Hours == 1)
        {
            return "1 hour ago";
        }
        else if (ts.Minutes > 1)
        {
            return ts.Minutes + " minutes ago";
        }
        else if (ts.Minutes <= 1)
        {
            return "1 minute ago";
        }
        return "";
    }
}