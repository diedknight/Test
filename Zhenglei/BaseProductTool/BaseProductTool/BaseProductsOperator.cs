using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace BaseProductTool
{
    public class BaseProductsOperator
    {
        int mMaxCount;
        List<IntraLinkingGenerationAndRelated> mIntraLinkingGenerationAndRelatedList;
        int mInterval;
        string mConnStr;
        LogWriter mLogWriter;

        public BaseProductsOperator(int maxCount, int interval, string connStr, LogWriter logWriter)
        {
            mMaxCount = maxCount;
            mInterval = interval;
            mIntraLinkingGenerationAndRelatedList = new List<IntraLinkingGenerationAndRelated>();
            mConnStr = connStr;
            mLogWriter = logWriter;
        }

        public void Add(List<IntraLinkingGenerationAndRelated> relatedProductScoreList)
        {
            mIntraLinkingGenerationAndRelatedList.AddRange(relatedProductScoreList);

            //if (mIntraLinkingGenerationAndRelatedList.Count >= mMaxCount)
            //{
            //    SaveToDB();
            //}
        }

        private void SaveToDB()
        {
            try
            {
                WriteToDB(mIntraLinkingGenerationAndRelatedList, mConnStr);
                if (mInterval > 0)
                {
                    System.Threading.Thread.Sleep(mInterval);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\t" + ex.StackTrace);
                mLogWriter.WriteLine(ex.Message + "\t" + ex.StackTrace);
            }

            mIntraLinkingGenerationAndRelatedList = new List<IntraLinkingGenerationAndRelated>();
        }

        public void Finish()
        {
            if (mIntraLinkingGenerationAndRelatedList.Count > 0)
            {
                SaveToDB();
            }
        }

        public static void WriteToDB(List<IntraLinkingGenerationAndRelated> list, string connStr)
        {
            foreach (var info in list)
            {

                using (SqlConnection sqlConn = new SqlConnection(connStr))
                {
                    var id = sqlConn.ExecuteScalar<int>("select top 1 ID from IntraLinkingGenerationAndRelated where ProductID=@ProductID and LinedPID=@LinedPID and LinkType='Variant'", new { ProductID = info.ProductId, LinedPID = info.LinedPID });

                    if (id > 0)
                    {
                        string updateStr = @"UPDATE [IntraLinkingGenerationAndRelated]
                                           SET [ProductID] = @ProductID
                                              ,[BaseProductValue] = @BaseProductValue
                                              ,[LinkType] = @LinkType
                                              ,[ShowType] = @ShowType
                                              ,[LinedPID] = @LinedPID
                                              ,[VariantProductValue] = @VariantProductValue
                                              ,[VariableNameOfPID] = @VariableNameOfPID
                                              ,[LinedPname] = @LinedPname
                                              ,[VariableNameOfPN] = @VariableNameOfPN
                                              ,[AttributeName] = @AttributeName
                                              ,[VariableNameOfAN] = @VariableNameOfAN
                                              ,[AttributeURL] = @AttributeURL
                                              ,[VariableNameOfAURL] = @VariableNameOfAURL
                                              ,[Text] = @Text
                                              ,[VariantTypeID] = @VariantTypeID                                              
                                              ,[ModifiedBy] = @ModifiedBy
                                              ,[ModifiedOn] = @ModifiedOn
                                         WHERE ID=@ID";

                        sqlConn.Execute(updateStr, info);
                    }
                    else
                    {
                        string insertStr = @" INSERT  INTO [IntraLinkingGenerationAndRelated]
                                           ([ProductID]
                                           ,[BaseProductValue]
                                           ,[LinkType]
                                           ,[ShowType]
                                           ,[LinedPID]
                                           ,[VariantProductValue]
                                           ,[VariableNameOfPID]
                                           ,[LinedPname]
                                           ,[VariableNameOfPN]
                                           ,[AttributeName]
                                           ,[VariableNameOfAN]
                                           ,[AttributeURL]
                                           ,[VariableNameOfAURL]
                                           ,[Text]
                                           ,[VariantTypeID]
                                           ,[CreatedBy]
                                           ,[CreatedOn]
                                           ,[ModifiedBy]
                                           ,[ModifiedOn])
                                      VALUES
                                           (@ProductID
                                           , @BaseProductValue
                                           , @LinkType
                                           , @ShowType
                                           , @LinedPID
                                           , @VariantProductValue
                                           , @VariableNameOfPID
                                           , @LinedPname
                                           , @VariableNameOfPN
                                           , @AttributeName
                                           , @VariableNameOfAN
                                           , @AttributeURL
                                           , @VariableNameOfAURL
                                           , @Text
                                           , @VariantTypeID
                                           , @CreatedBy
                                           , @CreatedOn
                                           , @ModifiedBy
                                           , @ModifiedOn)";

                        sqlConn.Execute(insertStr, info);

                    }
                }
            }

            //DateTime dtNow = DateTime.Now;
            //string pIdsStr = string.Join(",", list.Select(p => p.ProductId).Distinct());
            ////string deleteSql = @"delete IntraLinkingGenerationAndRelated where ShowType = '1' and ProductId in (" + pIdsStr + ")";
            //string deleteSql = @"delete IntraLinkingGenerationAndRelated where ShowType = '1' and LinkType = 'Variant'";

            //string insertSql = @"INSERT INTO IntraLinkingGenerationAndRelated
            //                   (ProductID
            //                   ,BaseProductValue
            //                   ,LinkType
            //                   ,ShowType
            //                   ,LinedPID
            //                   ,VariantProductValue
            //                   ,VariableNameOfPID
            //                   ,LinedPname
            //                   ,VariableNameOfPN
            //                   ,AttributeName
            //                   ,VariableNameOfAN
            //                   ,AttributeURL
            //                   ,VariableNameOfAURL
            //                   ,Text
            //                   ,VariantTypeID
            //                   ,CreatedBy
            //                   ,CreatedOn
            //                   ,ModifiedBy
            //                   ,ModifiedOn)";
            //foreach (var prs in list)
            //{
            //    insertSql += prs.ToSqlString(dtNow) + " union all ";
            //}

            //if (insertSql.EndsWith(" union all "))
            //{
            //    insertSql = insertSql.Substring(0, insertSql.Length - " union all ".Length);
            //}

            //string sql = "BEGIN TRANSACTION ";
            //sql += @"
            //        " + deleteSql + " ";
            //sql += @"
            //        IF @@ERROR>0 GOTO TRANS_ERR ";
            //sql += @"
            //        " + insertSql + " ";
            //sql += @"
            //        IF @@ERROR>0 GOTO TRANS_ERR ";
            //sql += @"
            //        COMMIT TRANSACTION 
            //        Return
            //        TRANS_ERR:
	           //         ROLLBACK TRANSACTION";

            //using (SqlConnection sqlConn = new SqlConnection(connStr))
            //{
            //    sqlConn.Open();
            //    using (SqlCommand sqlCmd1 = new SqlCommand(sql, sqlConn))
            //    {
            //        sqlCmd1.CommandTimeout = 0;
            //        sqlCmd1.ExecuteNonQuery();
            //    }
            //}
        }
    }
}
