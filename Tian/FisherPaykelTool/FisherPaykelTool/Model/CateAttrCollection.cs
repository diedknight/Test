using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FisherPaykelTool.Model
{
    public class CateAttrCollection
    {
        private List<CateAttr> _cateAttrList = null;

        private CateAttrCollection()
        {
            this._cateAttrList = new List<CateAttr>();
        }

        public CateAttr GetCateAttr(int cateId)
        {
            return this._cateAttrList.SingleOrDefault(item => item.CategoryId == cateId);
        }

        public List<int> GetCateIds()
        {
            return this._cateAttrList.Select(item => item.CategoryId).ToList();
        }

        private static CateAttrCollection _collection = null;
        private static readonly object _obj = new object();

        public static CateAttrCollection Load()
        {
            if (_collection == null)
            {
                lock (_obj)
                {
                    if (_collection == null)
                    {
                        _collection = new CateAttrCollection();

                        string fileName = System.Configuration.ConfigurationManager.AppSettings["excelFilePath"];

                        IWorkbook workbook = WorkbookFactory.Create(fileName);
                        ISheet sheet = workbook.GetSheet("Sheet1");

                        for (int i = 1; i <= sheet.LastRowNum; i++)
                        {
                            var row = sheet.GetRow(i);

                            string cateId = row.GetCell(0).ToString();
                            if (string.IsNullOrWhiteSpace(cateId)) continue;

                            CateAttr cateAttr = new CateAttr();
                            cateAttr.CategoryId = Convert.ToInt32(cateId.Trim());
                            cateAttr.Size_Capacity = ParserAttr(row.GetCell(1).ToString(), row.GetCell(2).ToString());
                            cateAttr.Type_Functions = ParserAttr(row.GetCell(3).ToString(), row.GetCell(4).ToString());
                            cateAttr.Finish = ParserAttr(row.GetCell(5).ToString(), row.GetCell(6).ToString());
                            cateAttr.Energy_Water_Rating = ParserAttr(row.GetCell(7).ToString(), row.GetCell(8).ToString());

                            _collection._cateAttrList.Add(cateAttr);
                        }

                        workbook.Close();                        
                    }
                }
            }

            return _collection;
        }

        private static List<AttrInfo> ParserAttr(string typesStr, string IdsStr)
        {
            List<AttrInfo> list = new List<AttrInfo>();

            if (string.IsNullOrWhiteSpace(typesStr) || string.IsNullOrWhiteSpace(IdsStr)) return list;

            var types = typesStr.Split('/');
            var Ids = IdsStr.Split('/');

            int count = types.Length > Ids.Length ? Ids.Length : types.Length;

            for (int i = 0; i < count; i++)
            {
                string type = types[i].ToLower().Trim();
                int id = Convert.ToInt32(Ids[i].Trim());

                if (type == "compare") list.AddRange(AttrInfo.GetAttr(TypeAttr.Compare, id));
                if (type == "general") list.AddRange(AttrInfo.GetAttr(TypeAttr.General, id));                
            }

            return list;
        }

    }
}
