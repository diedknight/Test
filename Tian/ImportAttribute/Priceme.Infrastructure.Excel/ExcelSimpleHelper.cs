﻿using NPOI.HSSF.UserModel;
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

        private int _readIndex = 0;
                
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

        public void WriteLine(params string[] dataList)
        {
            //init
            if (this._workbook == null)
            {
                this.Init();
            }

            for (int i = 0; i < dataList.Length; i++)
            {
                this._curRow.CreateCell(i).SetCellValue(dataList[i]);
            }

            //下一行
            this._curRow = this._curSheet.CreateRow(this._curRow.RowNum + 1);
        }

        public List<string> ReadLine(int startIndex, int length)
        {
            List<string> list = new List<string>();

            for (int i = startIndex; i < length; i++)
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

            this._readIndex++;
            this._curRow = this._curSheet.GetRow(this._readIndex);

            return list;
        }

        public void Read(string fileName)
        {
            this._workbook = WorkbookFactory.Create(fileName);
            this._curSheet = this._workbook.GetSheetAt(0);
            this._curRow = this._curSheet.GetRow(0);
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
