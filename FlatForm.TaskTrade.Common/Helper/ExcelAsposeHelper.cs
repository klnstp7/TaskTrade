using System;
using System.Data;
using System.Collections.Generic;
using Aspose.Cells;

namespace Peacock.Common.Helper
{
    public class ExcelAsposeHelper
    {
        /// <summary>
        /// 检查文档是否可以读取
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool FileVaild(string path)
        {
            bool result = true;
            try
            {
                Workbook sheet = new Workbook(path);
            }
            catch
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// 将Excel转成Html文件
        /// </summary>
        /// <param name="ExcelPath">excel文件路径</param>
        /// <param name="HtmlPath">要保存的Html文件路径</param>
        public static bool ExcelConvertToHtml(string ExcelPath, string HtmlPath)
        {
            bool result = false;
            try
            {
                Workbook sheet = new Workbook(ExcelPath);
                List<string> listRemoveSheetName = new List<string>();//不可见的sheet名称集合
                foreach (Worksheet item in sheet.Worksheets)
                {
                    if (!item.IsVisible)
                    {//sheet隐藏
                        listRemoveSheetName.Add(item.Name);

                    }
                }
                //移除不可见的sheet
                foreach (string SheetName in listRemoveSheetName)
                {
                    sheet.Worksheets.RemoveAt(SheetName);
                }
                sheet.Save(HtmlPath, SaveFormat.Html);
                result = true;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("将Excel转成Html文件", ex);
                result = false;
            }
            return result;

        }

        /// <summary>
        /// 数据导出到Excel
        /// </summary>
        /// <param name="FilePath">路径</param>
        /// <param name="dsSourceData">数据源</param>
        /// <returns></returns>
        public static bool DataTableToExcel(string FilePath, DataTable dsSourceData, string SheetName)
        {

            bool result = false;
            try
            {
                Workbook wb = new Workbook();

                Worksheet ws = null;
                wb.Worksheets.Add();
                //设置sheet表名称
                ws = wb.Worksheets[0];
                ws.Name = SheetName;
                ws.Cells.ImportDataTable(dsSourceData, true, 0, 0);
                wb.Save(FilePath);
                result = true;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("数据导出到Excel", ex);
                result = false;
            }

            return result;
        }
        /// <summary>
        /// 读取Excel
        /// </summary>
        /// <param name="excelFilePath"></param>
        /// <param name="totalRows"></param>
        /// <param name="totalColumns"></param>
        /// <returns></returns>
        public static DataTable ReadExcel(string excelFilePath, int totalRows, int totalColumns)
        {
            DataTable result = new DataTable();
            Workbook wb = new Aspose.Cells.Workbook(excelFilePath);
            Cells cells = wb.Worksheets[0].Cells;
            result = cells.ExportDataTable(0, 0, totalRows, totalColumns);
            return result;
        }

        public static Dictionary<string, string> GetDataMaping(string excelFilePath)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            Aspose.Cells.Workbook workbook = new Aspose.Cells.Workbook(excelFilePath);
            Worksheet sheet = workbook.Worksheets[0];
            int RowsNum = sheet.Cells.MaxRow + 1;
            for (int i = 1; i < RowsNum; i++)
            {
                string EnColumnName = sheet.Cells[i, 0].StringValue.Trim();
                string ZhColumnName = sheet.Cells[i, 2].StringValue.Trim();
                string[] arrSplit = System.Text.RegularExpressions.Regex.Split(ZhColumnName, "##");
                foreach (var itemKey in arrSplit)
                {
                    if (!result.ContainsKey(itemKey) && !string.IsNullOrWhiteSpace(itemKey))
                    {
                        result.Add(itemKey, EnColumnName);
                    }
                }
            }
            return result;
        }

    }
}
