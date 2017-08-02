using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FisherPaykelTool.Excel
{
    public class ExcelSimpleHelper
    {
        private IWorkbook _workbook = null;
        private ISheet _curSheet = null;
        private IRow _curRow = null;

        private ICellStyle _boldStyle = null;
        private IFont _boldFont = null;

        private ICellStyle _normalStyle = null;
        private IFont _normalFont = null;

        private string _userId = "";
        private string _targetIP = "";
        private string _password = "";

        private void Init()
        {            
            this._workbook = new HSSFWorkbook();
            this._workbook.CreateSheet("Sheet1");
            this._workbook.CreateSheet("Sheet2");
            this._workbook.CreateSheet("Sheet3");
            this._workbook.CreateSheet("Sheet4");

            this._curSheet = this._workbook.GetSheetAt(0);
            this._curRow = this._curSheet.CreateRow(0);

            this._boldFont = this._workbook.CreateFont();
            this._boldFont.Boldweight = (short)FontBoldWeight.Bold;
            this._boldStyle = this._workbook.CreateCellStyle();
            this._boldStyle.SetFont(this._boldFont);

            this._normalFont = this._workbook.CreateFont();
            this._normalFont.Boldweight = (short)FontBoldWeight.Normal;
            this._normalStyle = this._workbook.CreateCellStyle();
            this._normalStyle.SetFont(this._normalFont);
        }

        public ExcelSimpleHelper()
        {

        }

        public void WriteLine(params string[] dataList)
        {
            this.WriteLine(false, dataList);
        }

        public void WriteLine(bool bold, params string[] dataList)
        {            
            //init
            if (this._workbook == null)
            {
                this.Init();
            }

            for (int i = 0; i < dataList.Length; i++)
            {
                var cell = this._curRow.CreateCell(i);
                
                cell.SetCellValue(dataList[i]);
                

                if (bold)
                {
                    cell.CellStyle = this._boldStyle;

                    //cell.CellStyle.SetFont(this._boldFont);
                }
                else
                {
                    cell.CellStyle = this._normalStyle;
                    
                    //cell.CellStyle.SetFont(this._normalFont);
                }
            }
            
            //下一行
            this._curRow = this._curSheet.CreateRow(this._curRow.RowNum + 1);
        }

        public byte[] Save(string fileName)
        {
            byte[] bt = null;

            if (!Directory.Exists(Path.GetDirectoryName(fileName)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fileName));
            }

            using (FileStream file = new FileStream(fileName, FileMode.Create))
            {
                this._workbook.Write(file);

                bt = new byte[file.Length];

                file.Seek(0, SeekOrigin.Begin);
                file.Read(bt, 0, bt.Length);
            }

            return bt;
        }

        public byte[] Save()
        {
            byte[] bt = null;

            using (MemoryStream ms = new MemoryStream())
            {
                this._workbook.Write(ms);

                bt = new byte[ms.Length];

                ms.Seek(0, SeekOrigin.Begin);
                ms.Read(bt, 0, bt.Length);
            }

            return bt;
        }

    }
}
