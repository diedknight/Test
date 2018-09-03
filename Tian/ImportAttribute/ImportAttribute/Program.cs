using PriceMeDBA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;

namespace ImportAttribute
{
    class Program
    {
        static void Main(string[] args)
        {
            var datas = ExcelData.Load();

            //add attribute group
            foreach (var data in datas)
            {
                var attrGroup = CSK_Store_AttributeGroup.SingleOrDefault(item => item.Categoryid == Convert.ToInt32(data.Cid) && item.AttributeGroupName == data.AttributeGroup);
                if (attrGroup == null)
                {
                    var attr = new CSK_Store_AttributeGroup();
                    attr.Categoryid = Convert.ToInt32(data.Cid);
                    attr.AttributeGroupName = data.AttributeGroup;

                    using (var con = GetConnection())
                    {
                        int orderId = con.ExecuteScalar<int>("select top 1 OrderID from CSK_Store_AttributeGroup where Categoryid=" + attr.Categoryid + " order by OrderID desc");
                        attr.OrderID = orderId + 1;
                    }

                    attr.Save();
                }
            }

            //general attr
            datas.Where(item => item.AttributeType.Contains("General attr")).ToList()
                .ForEach(data =>
                {
                    using (var con = GetConnection())
                    {
                        string sql = "select top 1 TypeID from CSK_Store_ProductDescriptorTitle where title='" + data.PriceMeAttributeName + "' and typeid in (select attributetitleid from CSK_Store_Category_AttributeTitle_Map where CategoryID=" + Convert.ToInt32(data.Cid) + " ) ";

                        int typeId = con.ExecuteScalar<int>(sql);
                        if (typeId != 0) return;
                    }                    

                    CSK_Store_ProductDescriptorTitle attrTitle = new CSK_Store_ProductDescriptorTitle();
                    attrTitle.Title = data.PriceMeAttributeName;
                    attrTitle.IsNumeric = (data.ValueType == "Numeric").ToString();
                    attrTitle.Unit = data.PriceMeAttributeUnit;
                    attrTitle.ShortDescription = data.Description;

                    using (var con = GetConnection())
                    {
                        int groupId = con.ExecuteScalar<int>("select top 1 AttributeGroupID from CSK_Store_AttributeGroup where Categoryid=" + data.Cid + " and AttributeGroupName='" + data.AttributeGroup + "'");
                        attrTitle.AttributeGroupID = groupId;

                        int valueTypeId = con.ExecuteScalar<int>("select top 1 AttributeTypeID from CSK_Store_AttributeType where TypeName='" + data.ValueType + "'");
                        attrTitle.AttributeTypeID = valueTypeId;
                    }

                    attrTitle.CatelogAttributes = false;
                    attrTitle.Save();

                    CSK_Store_Category_AttributeTitle_Map map = new CSK_Store_Category_AttributeTitle_Map();
                    map.CategoryID = Convert.ToInt32(data.Cid);
                    map.AttributeTitleID = attrTitle.TypeID;
                    map.IsPrimary = false;

                    using (var con = GetConnection())
                    {
                        int order = con.ExecuteScalar<int>("select top 1 AttributeOrder from CSK_Store_Category_AttributeTitle_Map where CategoryID=" + data.Cid + " order by AttributeOrder desc");
                        map.AttributeOrder = order + 1;
                    }
                    map.IsSlider = false;

                    map.Save();

                    if (!string.IsNullOrEmpty(data.DefinedValues))
                    {
                        data.DefinedValues.Split(';').ToList().ForEach(definedValue =>
                        {
                            CSK_Store_AttributeValue attrVal = new CSK_Store_AttributeValue();
                            attrVal.AttributeTitleID = attrTitle.TypeID;
                            attrVal.Value = definedValue;
                            attrVal.PopularAttribute = true;

                            attrVal.Save();
                        });
                    }
                });

            //compare attr
            datas.Where(item => item.AttributeType.Contains("Compare attr")).ToList()
                .ForEach(data =>
                {
                    var tempAttr = Store_Compare_Attribute.SingleOrDefault(item => item.CategoryID == Convert.ToInt32(data.Cid) && item.Name == data.PriceMeAttributeName);

                    if (tempAttr != null) return;

                    Store_Compare_Attribute comAttr = new Store_Compare_Attribute();
                    comAttr.CategoryID = Convert.ToInt32(data.Cid);
                    comAttr.Name = data.PriceMeAttributeName;
                    comAttr.IsNumeric = data.ValueType == "Numeric";
                    comAttr.Unit = data.PriceMeAttributeUnit;
                    comAttr.ShortDescription = data.Description;

                    using (var con = GetConnection())
                    {
                        int groupId = con.ExecuteScalar<int>("select top 1 AttributeGroupID from CSK_Store_AttributeGroup where Categoryid=" + data.Cid + " and AttributeGroupName='" + data.AttributeGroup + "'");
                        comAttr.AttributeGroupID = groupId;

                        int valueTypeId = con.ExecuteScalar<int>("select top 1 AttributeTypeID from CSK_Store_AttributeType where TypeName='" + data.ValueType + "'");
                        comAttr.AttributeTypeID = valueTypeId;
                    }

                    comAttr.Save();
                    comAttr = Store_Compare_Attribute.SingleOrDefault(item => item.ID == comAttr.ID);
                    comAttr.CompareAttributeID = comAttr.ID;
                    comAttr.Save();

                    if (!string.IsNullOrEmpty(data.DefinedValues))
                    {
                        using (var con = GetConnection())
                        {
                            data.DefinedValues.Split(';').ToList().ForEach(definedValue =>
                            {
                                string sql = "INSERT INTO [AT_CompareAttributeValue_Map] ([CompareAttributeID],[Value],[CategoryID]) VALUES (@CompareAttributeID,@Value,@CategoryID)";

                                con.Execute(sql, new { CompareAttributeID = comAttr.CompareAttributeID, Value = definedValue, CategoryID = comAttr.CategoryID });
                            });                            
                        }
                    }

                });


            //add attr map
            datas.Where(item => !string.IsNullOrEmpty(item.DSAttributeName)).ToList()
                .ForEach(data =>
                {
                    using (var con = GetConnection())
                    {
                        int RetailerId = Convert.ToInt32(data.RetailerID);
                        int CategoryId = Convert.ToInt32(data.Cid);
                        string RetailerAttributeName = data.DSAttributeName;
                        string Unit = data.DSAttributeUnit;
                        int PM_AttributeID = 0;
                        int AttributeType = 0;

                        if (data.AttributeType.Contains("General attr"))
                        {
                            string sql1 = "select top 1 AttributeTitleID from CSK_Store_Category_AttributeTitle_Map where categoryid=" + CategoryId + " and AttributeTitleID in (select typeid from CSK_Store_ProductDescriptorTitle where title='" + data.PriceMeAttributeName + "')";
                            PM_AttributeID = con.ExecuteScalar<int>(sql1);

                            AttributeType = 2;
                        }

                        if (data.AttributeType.Contains("Compare attr"))
                        {
                            string sql2 = "select top 1 CompareAttributeID from Store_Compare_Attributes where CategoryID=" + CategoryId + " and Name='" + data.PriceMeAttributeName + "'";
                            PM_AttributeID = con.ExecuteScalar<int>(sql2);

                            AttributeType = 3;
                        }

                        var tempAttrMapId = con.ExecuteScalar<int>("select top 1 attributeretailerMapId from CSK_Store_AttributeRetailerMap where CategoryId=" + CategoryId + " and RetailerId=" + RetailerId + " and PM_AttributeID=" + PM_AttributeID);
                        if (tempAttrMapId != 0) return;

                        string sql = "INSERT INTO [CSK_Store_AttributeRetailerMap]([RetailerId],[CategoryId],[RetailerAttributeName],[Unit],[PM_AttributeID],[AttributeType])VALUES(@RetailerId,@CategoryId,@RetailerAttributeName,@Unit,@PM_AttributeID,@AttributeType)";

                        con.Execute(sql, new { RetailerId = RetailerId, CategoryId = CategoryId, RetailerAttributeName = RetailerAttributeName, Unit = Unit, PM_AttributeID = PM_AttributeID, AttributeType = AttributeType });
                    }
                });
        }


        public static SqlConnection GetConnection()
        {
            return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate"].ConnectionString);
        }

    }
}
