using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;

namespace FisherPaykelTool.Model
{
    public class AttrInfo
    {
        //private bool _isInit = false;

        //public AttrInfo(TypeAttr type, int Id)
        //{
        //    this.TypeAttr = type;
        //    this.Id = Id;


        //}

        public TypeAttr TypeAttr { get; set; }
        public int Id { get; set; }        
        public string Name { get; set; }
        public string Value { get; set; }                            
        public string Unit { get; set; }

        public int ProductId { get; set; }
        //private HashSet<int> ProductIds { get; set; }

        public override string ToString()
        {
            return this.Value + " " + this.Unit;
        }


        public static List<AttrInfo> GetAttr(TypeAttr type, int Id)
        {
            string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["Priceme"].ConnectionString;
            string sql = "";

            switch (type)
            {
                case TypeAttr.Compare:
                    sql = " select A.ProductId, A.CompareAttributeID as Id, Name, comparevalue as [Value], Unit"
                        + " from Store_Compare_Attribute_Map A"
                        + " inner join Store_Compare_Attributes T on T.CompareAttributeID = A.CompareAttributeID"
                        + " where A.CompareAttributeID = @Id";
                    break;
                case TypeAttr.General:
                    sql = " select ProductId, TT.typeid as Id, TT.title as Name, V.value as [Value], Unit from CSK_Store_AttributeValue V"
                        + " inner join"
                        + " ("
                        + " select ProductID,PD.typeid,T.title,pd.attributevalueid,unit from CSK_Store_ProductDescriptor PD"
                        + " inner join CSK_Store_ProductDescriptorTitle T on PD.typeid=T.typeid"
                        + " where T.typeid=@Id"
                        + " ) TT"
                        + " on V.attributevalueid=TT.attributevalueid";
                    break;
            }

            using (SqlConnection con = new SqlConnection(conStr))
            {
                var list = con.Query<AttrInfo>(sql, new { Id = Id }).ToList();

                return list;
            }
        }        

    }
}
