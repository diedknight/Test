using CoverInsuranceReport.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Priceme.Infrastructure.Excel;

namespace CoverInsuranceReport
{
    public class ExcelCtrl
    {
        public ExcelSimpleHelper _hepler = null;
        public List<ExcelData> _dataList = null;
        private int _lastRowIndex = 0;

        public ExcelCtrl(int sheetIndex = 0)
        {
            this._dataList = new List<ExcelData>();
            this._hepler = new ExcelSimpleHelper();
            this._hepler.Read(System.Configuration.ConfigurationManager.AppSettings["filePath"]);
            this._hepler.ReadSheetAt(sheetIndex);
            this._hepler.CurIndex = 1;

            while (true)
            {
                var row = this._hepler.ReadLine();
                if (row == null) break;
                if (row.Count == 0) continue;

                ExcelData data = new ExcelData();
                data.RowIndex = this._hepler.CurIndex - 1;
                data.PId = row.Count > 0 ? Convert.ToInt32(row[0]) : 0;
                data.CategoryName = row.Count > 1 ? row[1] : "";
                data.Manufacturer = row.Count > 2 ? row[2] : "";
                data.ProductFamily = row.Count > 3 ? row[3] : "";
                data.Series = row.Count > 4 ? row[4] : "";
                data.Model = row.Count > 5 ? row[5] : "";
                data.ProductName = row.Count > 6 ? row[6] : "";
                data.MinPrice = row.Count > 7 ? Convert.ToDecimal(row[7]) : 0;
                data.AveragePrice = row.Count > 8 ? Convert.ToDecimal(row[8]) : 0;
                data.MedianPrice = row.Count > 9 ? Convert.ToDecimal(row[9]) : 0;
                data.MaxPrice = row.Count > 10 ? Convert.ToDecimal(row[10]) : 0;
                data.NumberOfPrices = row.Count > 11 ? Convert.ToInt32(row[11]) : 0;
                data.ProductImageUrl = row.Count > 12 ? row[12] : "";
                data.Upcoming = row.Count > 13 ? row[13] : "";
                data.CreatedOn = row.Count > 14 ? row[14] : "";
                data.UpdateOn = row.Count > 15 ? row[15] : "";
                data.ForSale = row.Count > 16 ? row[16] : "";

                this._dataList.Add(data);
            }

            this._lastRowIndex = this._hepler.CurIndex;
        }

        public List<ExcelData> GetData()
        {
            return this._dataList;
        }        

        public void UpdateCell(int rowIndex, int colIndex, string data, bool isRed = false, bool isWeight = false)
        {
            this._hepler.CurIndex = rowIndex;
            this._hepler.UpdateCell(isWeight, isRed, colIndex, data);
        }

        public void WriteNewLine(ExcelData data)
        {
            this._hepler.CurIndex = this._lastRowIndex;
            this._hepler.WriteLine(false, true,
                data.PId.ToString(),
                data.CategoryName,
                data.Manufacturer,
                data.ProductFamily,
                data.Series,
                data.Model,
                data.ProductName,
                data.MinPrice.ToString("0.00"),
                data.AveragePrice.ToString("0.00"),
                data.MaxPrice.ToString("0.00"),
                data.NumberOfPrices.ToString(),
                data.ProductImageUrl,
                data.Upcoming,
                data.CreatedOn,
                data.UpdateOn,
                data.ForSale
                );

            this._lastRowIndex = this._hepler.CurIndex;
        }

        public void RemoveRow(int rowIndex)
        {
            this._hepler.CurIndex = rowIndex;
            this._hepler.RemoveLine();
        }

    }
}
