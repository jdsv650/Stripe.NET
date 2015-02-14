using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Office.Interop.Excel;

namespace StripeWithCustomersAndCharges.Controllers
{
    public class ProductController : Controller
    {
        List<Models.Product> products = new List<Models.Product>();

        // GET: Product
        public ActionResult Index()
        {
            Microsoft.Office.Interop.Excel.Application excelApp;
            Workbook workbook;
            Worksheet worksheet;
            // object missing = System.Reflection.Missing.Value;

            excelApp = new Microsoft.Office.Interop.Excel.Application();
            workbook = excelApp.Workbooks.Open("C:\\ElevenFiftyPractice\\StripeWithCustomersAndCharges\\Book1.xlsx", 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets.get_Item(1);

            var range2 = worksheet.UsedRange;
          //  var result = "";

            for (var r = 1; r < range2.Rows.Count + 1; r++)
            {
                Models.Product product = new Models.Product();

                for (var c = 1; c < range2.Columns.Count + 1; c++)
                {
                    product.Description = (range2.Cells[r, 1] as Microsoft.Office.Interop.Excel.Range).Value2.ToString();
                    product.Price = (range2.Cells[r, 2] as Microsoft.Office.Interop.Excel.Range).Value2;

                    //var cellAsString = "";
                    //if ((range2.Cells[r, c] as Microsoft.Office.Interop.Excel.Range).Value2 != null)
                    //{
                    //    cellAsString = (string)(range2.Cells[r, c] as Microsoft.Office.Interop.Excel.Range).Value2.ToString();
                    //}
                    //result += cellAsString;
                    //result += " ";
                }

                products.Add(product);
            }

            workbook.Close();
            excelApp.Quit();

            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                worksheet = null;
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                workbook = null;
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                excelApp = null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                worksheet = null;
            
            }
            finally
            {
                GC.Collect();
            }

            return View(products);
        }
    }
}