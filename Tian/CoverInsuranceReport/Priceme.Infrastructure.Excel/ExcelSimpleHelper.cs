using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Priceme.Infrastructure.Excel
{
    public class ExcelSimpleHelper
    {
        private IWorkbook _workbook = null;
        private ISheet _curSheet = null;
        private IRow _curRow = null;

        private int _curIndex = 0;

        public int CurIndex
        {
            get { return this._curIndex; }
            set
            {
                this._curIndex = value;
                this._curRow = this._curSheet.GetRow(this._curIndex);
            }
        }

        private void Init()
        {
            this._workbook = new HSSFWorkbook();
            this._workbook.CreateSheet("Sheet1");
            this._workbook.CreateSheet("Sheet2");
            this._workbook.CreateSheet("Sheet3");
            this._workbook.CreateSheet("Sheet4");

            this._curSheet = this._workbook.GetSheetAt(0);
            this._curRow = this._curSheet.CreateRow(0);
        }

        public ExcelSimpleHelper()
        {

        }

        //public void InsertCol(int colIndex, string data)
        //{
        //    this.InsertCol(false, false, colIndex, data);
        //}

        //public void InsertCol(bool isWeight, bool isRed, int colIndex, string data)
        //{

        //}        

        public void UpdateCell(int colIndex, string data)
        {
            this.UpdateCell(false, false, colIndex, data);
        }

        public void UpdateCell(bool isWeight, bool isRed, int colIndex, string data)
        {
            //init
            if (this._workbook == null)
            {
                this.Init();
            }

            if (this._curRow == null)
            {
                this._curRow = this._curSheet.CreateRow(this._curIndex);
            }

            var cell = this._curRow.GetCell(colIndex);
            if (cell == null) cell = this._curRow.CreateCell(colIndex);

            if (isRed || isWeight)
            {
                var cellStyle = this._workbook.CreateCellStyle();
                var font = this._workbook.CreateFont();

                if (isRed) font.Color = NPOI.SS.UserModel.IndexedColors.Red.Index;
                if (isWeight) font.IsBold = true;

                cellStyle.SetFont(font);
                cell.CellStyle = cellStyle;
            }

            cell.SetCellValue(data);
        }

        public void WriteLine(params string[] dataList)
        {
            this.WriteLine(false, false, dataList);
        }

        public void WriteLine(bool isWeight, bool isRed, params string[] dataList)
        {
            //init
            if (this._workbook == null)
            {
                this.Init();
            }

            if (this._curRow == null)
            {
                this._curRow = this._curSheet.CreateRow(this._curIndex);
            }

            for (int i = 0; i < dataList.Length; i++)
            {
                var cell = this._curRow.CreateCell(i);

                if (isRed || isWeight)
                {

                    var cellStyle = this._workbook.CreateCellStyle();
                    var font = this._workbook.CreateFont();

                    if (isRed) font.Color = NPOI.SS.UserModel.IndexedColors.Red.Index;
                    if (isWeight) font.IsBold = true;


                    cellStyle.SetFont(font);
                    cell.CellStyle = cellStyle;
                }

                cell.SetCellValue(dataList[i]);
            }

            //下一行
            //this._curRow = this._curSheet.CreateRow(this._curRow.RowNum + 1);

            this._curIndex++;
            this._curRow = null;
        }

        public List<string> ReadLine(int startColIndex, int length)
        {
            List<string> list = new List<string>();

            if (this._curIndex > this._curSheet.LastRowNum) return null;

            for (int i = startColIndex; i < length; i++)
            {
                if (this._curRow == null || this._curRow.GetCell(i) == null)
                {
                    list.Add("");
                }
                else
                {
                    list.Add(this._curRow.GetCell(i).ToString());
                }
            }

            this._curIndex++;
            this._curRow = this._curSheet.GetRow(this._curIndex);

            return list;
        }

        public List<string> ReadLine()
        {
            List<string> list = new List<string>();

            if (this._curIndex > this._curSheet.LastRowNum) return null;

            if (this._curRow != null)
            {

                short minColIx = this._curRow.FirstCellNum;
                short maxColIx = this._curRow.LastCellNum;

                for (short colIx = minColIx; colIx < maxColIx; colIx++)
                {
                    if (this._curRow == null || this._curRow.GetCell(colIx) == null)
                    {
                        list.Add("");
                    }
                    else
                    {
                        list.Add(this._curRow.GetCell(colIx).ToString());
                    }
                }
            }

            this._curIndex++;
            this._curRow = this._curSheet.GetRow(this._curIndex);

            return list;
        }

        public void RemoveLine()
        {
            if (this._curRow != null)
            {
                this._curSheet.RemoveRow(this._curRow);
                this._curRow = this._curSheet.GetRow(this._curIndex);
            }
        }


        public void Read(string fileName)
        {
            this._workbook = WorkbookFactory.Create(fileName);
            this._curSheet = this._workbook.GetSheetAt(0);
            this._curRow = this._curSheet.GetRow(0);
        }

        public void ReadSheetAt(int index)
        {
            this._curSheet = this._workbook.GetSheetAt(index);
            this._curRow = this._curSheet.GetRow(0);
            this._curIndex = 0;
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
            }

            using (FileStream file = new FileStream(fileName, FileMode.Open))
            {
                bt = new byte[file.Length];
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
