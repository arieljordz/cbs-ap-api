using CbsAp.Application.Abstractions.Services.Reports;
using ClosedXML.Excel;
using System.ComponentModel.DataAnnotations;

namespace CbsAp.Infrastracture.Utility.ExcelGenerator
{
    public class ExcelService : IExcelService
    {
        public byte[] GenerateExcel<T>(List<T> data, string worksheetName, bool orderByDisplay = false)
        {
            if (data == null || !data.Any())
            {
                return new byte[0];
            }
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add(worksheetName);
                var properties = typeof(T).GetProperties();

                if (orderByDisplay)
                {
                    properties = properties
                        .OrderBy(p => p.GetCustomAttributes(typeof(DisplayAttribute), true)
                                       .Cast<DisplayAttribute>()
                                       .FirstOrDefault()?.Order ?? int.MaxValue)
                        .ToArray();
                }

                int columnCount = properties.Length;

                // Add headers
                for (int i = 0; i < columnCount; i++)
                {
                    worksheet.Cell(1, i + 1).SetValue(properties[i].Name)
                        .Style.Font.SetBold(true);
                }

                var values = new List<object[]>();

                foreach (var item in data)
                {
                    values.Add(properties.Select(p => p.GetValue(item)?.ToString() ?? " ").ToArray());
                }

                // bulk data
                worksheet.Cell(2, 1).InsertData(values);

                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }
    }
}