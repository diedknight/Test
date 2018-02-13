using CoverInsuranceReport.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoverInsuranceReport
{
    public class CateCtrl
    {
        private List<CategoryData> _dataList = null;

        public CateCtrl()
        {
            this._dataList = new List<CategoryData>();

            string str = System.Configuration.ConfigurationManager.AppSettings["categoryID"];
            str.Split(';').ToList().ForEach(item => {
                var tempVal = item.Split(',');
                var data = new CategoryData();
                data.CId = Convert.ToInt32(tempVal[0].Trim());
                data.CategoryName = tempVal[1].Trim();

                this._dataList.Add(data);
            });
        }

        public List<CategoryData> GetData()
        {
            return this._dataList;
        }

    }
}
