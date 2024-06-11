using ExcelHandler.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExcelHandler.Controllers
{
    public class ExcelController : Controller
    {
        // GET: Excel
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Handler(HttpPostedFileBase importFile)
        {
            var dt = ToDataTable(importFile);
            var voucherList = new List<Voucher>();
            foreach (DataRow row in dt.Rows)
            {
                voucherList.Add(new Voucher
                {
                    VoucherNo = row.ItemArray[0].ToString(),
                    Date=row.ItemArray[1].ToString(),
                    SalesAccount= row.ItemArray[2].ToString(),
                    CustomerName = row.ItemArray[3].ToString(),
                    Items = row.ItemArray[4].ToString(),
                    Quantity = int.Parse(row.ItemArray[5].ToString()),
                    Price = row.ItemArray[6].ToString(),
                    Discount = row.ItemArray[7].ToString(),
                    Remarks = row.ItemArray[8].ToString(),

                });
            }
            return View();
        }
        private DataTable ToDataTable(HttpPostedFileBase importFile)
        {
            string fileName = Path.GetFileName(importFile.FileName);
            string filePath = Path.Combine(Server.MapPath("~/Files"), fileName);
            importFile.SaveAs(filePath);

            DataTable dt = new DataTable();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.End.Row;
                int colCount = worksheet.Dimension.Columns;

                int startRow = 1;
                for (int row = 1; row <= rowCount; row++)
                {
                    bool isEmptyRow = true;
                    for (int col = 1; col <= colCount; col++)
                    {
                        if (worksheet.Cells[row, col].Value != null)
                        {
                            isEmptyRow = false;
                            break;
                        }
                    }
                    if (!isEmptyRow)
                    {
                        startRow = row;
                        break;
                    }
                }

                int startCol = 1;
                for (int col = 1; col <= colCount; col++)
                {
                    bool isEmptyColumn = true;
                    for (int row = startRow; row <= rowCount; row++)
                    {
                        if (worksheet.Cells[row, col].Value != null)
                        {
                            isEmptyColumn = false;
                            break;
                        }
                    }
                    if (!isEmptyColumn)
                    {
                        startCol = col;
                        break;
                    }
                }

                for (int col = startCol; col <= colCount; col++)
                {
                    string columnName = worksheet.Cells[startRow, col].Value?.ToString();
                    if (!string.IsNullOrEmpty(columnName))
                        dt.Columns.Add(columnName);
                }

                for (int row = startRow + 1; row <= rowCount; row++)
                {
                    DataRow dataRow = dt.NewRow();
                    for (int col = startCol; col <= colCount; col++)
                    {
                        dataRow[col - startCol] = worksheet.Cells[row, col].Value?.ToString();
                    }
                    dt.Rows.Add(dataRow);
                }
            }

            return dt;
        }
    }
}