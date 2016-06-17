
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace Peacock.Common.Helper
{
    public class ExcelHelper
    {
        private ExcelHelper()
        {

        }

        /// <summary>
        /// 读取DataColumn时执行的方法
        /// </summary>
        /// <param name="column"></param>
        /// <param name="columnvalue"></param>
        /// <returns></returns>
        public delegate object ReadColumn(DataColumn column, object columnvalue);

        /// <summary>
        /// 读书datatable的DataColumn时，当DataColumn不存在时执行的方法
        /// </summary>
        /// <param name="columnname"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        public delegate object NotExistColumn(string columnname, DataRow row);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="table">要导出的datatable</param>
        /// <param name="title">datatable对应的excel的标题</param>
        /// <param name="DoinginReadColumn">对读取的Column的值的转换方式</param>
        /// <param name="DoinginNotExistColumn">在读取时发现不存在此Column，则执行此函数。如果此参数为null，则抛出异常</param>
        /// <returns></returns>
        public static byte[] DataTableToExcel(DataTable table, Dictionary<string, string> title, ReadColumn DoinginReadColumn = null, NotExistColumn DoinginNotExistColumn = null)
        {
            HSSFWorkbook book = new HSSFWorkbook();
            ISheet sheet = book.CreateSheet("sheet1");
            int step = 60000;
            IRow row = null;
            ICell cell = null;
            List<DataRow> dataRows = table.Rows.Cast<DataRow>().ToList();

            row = sheet.CreateRow(0);
            List<string> excelTitles = title.Keys.ToList();
            for (int i = 0; i < excelTitles.Count; i++)
            {
                string column = excelTitles[i];
                cell = row.CreateCell(i);
                cell.SetCellValue(column);
            }

            for (int i = 0; i < dataRows.Count; i++)
            {
                row = sheet.CreateRow(i + 1);
                if ((i + 1) % step == 0)
                {
                    int sheetindex = i / step + 1;
                    sheet = book.CreateSheet("sheet" + sheetindex);
                }
                DataRow dataRow = dataRows[i];
                List<DataColumn> dataColumns = dataRow.Table.Columns.Cast<DataColumn>().ToList();
                int cellindex = 0;
                foreach (string key in title.Values)
                {
                    DataColumn column = dataColumns.FirstOrDefault(x => x.ColumnName == key);
                    object value = null;
                    if (column == null)
                    {
                        if (DoinginNotExistColumn == null)
                            throw new Exception(string.Format("不存在列{0}", key));
                        value = DoinginNotExistColumn(key, dataRow);
                        goto finish;
                    }
                    value = dataRow[column.ColumnName];
                    if (DoinginReadColumn != null)
                        value = DoinginReadColumn(column, value);
                    if (value is DBNull || value == null)
                        value = string.Empty;
                finish:
                    cell = row.CreateCell(cellindex++);
                    cell.SetCellValue(value.ToString());
                }
            }
            MemoryStream stream = new MemoryStream();
            book.Write(stream);
            return stream.ToArray();
        }
    }
}
