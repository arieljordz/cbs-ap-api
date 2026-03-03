namespace CbsAp.Application.Abstractions.Services.Reports
{
    public interface IExcelService
    {
        byte[] GenerateExcel<T>(List<T> data, string worksheetName, bool orderByDisplay = false);
    }
}